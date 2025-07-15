using Godot;
using System;

public partial class DebugTargetLabel : Label3D
{
	UnitDetails unitDetails = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		unitDetails = GetParent() as UnitDetails;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(unitDetails.GetEnemyTarget() != null)
		{
            Text = $"Target: {unitDetails.GetEnemyTarget().GetUnitName()}";
        }
		else
		{
			Text = "Target: No Target";
		}

    }
}
