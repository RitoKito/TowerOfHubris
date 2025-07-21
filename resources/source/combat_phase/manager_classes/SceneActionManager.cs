using Godot;
using System.Collections.Generic;

public partial class SceneActionManager : Node3D
{
	public static SceneActionManager Instance { get; private set; }


	public delegate void ActionDelegate();

	private SceneManager _sceneManager;
	private Messenger _messenger;
	private Queue<GameAction> _gameActionQueue = new Queue<GameAction>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		_messenger = Messenger.Instance;
		_sceneManager = SceneManager.Instance;

		_messenger.OnResolveRound += QueueUnitActions;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void EnqueueTask(GameAction action)
	{
		_gameActionQueue.Enqueue(action);
	}

	private void ProcessNextTask()
	{
		GameAction a = _gameActionQueue.Dequeue();
		GD.Print("Tasking");
		a.Execute(() =>
		{
			if (_gameActionQueue.Count > 0)
			{
				ProcessNextTask();
			}
		});
	}

	private void QueueUnitActions()
	{
		GD.Print(_sceneManager.PlayerUnits.Count);
		foreach (Unit unit in _sceneManager.PlayerUnits)
		{
			unit.HideTargetingUI();
			// TODO ALLOW ONLY WHEN ALL UNITS HAVE TARGET
			// TODO Make interface for GameAction
			if (unit.GetEnemyTarget() != null)
			{
				GameAction unitAction = new UnitAttackAction(unit);
				AddChild((unitAction));
				EnqueueTask(unitAction);
			}
		}

		ProcessNextTask();
	}
}
