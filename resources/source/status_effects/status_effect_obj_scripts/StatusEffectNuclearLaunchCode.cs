using Godot;
using System;

public partial class StatusEffectNuclearLaunchCode : StatusEffect
{
	public override int Id => 7;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/nuclear_launch_codes.png");
	public override string EffectName => "Nuclear Launch Code";
	public override string Description => $"Increases Damage of Tier 3 Abilities by {_minAttackIncreasePercent}%.";
	public override bool Stackable => true;
	private float _minAttackIncreasePercent = 50;
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
			unit.GetAbility(2).ModifyDamagePercent(_minAttackIncreasePercent * _stackCount);
	}
}
