using Godot;
using System.Collections.Generic;

public partial class ActionManager : Node3D
{
	public static ActionManager Instance { get; private set; }


	public delegate void ActionDelegate();

	private SceneManager _sceneManager;
	private Messenger _messenger;
	private Queue<IGameAction> _gameActionQueue = new Queue<IGameAction>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else
			Free();

		_messenger = Messenger.Instance;
		_sceneManager = SceneManager.Instance;

		_messenger.OnResolveTurn += QueueUnitActions;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void EnqueueAction(IGameAction action)
	{
		_gameActionQueue.Enqueue(action);
	}

	private void ProcessNextAction()
	{
		if (_gameActionQueue.Count == 0)
		{
            _messenger.EmitTurnResolved();
            return;
        }

        IGameAction a = _gameActionQueue.Dequeue();
		GD.Print("Tasking");
		a.Execute(() =>
		{
			ProcessNextAction();
        });
	}

	private void QueueUnitActions()
	{
		foreach (Unit unit in _sceneManager.GetAllUnits())
		{
			unit.HideTargetingUI();
			// TODO ALLOW ONLY WHEN ALL UNITS HAVE TARGET
			// TODO Make interface for GameAction
			if (unit.GetEnemyTarget() != null)
			{
				IGameAction unitAction = new UnitAttackAction(unit);
				AddChild(unitAction as Node);
				EnqueueAction(unitAction);
			}
		}


		ProcessNextAction();
	}
}
