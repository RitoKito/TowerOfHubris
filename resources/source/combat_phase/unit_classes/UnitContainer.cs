using Godot;
using System;

public abstract partial class UnitContainer : Node
{
	// Base class for UnitContainerPlayer and UnitContainerEnemy

	[Export]
	protected Node3D[] _unitPosArray;
	public Node3D[] UnitPosArray {  get { return _unitPosArray; } }
	protected Unit[] _unitArray = new Unit[4];
	public Unit[] UnitArray { get { return _unitArray; } }

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

	public virtual void PopulateContainer() { }
}
