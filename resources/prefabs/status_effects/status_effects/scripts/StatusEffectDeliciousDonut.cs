using Godot;
using System;

public partial class StatusEffectDeliciousDonut : StatusEffect
{
	public override int Id => 1;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/heart_icon.png");
	public override string EffectName => "Delicious Donut";
	public override string Description => $"Increase Maximum HP of the team by {_maxHpIncrease} points.";
	public override bool Stackable => true;
	private float _maxHpIncrease = 50;
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
		unit.ModifyMaxHP(_maxHpIncrease * _stackCount);
	}
}
