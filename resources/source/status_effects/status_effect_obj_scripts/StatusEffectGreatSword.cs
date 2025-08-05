using Godot;
using System;

public partial class StatusEffectGreatSword : StatusEffect
{
	public override int Id => 14;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/great_sword.png");
	public override string EffectName => "Great Sword";
	public override string Description => $"Increases Critical Multiplier of Tier 2 Abilities by {_critMult}%.";
	public override bool Stackable => true;
	private float _critMult = 50;
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
			unit.GetAbility(1).ModifyCritMult(_critMult * _stackCount);
	}
}
