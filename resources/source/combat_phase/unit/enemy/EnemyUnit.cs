using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyUnit : Unit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void TargetPlayerUnit(Unit target)
	{
		//_enemyTarget = target;
		//_unitUIManager.SetTargetCurveTarget = _enemyTarget;
		//DrawTargetingCurve();

		SetEnemyTarget(target);
	}
}
