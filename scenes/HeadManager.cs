using Godot;
using System;

public partial class HeadManager : Node3D
{
	public static HeadManager Instance { get; private set; }

	private InputHandler _inputHandler;
	public InputHandler InputHandler {  get { return _inputHandler; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		_inputHandler = GetNode<InputHandler>("InputHandler");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
