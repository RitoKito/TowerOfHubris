using Godot;
using System;
using System.Collections.Generic;

public partial class SceneManager : Node3D
{
	//public static SceneManager Instance { get; private set; }

	private Messenger _messenger;

	private readonly List<PlayerUnit> _playerUnits = new List<PlayerUnit>();
	private List<PlayerUnit> _alivePlayerUnits = new List<PlayerUnit>();

	private readonly List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
	private List<EnemyUnit> _aliveEnemyUnits = new List<EnemyUnit>();
	public IReadOnlyList<EnemyUnit> GetEnemyUnitsAlive()
	{
		return _aliveEnemyUnits.AsReadOnly();
	}

	public IReadOnlyList<PlayerUnit> GetPlayerUnitsAlive()
	{
		return _alivePlayerUnits.AsReadOnly();
	}

	private readonly List<Unit> _allUnits = new List<Unit>();

	private int _playerUnitsAlive = 0;
	private int _enemyUnitsAlive = 0;

	public IReadOnlyList<Unit> GetAllUnits()
	{
		return _allUnits.AsReadOnly();
	}

	private bool _processingTask = false;

	public void AppendPlayerUnit(PlayerUnit unit)
	{
		_playerUnits.Add(unit);
	}
	public void AppendEnemyUnit(EnemyUnit unit)
	{
		_enemyUnits.Add(unit);
	}

	private Unit GetRandomPlayerUnit()
	{
		Random rnd = new Random();
		int index = rnd.Next(0, _alivePlayerUnits.Count);
		return _alivePlayerUnits[index];
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_messenger = Messenger.Instance;
		_messenger.OnTurnStateChanged += HandleTurnStateChanged;
		_messenger.OnUnitDeath += HandleUnitDeath;
		//_messenger.OnGameStateChanged += HandleGameStateChanged;


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

		_messenger.EmitCombatSceneLoaded();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

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
				AppendPlayerUnit(playerUnit);
			}
		}
	}

	private void InstantiateEnemyUnits()
	{
		PackedScene enemyContainer = GD.Load<PackedScene>(PathConstants.CONTAINER_UNIT_ENEMY);
		Node enemyContainerInstance = enemyContainer.Instantiate();
		var enemyUnitContainer = enemyContainerInstance as UnitContainerEnemy;
		enemyUnitContainer.PopulateContainer();
		AddChild(enemyContainerInstance);

		foreach (EnemyUnit enemyUnit in enemyUnitContainer.UnitArray)
		{
			if (enemyUnit != null)
			{
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
				_messenger.EmitTargetSelected(unit);
			}
		}
	}

	private void HandleTurnStateChanged(TurnState state)
	{
		if(state == TurnState.PlayerTurn)
			TargetRandomPlayerUnits();
	}

	private void HandleUnitDeath(Unit unit)
	{
		if (unit.GetGroups().Contains("PlayerUnit"))
		{
			_playerUnitsAlive--;
			_alivePlayerUnits.Remove(unit as PlayerUnit);
			if(_playerUnitsAlive <= 0)
			{
				GD.Print("Player Lost");
			}
		}
		else if (unit.GetGroups().Contains("EnemyUnit"))
		{
			_enemyUnitsAlive--;
			_aliveEnemyUnits.Remove(unit as EnemyUnit);
			if (_enemyUnitsAlive <= 0)
			{
				GD.Print("Player Won");
			}
		}
	}

	public void HandleGameStateChanged(GameState state)
	{
		GD.Print("TO");

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
		_messenger.OnTurnStateChanged -= HandleTurnStateChanged;
		_messenger.OnUnitDeath -= HandleUnitDeath;
	}
}
