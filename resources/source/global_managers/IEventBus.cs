
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

	public event Func<GameState, Task> OnGameStateChanged;

	public event Func<Task> OnEnterGame;

	public event Func<Task> OnLevelTreeLoaded;

	public event Func<Task> OnEnterCombat;
	public event Func<Task> OnCombatSceneLoaded;
	public event Func<List<StatusEffect>, Task> OnPlayerStatusEffectsApply;
	public event Func<List<StatusEffect>, Task> OnEnemyStatusEffectsApply;
	public event Func<Task> OnRewardSelection;
	public event Func<StatusEffect, Task> OnRewardSelected;

	public event Func<StatusEffect, Task> OnPlayerPermanentEffectAdded;
	public event Func<StatusEffect, Task> OnEnemyPermanentEffectAdded;

	public event Func<List<StatusEffect>, Task> OnAssignRewards;

	public event Func<int, Task> OnNewFloor;

	public event Func<Task> OnDefeat;

	public event Func<Task> OnRestart;

	public void EmitMouseLeftClicked(Dictionary clickedObject);
	public void EmitMouseLeftReleased(Dictionary clickedObject);


	public void EmitTargetSelected(Unit emitter);
	public void EmitTargetDeselected(Unit emitter);
	public void EmitUnitDied(Unit unit);


	public void EmitExecuteTurn();
	public void EmitTurnInProgress();
	public void EmitNewTurn(int turnCount);


	public Task EmitGameStateChanged(GameState state);

	public Task EmitEnterGame();

	public Task EmitLevelTreeLoaded();

	public Task EmitEnterCombat();
	public Task EmitCombatSceneLoaded();
	public Task EmitPlayerApplyStatusEffects(List<StatusEffect> effects);
	public Task EmitEnemyApplyStatusEffects(List<StatusEffect> effects);
	public Task EmitRewardSelection();
	public Task EmitRewardSelected(StatusEffect selectedReward);

	public Task EmitPlayerPermanentEffectAdded(StatusEffect selectedPermanentEffect);
	public Task EmitEnemyPermanentEffectAdded(StatusEffect selectedPermanentEffect);

	public Task EmitAssignRewards(List<StatusEffect> effects);

	public Task EmitNewFloor(int currentFloor);

	public Task EmitDefeat();

	public Task EmitRestart();
}
