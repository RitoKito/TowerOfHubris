using Godot;
using System;

public partial class MouseCollider : StaticBody3D
{
	Camera3D _camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetViewport().GetCamera3D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Rotation = _camera.Rotation;
	}
}
