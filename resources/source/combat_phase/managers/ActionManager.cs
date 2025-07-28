using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class ActionManager : Node3D
{
	private EventBus _eventBus;
	private TurnManager _turnManager;
	// List datastructure is used as it allows to remove 
	// actions from any index
	private List<Unit> _playerUnitQueue = new List<Unit>();
	private List<Unit> _enemyUnitQueue = new List<Unit>();
	private List<Unit> _combinedUnitQueue = new List<Unit>();
	private GameAction _currentAction = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_turnManager = GetParent().GetNode<TurnManager>("turn_manager");
		_turnManager.OnTurnStateChanged += HandleOnTurnStateChanged;

		_eventBus = EventBus.Instance;

		_eventBus.OnTargetSelected += HandleTargetSelected;
		_eventBus.OnTargetDeselected += HandleTargetDeselected;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void EnqueueAction(GameAction action)
	{
		_currentAction = action;
	}

	private void HandleTargetSelected(Unit unit)
	{
		switch (unit.Tag)
		{
			case UnitEnums.UnitTag.Player:
				_playerUnitQueue.Add(unit);
				break;
			case UnitEnums.UnitTag.Enemy:
				_enemyUnitQueue.Add(unit);
				break;
		}
	}
	private void HandleTargetDeselected(Unit unit)
	{
		switch (unit.Tag)
		{
			case UnitEnums.UnitTag.Player:
				_playerUnitQueue.Remove(unit);
				break;
			case UnitEnums.UnitTag.Enemy:
				_enemyUnitQueue.Remove(unit);
				break;
		}
	}

	public void ActionComplete(GameAction action)
	{
		_combinedUnitQueue.Remove(action.Creator);
		ProcessNextUnit(_combinedUnitQueue);
	}

	private void ProcessNextUnit(List<Unit> unitQueue) 
	{
		if (unitQueue.Count == 0)
		{
			_turnManager.TurnResolved();
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
			if (unit.GetEnemyTarget() == null)
			{
				unitQueue.Remove(unit);
				ProcessNextUnit(unitQueue);
				return;
			}
		}

		GameAction unitAction = new UnitAttackAction(this, unit);

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

	private async Task HandleOnTurnStateChanged(TurnState state)
	{
		if(state == TurnState.InProgress)
			QueueUnitActions();

		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_turnManager.OnTurnStateChanged -= HandleOnTurnStateChanged;

		_eventBus.OnTargetSelected -= HandleTargetSelected;
		_eventBus.OnTargetDeselected -= HandleTargetDeselected;
		base._ExitTree();
	}
}
