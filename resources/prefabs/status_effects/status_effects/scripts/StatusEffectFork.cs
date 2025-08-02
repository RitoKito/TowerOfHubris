using Godot;
using System;

public partial class StatusEffectFork : StatusEffect
{
	public override int Id => 3;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/heart_icon.png");
	public override string EffectName => "Fork";
	public override string Description => $"Increase Attack of Tier 2 Abilities by {_minAttackIncrease}.";
	public override bool Stackable => true;
	private float _minAttackIncrease = 25;
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
		unit.GetAbility(1).ModifyBaseDamage(_minAttackIncrease);
	}
}
