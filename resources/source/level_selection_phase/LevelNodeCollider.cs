using Godot;
using System;

public partial class LevelNodeCollider : Area3D
{
	private LevelNode _parent = null;
	public LevelNode Parent {  get { return _parent; } }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parent = GetParent<LevelNode>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
