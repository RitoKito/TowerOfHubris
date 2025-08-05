using Godot;
using System;

public partial class StatusEffectTrident : StatusEffect
{
	public override int Id => 4;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/trident.png");
	public override string EffectName => "Trident";
	public override string Description => $"Increases Damage of Tier 3 Abilities by {_minAttackIncrease}.";
	public override bool Stackable => true;
	private float _minAttackIncrease = 50;
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
		if (unit.GetAbility(2) != null)
			unit.GetAbility(2).ModifyBaseDamage(_minAttackIncrease * _stackCount);
	}
}
