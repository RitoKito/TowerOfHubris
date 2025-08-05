using Godot;
using System;

public partial class NodeLine : Path3D
{
	private CsgPolygon3D _polygon;
	private Color _defaultPolygonColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_polygon = GetNode<CsgPolygon3D>("csg_polygon");
		_polygon.Material = _polygon.Material.Duplicate() as Material;
		StandardMaterial3D material = _polygon.Material as StandardMaterial3D;
		_defaultPolygonColor = material.AlbedoColor;

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

	public void SetLineColour(Color color) 
	{
		StandardMaterial3D material = _polygon.Material as StandardMaterial3D;
		material.AlbedoColor = color;
	}

	public void SetDefaultLineColour()
	{
		StandardMaterial3D material = _polygon.Material as StandardMaterial3D;
		material.AlbedoColor = _defaultPolygonColor;
	}
}
