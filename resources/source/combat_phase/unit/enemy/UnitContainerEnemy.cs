using Godot;
using System;

public partial class UnitContainerEnemy : UnitContainer
{
	public override void PopulateContainer()
	{
		var rnd = new Random();

		int minEnemyCount = RandomEnemyGenerator.GetMinEnemyCount(LevelTreeManager.Instance.Escalation);
		for (int i = 0; i < minEnemyCount; i++)
		{
			int enemyTier = RandomEnemyGenerator.GetRandomEnemyTier(LevelTreeManager.Instance.Escalation);
			var enemyType = rnd.Next(0, 2);
			GD.Print(enemyType);
			PackedScene enemyUnitPrefab = null;

			switch (enemyTier)
			{
				case 1:
					enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNITS_ENEMY_T1[enemyType]);
					break;
				case 2:
					enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNITS_ENEMY_T1[enemyType]);
					break;
				case 3:
					enemyUnitPrefab = GD.Load<PackedScene>(PathConstants.UNITS_ENEMY_T1[enemyType]);
					break;
			}

			Node enemyUnitInstance = enemyUnitPrefab.Instantiate();
			Unit enemyUnit = enemyUnitInstance as Unit;

			_unitPosArray[i].AddChild(enemyUnit);
			_unitArray[i] = enemyUnit;
		}
	}
}
