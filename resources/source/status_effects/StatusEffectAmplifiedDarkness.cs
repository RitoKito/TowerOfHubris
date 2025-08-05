using Godot;
using System;

public partial class StatusEffectAmplifiedDarkness : StatusEffect
{
	public override int Id => 17;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/heart_icon.png");
	public override string EffectName => "Amplified Darkness";
	public override string Description => $"Increases all Stats by {_statIncrease}%.";
	public override bool Stackable => true;
	private float _statIncrease = 2;
	private float _damageIncrease = 50; // percent
	private float _critMultIncrease = 5;
	private float _critChanceIncrease = 5;

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
		unit.ModifyMaxHPPercent(_statIncrease * _stackCount);
		unit.ModifyDamageResistance(_statIncrease * _stackCount);

		foreach(Ability ability in unit.Abilities)
		{
			ability.ModifyDamagePercent(_damageIncrease * _stackCount);
			ability.ModifyCritChance(_critChanceIncrease * _stackCount);
			ability.ModifyCritMult(_critMultIncrease * _stackCount * 100);
		}
	}
}
