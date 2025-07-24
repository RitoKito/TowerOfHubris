using Godot;
using System;

public partial class UnitContainerPlayer : UnitContainer
{
    public override void PopulateContainer()
    {
        foreach (string path in PathConstants.PLAYER_UNITS)
        {
            PackedScene playerUnitPrefab = GD.Load<PackedScene>(path);
            Node playerUnitInstance = playerUnitPrefab.Instantiate();
            Unit playerUnit = playerUnitInstance as Unit;

            //Messenger dependency injection
            playerUnit.SetMessenger(Messenger.Instance);

            //_unitPosArray variable is populated in the editor
            // by dragging child node objects
            _unitPosArray[playerUnit.UnitPos].AddChild(playerUnit);
            _unitArray[playerUnit.UnitPos] = playerUnit;
        }
    }
}
