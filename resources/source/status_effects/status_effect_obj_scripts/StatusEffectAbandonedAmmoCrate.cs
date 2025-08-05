using Godot;
using System;

public partial class StatusEffectAbandonedAmmoCrate : StatusEffect
{
	public override int Id => 6;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/abandoned_ammo_crate.png");
	public override string EffectName => "Abandoned Ammo Crate";
	public override string Description => $"Increases Damage of Tier 2 Abilities by {_minAttackIncreasePercent}%.";
	public override bool Stackable => true;
	private float _minAttackIncreasePercent = 30;

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
			unit.GetAbility(1).ModifyDamagePercent(_minAttackIncreasePercent * _stackCount);
	}
}
