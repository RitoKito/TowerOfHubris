using Godot;
using System;
using System.Diagnostics;

public partial class UnitDetails : Node3D
{
	private Sprite3D selectSprite = null;
	private HpDetails hpDetails = null;

	[Export]
	private string unitName = "placeHolder";
	public string getUnitName () { return unitName; }

	[Export]
	private int maxHp = 10;
	private int currentHp;
	private int attackValue = 2;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		selectSprite = GetNode<Sprite3D>("char_select_spr");

		hpDetails = GetNode<HpDetails>("hp_label");
		currentHp = maxHp;

		hpDetails.updateHp(currentHp.ToString() + "/" + maxHp.ToString());

		showSelectSpr(false);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void showSelectSpr(bool state)
	{
		if (state)
		{
			selectSprite.Visible = true;
		}
		else
		{
			selectSprite.Visible = false;
		}
	}

	private void _on_static_body_3d_mouse_entered()
	{
		showSelectSpr(true);
	}

	private void _on_static_body_3d_mouse_exited()
	{
		showSelectSpr(false);
	}
}
