using Godot;
using System;

public partial class HpDetails : Label3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void updateHpLabel(string value)
	{
		this.Text = value;
	}

	public void faceCamera(Vector3 cameraPos)
	{
		Vector3 target = cameraPos;

		LookAt(target);
	}
}
