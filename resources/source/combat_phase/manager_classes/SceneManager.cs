using Godot;
using System;
using System.Collections.Generic;

public partial class SceneManager : Node3D
{
	public static SceneManager Instance { get; private set; }

	private Messenger _messenger;

	private readonly List<Unit> _playerUnits = new List<Unit>();
	private readonly List<Unit> _enemyUnits = new List<Unit>();
	private readonly List<Unit> _allUnits = new List<Unit>();

	private int _playerUnitCount = 0;
	private int _enemyUnitCount = 0;

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
		int index = rnd.Next(0, _playerUnitCount);
		return _playerUnits[index];
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();

		_messenger = Messenger.Instance;
		_messenger.OnTurnStateChanged += HandleTurnStateChanged;

		InstantiatePlayerUnits();
		_playerUnitCount = _playerUnits.Count;
		InstantiateEnemyUnits();
		_enemyUnitCount = _enemyUnits.Count;

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
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InstantiatePlayerUnits()
	{
		PackedScene playerContainer = GD.Load<PackedScene>(PathConstants.PLAYER_CONTAINER_PATH);
		Node playerContainerInstance = playerContainer.Instantiate();
		var playerUnitContainer = playerContainerInstance as UnitContainer;	
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
		PackedScene enemyContainer = GD.Load<PackedScene>(PathConstants.ENEMY_CONTAINER_PATH);
		Node enemyContainerInstance = enemyContainer.Instantiate();
		var enemyUnitContainer = enemyContainerInstance as UnitContainer;
		AddChild(enemyContainerInstance);

		foreach (EnemyUnit enemyUnit in enemyUnitContainer.UnitArray)
		{
			if (enemyUnit != null)
			{
				AppendEnemyUnit(enemyUnit);
			}
		}
	}

	private void TargetRandomPlayerUnits()
	{
		foreach (EnemyUnit unit in _enemyUnits)
		{
			unit.TargetPlayerUnit(GetRandomPlayerUnit());
		}
	}

	private void HandleTurnStateChanged(TurnState state)
	{
		if(state == TurnState.PlayerTurn)
			TargetRandomPlayerUnits();
	}
}
