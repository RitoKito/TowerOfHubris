using Godot;
using System;
using System.Runtime.Intrinsics.X86;

public partial class PowerOfPlaceholder : StatusEffect
{
	protected override int Id => -1;
	protected override Sprite2D Icon => null;
	public override string EffectName => "Power of Placeholder";
	public override string Description => $"Increase Maximum HP value by {_maxHpIncrease} points.";
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
		unit.ModifyMaxHP(_maxHpIncrease);
	}
}
