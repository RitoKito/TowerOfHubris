using Godot;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;

public partial class SceneManager : Node3D
{
	public static SceneManager Instance { get; private set; }
	Camera3D _cameraObj;

	private readonly List<Node> _playerUnits = new List<Node>();
	public List<Node> PlayerUnits { get{ return _playerUnits; } }

	public delegate void ActionDelegate();
	private bool _processingTask = false;

	public void AppendPlayerUnit(Node unit)
	{
		_playerUnits.Add(unit);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		_cameraObj = GetViewport().GetCamera3D();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
