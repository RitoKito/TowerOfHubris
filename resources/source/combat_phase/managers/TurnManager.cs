using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class TurnManager : Node3D
{
	private EventBus _eventBus;
	private SceneManager _sceneManager;
	private InputHandler _inputHandler;

	private int _turnCount = 0;
	public int TurnCount {  get { return _turnCount; } private set { } }
	private TurnState _turnState = TurnState.SceneInitialization;
	public TurnState TurnState { get { return _turnState; } private set { } }

	public event Func<int, Task> OnNewTurn;
	public event Func<TurnState, Task> OnTurnStateChanged;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnExecuteTurn += HandleOnExecuteTurn;

		_sceneManager = GetParent().GetNode<SceneManager>("scene_manager");

		_turnState = TurnState.SceneInitialization;
		BeginNewTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {}

	private async void BeginNewTurn()
	{
		await EmitNewTurn(_turnCount);

		_turnCount++;
		_eventBus.EmitNewTurn(_turnCount);
		_turnState = TurnState.PlayerTurn;

		await EmitTurnStateChanged(TurnState);
	}

	private async void HandleOnExecuteTurn()
	{
		_turnState = TurnState.InProgress;

		_eventBus.EmitTurnInProgress();

		await EmitTurnStateChanged(TurnState);
	}

	public async void TurnResolved()
	{
		_turnState = TurnState.TurnResolved;
		await EmitTurnStateChanged(_turnState);

		if (_sceneManager.GetEnemyUnitsAlive().Count <= 0)
		{
			_turnState = TurnState.SceneComplete;
			await EmitTurnStateChanged(TurnState);

			await _eventBus.EmitRewardSelection();
		}
		else if(_sceneManager.GetPlayerUnitsAlive().Count <= 0)
		{
			_turnState = TurnState.SceneComplete;
			await EmitTurnStateChanged(TurnState);
		}
		else
		{

			BeginNewTurn();
		}
	}

	public async Task EmitNewTurn(int turnCount)
	{
		if (OnNewTurn == null)
			return;

		var handlers = OnNewTurn.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach(Func<int, Task> handler in handlers)
			tasks.Add(handler.Invoke(turnCount));

		await Task.WhenAll(tasks);
	}

	public async Task EmitTurnStateChanged(TurnState state)
	{
		if (OnTurnStateChanged == null)
			return;

		var handlers = OnTurnStateChanged.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<TurnState,Task> handler in handlers)
			tasks.Add(handler.Invoke(state));

		await Task.WhenAll(tasks);
	}


	public override void _ExitTree()
	{
		_eventBus.OnExecuteTurn -= HandleOnExecuteTurn;
		base._ExitTree();
	}
}
