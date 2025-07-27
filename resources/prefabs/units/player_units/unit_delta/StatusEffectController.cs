using Godot;
using System;
using System.Collections.Generic;

public partial class StatusEffectController : Node3D
{
	private List<StatusEffect> _statusEffects = new List<StatusEffect>();
	private Unit owner = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		owner = (Unit)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TickAll()
	{
		foreach (var statusEffect in _statusEffects)
		{
			statusEffect.Tick();

			if(statusEffect.IsExpired)
				_statusEffects.Remove(statusEffect);
		}
	}

	public void AddStatusEffect(StatusEffect statusEffect)
	{
		if(_statusEffects.Contains(statusEffect) && !statusEffect.Stackable)
		{
			GD.PrintErr("This Status Effect Cannot be stacked");
			return;
		}

		statusEffect.OnApply(owner);
		_statusEffects.Add(statusEffect);
	}

	public void RemoveStatusEffect(StatusEffect statusEffect)
	{
		statusEffect.OnExpire(owner);
		_statusEffects.Remove(statusEffect);
	}
}
