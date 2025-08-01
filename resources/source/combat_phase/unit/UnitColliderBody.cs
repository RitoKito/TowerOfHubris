using Godot;
using System;

public partial class UnitColliderBody : StaticBody3D
{
	private Node3D unitDetails = null;
	public Node3D GetUnitDetails { get { return this.unitDetails; } }

	private Unit parentUnitDetails = null;
	public Unit GetParentUnitDetails() { return this.parentUnitDetails; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		unitDetails = GetParent() as Node3D;
		parentUnitDetails = unitDetails as Unit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
