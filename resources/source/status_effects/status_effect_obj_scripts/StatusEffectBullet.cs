using Godot;
using System;

public partial class StatusEffectBullet : StatusEffect
{
	public override int Id => 5;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/bullet.png");
	public override string EffectName => "Bullet";
	public override string Description => $"Increases Damage of Tier 1 Abilities by {_minAttackIncreasePercent}%.";
	public override bool Stackable => true;
	private float _minAttackIncreasePercent = 15;
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
			unit.GetAbility(0).ModifyDamagePercent(_minAttackIncreasePercent * _stackCount);
	}
}
