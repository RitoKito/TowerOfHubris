using Godot;
using System;

public partial class UnitContainer : Node
{
	[Export]
	public Node3D[] _unitPosArray;
	protected Unit[] _unitArray = new Unit[4];
	public Unit[] UnitArray { get { return _unitArray; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GetUnitRefs();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GetUnitRefs()
	{
		if (UnitArray.Length == 0)
		{
			GD.PrintErr("WARNING! Unit Container Is Empty");
			return;
		}

		for (int i = 0; i < _unitArray.Length; i++)
		{
			if (_unitPosArray[i].GetChildCount() > 0)
			{
				_unitArray[i] = _unitPosArray[i].GetChild(0) as Unit;
			}
		}
	}

	public void PopulatePlayerContainer()
	{
		foreach(string path in PathConstants.PLAYER_UNITS)
		{
            PackedScene playerUnitPrefab = GD.Load<PackedScene>(path);
			Node playerUnitInstance = playerUnitPrefab.Instantiate();
			Unit playerUnit = playerUnitInstance as Unit;
			_unitPosArray[playerUnit.UnitPos].AddChild(playerUnit);
			_unitArray[playerUnit.UnitPos] = playerUnit;
        }
	}

	public void PopulateEnemyContainer()
	{
		int minEnemyCount = RandomEnemyGenerator.GetMinEnemyCount(LevelManager.Instance.Escalation);

		for(int i = 0; i < minEnemyCount; i++)
		{
			int enemyTier = RandomEnemyGenerator.GetRandomEnemyTier(LevelManager.Instance.Escalation);
			PackedScene enemyUnitPrefab = null;

            switch (enemyTier)
			{
				case 1:
					enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNIT_ENEMY_T1_PLACEHOLDER);
                    break;
				case 2:
                    enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNIT_ENEMY_T2_PLACEHOLDER);
                    break;
				case 3:
                    enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNIT_ENEMY_T3_PLACEHOLDER);
                    break;
			}

            Node enemyUnitInstance = enemyUnitPrefab.Instantiate();
            Unit enemyUnit = enemyUnitInstance as Unit;
            _unitPosArray[i].AddChild(enemyUnit);
			_unitArray[i] = enemyUnit;
        }
	}
}
