using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class PlayerUnit : Unit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		//_eventBus.OnPlayerStatusEffectsApply += HandlePlayerStatusEffectApply;
	}

	public override void Init(TurnManager turnManager, IEventBus messenger)
	{
		base.Init(turnManager, messenger);
		_eventBus.OnPlayerStatusEffectsApply += HandlePlayerStatusEffectApply;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	private async Task HandlePlayerStatusEffectApply(List<StatusEffect> statusEffects)
	{
		foreach (StatusEffect effect in statusEffects)
		{
			_statusEffectController.AddStatusEffect(effect);
		}

		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnPlayerStatusEffectsApply -= HandlePlayerStatusEffectApply;
		base._ExitTree();
	}
}
