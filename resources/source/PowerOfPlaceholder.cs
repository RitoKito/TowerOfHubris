using Godot;
using System;
using System.Runtime.Intrinsics.X86;

public partial class PowerOfPlaceholder : StatusEffect
{
	public override int Id => -1;
	public override Texture2D IconTexture => (Texture2D)ResourceLoader.Load("res://resources/sprites/icons/heart_icon.png");
	public override string EffectName => "Power of Placeholder";
	public override string Description => $"Increase Maximum HP value by {_maxHpIncrease*_stackCount} points.";
	public override bool Stackable => true;
	private float _maxHpIncrease = 100f;


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
		unit.ModifyMaxHP(_maxHpIncrease*_stackCount);
	}
}
