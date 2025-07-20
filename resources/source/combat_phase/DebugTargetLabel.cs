using Godot;
using System;

public partial class DebugTargetLabel : Label3D
{
	Unit unit = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		unit = GetParent() as Unit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(unit.GetEnemyTarget() != null)
		{
            Text = $"Target: {unit.GetEnemyTarget().UnitName}";
        }
		else
		{
			Text = "Target: No Target";
		}

    }
}
