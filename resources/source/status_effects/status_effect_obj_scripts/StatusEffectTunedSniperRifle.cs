using Godot;
using System;

public partial class StatusEffectTunedSniperRifle : StatusEffect
{
	public override int Id => 12;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/tuned_sniper_rifle.png");
	public override string EffectName => "Tuned Sniper Rifle";
	public override string Description => $"Increases Critical Chance of Tier 3 Abilities by {_critChance}%.";
	public override bool Stackable => true;
	private float _critChance = 15;
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
		if(unit.GetAbility(2) != null)
			unit.GetAbility(2).ModifyCritChance(_critChance * _stackCount);
	}
}
