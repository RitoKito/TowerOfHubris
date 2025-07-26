using Godot;
using System;

public partial class CSHeadManager : Node3D
{
	private SceneManager _sceneManager;
	public SceneManager SceneManager {  get { return _sceneManager; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sceneManager = GetNode<SceneManager>("scene_manager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
