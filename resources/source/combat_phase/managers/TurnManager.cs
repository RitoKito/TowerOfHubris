using Godot;
using System;

public partial class TurnManager : Node3D
{
	private Messenger _messenger;
	private SceneManager _sceneManager;
	private InputHandler _inputHandler;

	private int _turnCount = 0;
	public int TurnCount {  get { return _turnCount; } private set { } }
	private TurnState _turnState = TurnState.SceneInitialization;
	public TurnState TurnState { get { return _turnState; } private set { } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_sceneManager = GetParent().GetNode<SceneManager>("scene_manager");

		//_messenger.OnResolveTurn += HandleTurnInProgress;
		_messenger.OnTurnResolved += HandleTurnResolved;
		_messenger.OnTurnInProgress += HandleTurnInProgress;

		_turnState = TurnState.SceneInitialization;
		BeginNewTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void BeginNewTurn()
	{
		_turnCount++;
		_turnState = TurnState.PlayerTurn;
		_messenger.EmitNewTurn(_turnCount);
		_messenger.EmitTurnStateChanged(TurnState);
	}

	private void HandleTurnInProgress()
	{
		_turnState = TurnState.InProgress;
		_messenger.EmitTurnStateChanged(TurnState);
	}
	private void HandleTurnResolved()
	{
		if(_sceneManager.GetEnemyUnitsAlive().Count <= 0)
		{
			_turnState = TurnState.SceneComplete;
			_messenger.EmitTurnStateChanged(TurnState);
			_messenger.EmitRewardScreen();
			//_messenger.EmitExitCombat(CombatOutcome.Victory);
		}
		else if(_sceneManager.GetPlayerUnitsAlive().Count <= 0)
		{
			_turnState = TurnState.SceneComplete;
			_messenger.EmitTurnStateChanged(TurnState);


			_messenger.EmitExitCombat(CombatOutcome.Defeat);
		}
		else
		{
			BeginNewTurn();
		}
	}

	public override void _ExitTree()
	{
		_messenger.OnTurnResolved -= HandleTurnResolved;
		_messenger.OnTurnInProgress -= HandleTurnInProgress;
		base._ExitTree();
	}
}
