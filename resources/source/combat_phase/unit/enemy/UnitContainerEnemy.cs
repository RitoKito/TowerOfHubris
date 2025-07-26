using Godot;
using System;

public partial class UnitContainerEnemy : UnitContainer
{
	public override void PopulateContainer()
	{
		int minEnemyCount = RandomEnemyGenerator.GetMinEnemyCount(LevelTreeManager.Instance.Escalation);

		for (int i = 0; i < minEnemyCount; i++)
		{
			int enemyTier = RandomEnemyGenerator.GetRandomEnemyTier(LevelTreeManager.Instance.Escalation);
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

			//Messenger dependency injection
			enemyUnit.SetMessenger(Messenger.Instance);

			//_unitPosArray variable is populated in the editor
			// by dragging child node objects
			_unitPosArray[i].AddChild(enemyUnit);
			_unitArray[i] = enemyUnit;
		}
	}
}
