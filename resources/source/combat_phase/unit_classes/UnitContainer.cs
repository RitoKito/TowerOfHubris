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
        GetUnitRefs();
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
            if (_unitPosArray[i].GetChild(0) != null)
            {
                _unitArray[i] = _unitPosArray[i].GetChild(0) as Unit;
            }
        }
    }
}
