using Godot;
using System;

public partial class StatusEffectDivineIntervention : StatusEffect
{
	public override int Id => 9;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/divine_intervention.png");
	public override string EffectName => "Divine Intervention";
	public override string Description => $"Increases Damage Resistance of all units by {_damageResIncrease}%.";
	public override bool Stackable => true;
	private float _damageResIncrease = 5;
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
		unit.ModifyDamageResistance(_damageResIncrease * _stackCount);
	}
}
