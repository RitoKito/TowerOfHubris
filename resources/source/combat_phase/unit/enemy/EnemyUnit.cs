using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class EnemyUnit : Unit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	public override void Init(TurnManager turnManager, IEventBus messenger)
	{
		base.Init(turnManager, messenger);
		_eventBus.OnEnemyStatusEffectsApply += HandleOnEnemyStatusEffectsApply;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void TargetPlayerUnit(Unit target)
	{
		//_enemyTarget = target;
		//_unitUIManager.SetTargetCurveTarget = _enemyTarget;
		//DrawTargetingCurve();

		SetEnemyTarget(target);
	}

	// TODO: Counter race condition
	private async Task HandleOnEnemyStatusEffectsApply(List<StatusEffect> statusEffects)
	{
		foreach (StatusEffect effect in statusEffects)
		{
			_statusEffectController.AddStatusEffect(effect);
		}

		_uiController.UpdateAbilityDetails(_currentAbility);

		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnEnemyStatusEffectsApply -= HandleOnEnemyStatusEffectsApply;
		base._ExitTree();
	}
}
