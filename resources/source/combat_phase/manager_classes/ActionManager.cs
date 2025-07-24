using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class ActionManager : Node3D
{
	public static ActionManager Instance { get; private set; }


	public delegate void ActionDelegate();

	private SceneManager _sceneManager;
	private Messenger _messenger;
	// List datastructure is used as it allows to remove 
	// actions from any index
	private List<Unit> _playerUnitQueue = new List<Unit>();
	private List<Unit> _enemyUnitQueue = new List<Unit>();
	private List<Unit> _combinedUnitQueue = new List<Unit>();
	private GameAction _currentAction = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();

		_messenger = Messenger.Instance;
		_sceneManager = SceneManager.Instance;

		_messenger.OnTurnInProgress += QueueUnitActions;
		_messenger.OnTargetSelected += HandleTargetSelected;
		_messenger.OnTargetDeselected += HandleTargetDeselected;
		_messenger.OnActionCompleted += HandleOnActionCompleted;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.V))
		{
			_messenger.EmitTurnResolved();
		}
	}

	private void EnqueueAction(GameAction action)
	{
		_currentAction = action;
	}

	private void HandleTargetSelected(Unit unit)
	{
		switch (unit.Tag)
		{
			case UnitTag.Player:
				_playerUnitQueue.Add(unit);
				break;
			case UnitTag.Enemy:
				_enemyUnitQueue.Add(unit);
				break;
		}
	}
	private void HandleTargetDeselected(Unit unit)
	{
		switch (unit.Tag)
		{
			case UnitTag.Player:
				GD.Print("Removed");
				_playerUnitQueue.Remove(unit);
				break;
			case UnitTag.Enemy:
                GD.Print("Removed");
                _enemyUnitQueue.Remove(unit);
				break;
		}
	}

	private void ProcessNextAction()
	{
		GameAction action = _currentAction;

		action.Execute();
	}

	private void HandleOnActionCompleted(GameAction action)
	{
		_combinedUnitQueue.Remove(action.Creator);
		ProcessNextUnit(_combinedUnitQueue);
	}

	private void ProcessNextUnit(List<Unit> unitQueue) 
	{
		if (unitQueue.Count == 0)
		{
			_messenger.EmitTurnResolved();
			return;
		}

		Unit unit = unitQueue.First();

		if (unit.IsDead)
		{
			unitQueue.Remove(unit);
			ProcessNextUnit(unitQueue);
			return;
		}

		if(unit.GetEnemyTarget() == null)
		{
			unitQueue.Remove(unit);
			ProcessNextUnit(unitQueue);
			return;
		}

		// TODO ALLOW ONLY WHEN ALL UNITS HAVE TARGET
		if (unit.GetEnemyTarget().IsDead)
		{
			unit.SelectAlternativeTarget();
		}

		GameAction unitAction = new UnitAttackAction(unit, unit.Messenger);

		AddChild(unitAction);
		unitAction.Execute();
	}



	private void QueueUnitActions()
	{
		_combinedUnitQueue.AddRange(_playerUnitQueue);
		_combinedUnitQueue.AddRange(_enemyUnitQueue);
		_playerUnitQueue.Clear();
		_enemyUnitQueue.Clear();

		ProcessNextUnit(_combinedUnitQueue);
	}
}
