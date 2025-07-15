using Godot;
using System;

public partial class UnitColliderBody : StaticBody3D
{
	private Node3D parentNode = null;
	public Node3D getParentNode() { return this.parentNode; }

	private UnitDetails parentUnitDetails = null;
	public UnitDetails getParentUnitDetails() { return this.parentUnitDetails; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parentNode = GetParent() as Node3D;
		parentUnitDetails = parentNode as UnitDetails;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
