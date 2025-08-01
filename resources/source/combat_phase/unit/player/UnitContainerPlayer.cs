using Godot;
using System;

public partial class UnitContainerPlayer : UnitContainer
{
	public override void PopulateContainer()
	{
		foreach (string path in PathConstants.PLAYER_UNITS)
		{
			GD.Print(path);
			PackedScene playerUnitPrefab = GD.Load<PackedScene>(path);
			Node playerUnitInstance = playerUnitPrefab.Instantiate();
			Unit playerUnit = playerUnitInstance as Unit;
			
			_unitPosArray[playerUnit.UnitPos].AddChild(playerUnit);
			_unitArray[playerUnit.UnitPos] = playerUnit;
		}
	}
}
