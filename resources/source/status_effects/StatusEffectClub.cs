using Godot;
using System;

public partial class StatusEffectClub : StatusEffect
{
	public override int Id => 13;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/club.png");
	public override string EffectName => "Club";
	public override string Description => $"Increases Critical Multiplier of Tier 1 Abilities by {_critMult}%.";
	public override bool Stackable => true;
	private float _critMult = 25;
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
		if(unit.GetAbility(0) != null)
			unit.GetAbility(0).ModifyCritMult(_critMult * _stackCount);
	}
}
