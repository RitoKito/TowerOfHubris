using Godot;
using System;

public partial class StatusEffectToothpick : StatusEffect
{
	public override int Id => 2;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/toothpick.png");
	public override string EffectName => "Toothpick";
	public override string Description => $"Increase Damage of Tier 1 Abilities by {_minAttackIncrease}.";
	public override bool Stackable => true;
	private float _minAttackIncrease = 10;
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
		if (unit.GetAbility(0) != null)
			unit.GetAbility(0).ModifyBaseDamage(_minAttackIncrease * _stackCount);
	}
}
