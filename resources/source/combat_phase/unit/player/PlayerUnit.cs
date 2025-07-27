using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerUnit : Unit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_messenger.OnPlayerStatusEffectsApply += HandlePlayerStatusEffectApply;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	private void HandlePlayerStatusEffectApply(List<StatusEffect> statusEffects)
	{
		foreach (StatusEffect effect in statusEffects)
		{
			_statusEffectController.AddStatusEffect(effect);
		}
	}

	public override void _ExitTree()
	{
		_messenger.OnPlayerStatusEffectsApply -= HandlePlayerStatusEffectApply;
		base._ExitTree();
	}
}
