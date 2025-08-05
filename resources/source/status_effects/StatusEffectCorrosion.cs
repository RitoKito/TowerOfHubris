using Godot;
using System;

public partial class StatusEffectCorrosion : StatusEffect
{
	public override int Id => 16;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/corrosion.png");
	public override string EffectName => "Corrosion";
	public override string Description => $"Reduces all Stats by {_statReductionPerecent}%.";
	public override bool Stackable => true;
	private float _statReductionPerecent = 2;

	public override bool IsNegative => true;
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
		unit.ModifyMaxHPPercent(-_statReductionPerecent * _stackCount);
		unit.ModifyDamageResistance(-_stackCount * _stackCount);

		foreach(Ability ability in unit.Abilities)
		{
			ability.ModifyDamagePercent(-_statReductionPerecent * _stackCount);
			ability.ModifyCritChance(-_statReductionPerecent * _stackCount);
			ability.ModifyCritMult(-_statReductionPerecent * _stackCount);
		}
	}
}
