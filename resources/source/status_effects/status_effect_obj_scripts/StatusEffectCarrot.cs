using Godot;
using System;

public partial class StatusEffectCarrot : StatusEffect
{
	public override int Id => 10;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/carrot.png");
	public override string EffectName => "Carrot";
	public override string Description => $"Increases Critical Chance of Tier 1 Abilities by {_critChance}%.";
	public override bool Stackable => true;
	private float _critChance = 5;
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
			unit.GetAbility(0).ModifyCritChance(_critChance * _stackCount);
	}
}
