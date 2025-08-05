using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SceneManager : Node3D
{
	private TurnManager _turnManager;
	private EventBus _eventBus;
	private LevelTreeManager _levelTreeManager;

	private readonly List<PlayerUnit> _playerUnits = new List<PlayerUnit>();
	private List<PlayerUnit> _alivePlayerUnits = new List<PlayerUnit>();
	private int _playerUnitsAlive = 0;

	private readonly List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
	private List<EnemyUnit> _aliveEnemyUnits = new List<EnemyUnit>();
	private int _enemyUnitsAlive = 0;

	private readonly List<Unit> _allUnits = new List<Unit>();

	public void AppendPlayerUnit(PlayerUnit unit)
	{
		_playerUnits.Add(unit);
	}
	public void AppendEnemyUnit(EnemyUnit unit)
	{
		_enemyUnits.Add(unit);
	}

	public IReadOnlyList<EnemyUnit> GetEnemyUnitsAlive()
	{
		return _aliveEnemyUnits.AsReadOnly();
	}

	public IReadOnlyList<PlayerUnit> GetPlayerUnitsAlive()
	{
		return _alivePlayerUnits.AsReadOnly();
	}

	public IReadOnlyList<Unit> GetAllUnits()
	{
		return _allUnits.AsReadOnly();
	}


	private Unit GetRandomPlayerUnit()
	{
		Random rnd = new Random();
		int index = rnd.Next(0, _alivePlayerUnits.Count);
		return _alivePlayerUnits[index];
	}

	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		_turnManager = GetParent().GetNode<TurnManager>("turn_manager");
		_turnManager.OnNewTurn += HandleNewTurn;

		_eventBus = EventBus.Instance;
		_eventBus.OnUnitDeath += HandleUnitDeath;

		_levelTreeManager = LevelTreeManager.Instance;

		InstantiatePlayerUnits();
		_alivePlayerUnits.AddRange(_playerUnits);
		_playerUnitsAlive = _playerUnits.Count;
		InstantiateEnemyUnits();
		_aliveEnemyUnits.AddRange(_enemyUnits);
		_enemyUnitsAlive = _enemyUnits.Count;

		_allUnits.AddRange(_playerUnits);
		_allUnits.AddRange(_enemyUnits);

		if (_playerUnits.Count == 0) 
		{
			GD.PrintErr("Warning! No player units registered in the scene!");
		}
		else if(_enemyUnits.Count == 0)
		{
			GD.PrintErr("Warning! No enemy units registered in the scene!");
		}

		await _eventBus.EmitCombatSceneLoaded();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {}

	private void InstantiatePlayerUnits()
	{
		PackedScene playerContainer = GD.Load<PackedScene>(PathConstants.CONTAINER_UNIT_PLAYER);
		Node playerContainerInstance = playerContainer.Instantiate();
		var playerUnitContainer = playerContainerInstance as UnitContainerPlayer;
		playerUnitContainer.PopulateContainer();
		AddChild(playerContainerInstance);

		foreach (PlayerUnit playerUnit in playerUnitContainer.UnitArray)
		{
			if (playerUnit != null)
			{
				playerUnit.Init(_turnManager, _eventBus);
				AppendPlayerUnit(playerUnit);
			}
		}
	}

	private void InstantiateEnemyUnits()
	{
		PackedScene enemyContainer = GD.Load<PackedScene>(PathConstants.CONTAINER_UNIT_ENEMY);
		Node enemyContainerInstance = enemyContainer.Instantiate();
		var enemyUnitContainer = enemyContainerInstance as UnitContainerEnemy;
		enemyUnitContainer.PopulateEnemyContainer(_levelTreeManager.Escalation, _levelTreeManager.CurrentLevel.IsExtreme);
		AddChild(enemyContainerInstance);

		foreach (EnemyUnit enemyUnit in enemyUnitContainer.UnitArray)
		{
			if (enemyUnit != null)
			{
				enemyUnit.Init(_turnManager, _eventBus);
				AppendEnemyUnit(enemyUnit);
			}
		}
	}

	// TODO move to enemy logic
	private void TargetRandomPlayerUnits()
	{
		if(_alivePlayerUnits.Count <= 0)
			return;

		foreach (EnemyUnit unit in _aliveEnemyUnits)
		{
			if (!unit.IsDead)
			{
				unit.TargetPlayerUnit(GetRandomPlayerUnit());
				_eventBus.EmitTargetSelected(unit);
			}
		}
	}

	private async Task HandleNewTurn(int _turnCount)
	{
		TargetRandomPlayerUnits();
		await Task.Yield();
	}

	private async void HandleUnitDeath(Unit unit)
	{
		if (unit.GetGroups().Contains("PlayerUnit"))
		{
			_playerUnitsAlive--;
			_alivePlayerUnits.Remove(unit as PlayerUnit);
			if(_playerUnitsAlive <= 0)
			{
				GD.Print("Defeat");
			}
		}
		else if (unit.GetGroups().Contains("EnemyUnit"))
		{
			_enemyUnitsAlive--;
			_aliveEnemyUnits.Remove(unit as EnemyUnit);
			if (_enemyUnitsAlive <= 0)
			{
				GD.Print("Victory");
			}
		}

		await Task.Yield();
	}

	public void HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.LevelTree:
				break;

			case GameState.Combat:
				InstantiatePlayerUnits();
				_alivePlayerUnits.AddRange(_playerUnits);
				_playerUnitsAlive = _playerUnits.Count;

				InstantiateEnemyUnits();
				_aliveEnemyUnits.AddRange(_enemyUnits);
				_enemyUnitsAlive = _enemyUnits.Count;

				_allUnits.AddRange(_playerUnits);
				_allUnits.AddRange(_enemyUnits);
				break;
		}
	}

	public override void _ExitTree()
	{
		_turnManager.OnNewTurn -= HandleNewTurn;
		_eventBus.OnUnitDeath -= HandleUnitDeath;
		base._ExitTree();
	}
}
