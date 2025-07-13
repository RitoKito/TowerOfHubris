using Godot;
using System;
using System.Diagnostics;

public partial class PlayerCharacter : Node3D
{
	private string charName = "placeHolder";
	private int maxHp = 10;
	private int currentHp;
	private int attackValue = 2;

	private HpDetails hpDetails = null;
	private Camera3D sceneCamera = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneCamera = GetViewport().GetCamera3D();


        hpDetails = GetNode<HpDetails>("hpDetailsLabel") as HpDetails;
		currentHp = maxHp;

		hpDetails.updateHp(currentHp.ToString() + "/" + maxHp.ToString());

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }

	
}
