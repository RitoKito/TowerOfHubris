using Godot;
using System;

public partial class PlayerUnitContainer : Node3D
{
	[Export]
	private Node3D[] _unitPosArray;
	private Unit[] _unitArray = new Unit[4];
	public Unit[] UnitArray {  get { return _unitArray; } }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		for(int i = 0; i < 4; i++)
		{
			if(_unitPosArray[i].GetChild(0) != null)
			{
				_unitArray[i] = _unitPosArray[i].GetChild(0) as Unit;
			}
			else
			{
				GD.PrintErr("WARNING! Player Unit Container Missing a Unit");
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
