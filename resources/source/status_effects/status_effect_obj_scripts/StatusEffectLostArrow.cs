using Godot;
using System;

public partial class StatusEffectLostArrow : StatusEffect
{
	public override int Id => 11;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/lost_arrow.png");
	public override string EffectName => "Lost Arrow";
	public override string Description => $"Increases Critical Chance of Tier 2 Abilities by {_critChance}%.";
	public override bool Stackable => true;
	private float _critChance = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void OnApply(Unit unit)
	{
		if (unit.GetAbility(1) != null)
			unit.GetAbility(1).ModifyCritChance(_critChance * _stackCount);
	}
}
