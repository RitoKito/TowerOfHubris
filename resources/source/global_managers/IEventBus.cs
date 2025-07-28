
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEventBus
{
	public static EventBus Instance { get; private set; }

	public event Action<Dictionary> OnMouseLeftClick;
	public event Action<Dictionary> OnMouseLeftRelease;

	public event Action<Unit> OnTargetSelected;
	public event Action<Unit> OnTargetDeselected;
	public event Action<Unit> OnUnitDeath;

	//Signals for UI components only
	//Scene components delegated by
	//TurnManager
	public event Action OnExecuteTurn;
	public event Action OnTurnInProgress;
	public event Action<int> OnNewTurn;

	public event Action<GameState> OnGameStateChanged;
	public event Func<Task> OnLevelTreeLoaded;
	public event Func<Task> OnEnterCombat;
	public event Func<Task> OnCombatSceneLoaded;
	public event Func<List<StatusEffect>, Task> OnPlayerStatusEffectsApply;
	public event Func<Task> OnRewardSelection;
	public event Func<StatusEffect, Task> OnRewardSelected;


	public void EmitMouseLeftClicked(Dictionary clickedObject);
	public void EmitMouseLeftReleased(Dictionary clickedObject);


	public void EmitTargetSelected(Unit emitter);
	public void EmitTargetDeselected(Unit emitter);
	public void EmitUnitDied(Unit unit);


	public void EmitExecuteTurn();
	public void EmitTurnInProgress();
	public void EmitNewTurn(int turnCount);


	public void EmitGameStateChanged(GameState state);
	public Task EmitLevelTreeLoaded();
	public Task EmitEnterCombat();
	public Task EmitCombatSceneLoaded();
	public Task EmitPlayerApplyStatusEffects(List<StatusEffect> effects);
	public Task EmitRewardSelection();
	public Task EmitRewardSelected(StatusEffect selectedReward);
}
