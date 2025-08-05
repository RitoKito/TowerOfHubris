using Godot;
using System;

public partial class StatusEffectOrbitalLaser : StatusEffect
{
	public override int Id => 15;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/orbital_laser.png");
	public override string EffectName => "Orbital Laser";
	public override string Description => $"Increases Critical Multiplier of Tier 3 Abilities by {_critMult}%.";
	public override bool Stackable => true;
	private float _critMult = 100;
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
			unit.GetAbility(2).ModifyCritMult(_critMult * _stackCount);
	}
}
