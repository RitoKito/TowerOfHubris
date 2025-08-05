using Godot;
using System;

public partial class StatusEffectHeartyMeal : StatusEffect
{
	public override int Id => 8;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/hearty_meal.png");
	public override string EffectName => "Hearty Meal";
	public override string Description => $"Increases Maximum HP of all units by {_maxHpIncrease}%.";
	public override bool Stackable => true;
	private float _maxHpIncrease = 5;
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
		unit.ModifyMaxHPPercent(_maxHpIncrease * _stackCount);
	}
}
