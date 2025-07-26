using Godot;
using System;

public partial class NodeLine : Path3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Curve = Curve.Duplicate() as Curve3D;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetPoint(Vector3 destionation)
	{
		Curve.AddPoint(ToLocal(destionation));
	}
}
