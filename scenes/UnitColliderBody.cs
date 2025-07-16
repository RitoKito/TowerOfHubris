using Godot;
using System;

public partial class UnitColliderBody : StaticBody3D
{
	private Node3D unitDetails = null;
	public Node3D GetUnitDetails { get { return this.unitDetails; } }

	private UnitDetails parentUnitDetails = null;
	public UnitDetails GetParentUnitDetails() { return this.parentUnitDetails; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		unitDetails = GetParent() as Node3D;
		parentUnitDetails = unitDetails as UnitDetails;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
