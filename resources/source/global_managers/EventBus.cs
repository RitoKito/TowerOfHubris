using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class EventBus : Node, IEventBus
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
	public event Func<Task> OnRewardSelection;
	public event Func<StatusEffect, Task> OnRewardSelected;

	public event Func<StatusEffect, Task> OnPermanentEffectAdded;

	public event Func<List<StatusEffect>, Task> OnAssignRewards;

	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();
	}

	public override void _Process(double delta) {}

	public void EmitMouseLeftClicked(Dictionary clickedObject)
	{
		OnMouseLeftClick?.Invoke(clickedObject);
	}

	public void EmitMouseLeftReleased(Dictionary clickedObject)
	{
		OnMouseLeftRelease?.Invoke(clickedObject);
	}



	public void EmitTargetSelected(Unit emitter)
	{
		OnTargetSelected?.Invoke(emitter);
	}

	public void EmitTargetDeselected(Unit emitter)
	{
		OnTargetDeselected?.Invoke(emitter);
	}

	public void EmitUnitDied(Unit emitter)
	{
		OnUnitDeath?.Invoke(emitter);
	}



	public void EmitExecuteTurn()
	{
		OnExecuteTurn?.Invoke();
	}

	public void EmitTurnInProgress()
	{
		OnTurnInProgress?.Invoke();
	}

	public void EmitNewTurn(int turnCount)
	{
		OnNewTurn?.Invoke(turnCount);
	}


	public async Task EmitGameStateChanged(GameState state)
	{
		if (OnGameStateChanged == null)
			return;

		var handlers = OnGameStateChanged.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<GameState, Task> handler in handlers)
			tasks.Add(handler.Invoke(state));

		await Task.WhenAll(tasks);
	}

	public async Task EmitEnterGame()
	{
		if (OnEnterGame == null)
			return;

		var handlers = OnEnterGame.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<Task> handler in handlers)
			tasks.Add(handler.Invoke());

		await Task.WhenAll(tasks);
	}

	public async Task EmitLevelTreeLoaded()
	{
		if (OnLevelTreeLoaded == null)
			return;

		var handlers = OnLevelTreeLoaded.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<Task> handler in handlers)
			tasks.Add(handler.Invoke());

		await Task.WhenAll(tasks);
	}


	public async Task EmitEnterCombat()
	{
		if (OnEnterCombat == null)
			return;

		var handlers = OnEnterCombat.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<Task> handler in handlers)
			tasks.Add(handler.Invoke());

		await Task.WhenAll(tasks);
	}

	public async Task EmitCombatSceneLoaded()
	{
		if (OnCombatSceneLoaded == null)
			return;

		var handlers = OnCombatSceneLoaded.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<Task> handler in handlers)
			tasks.Add(handler.Invoke());

		await Task.WhenAll(tasks);
	}

	public async Task EmitPlayerApplyStatusEffects(List<StatusEffect> effects)
	{
		if (OnPlayerStatusEffectsApply == null)
			return;

		var handlers = OnPlayerStatusEffectsApply.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<List<StatusEffect>, Task> handler in handlers)
			tasks.Add(handler.Invoke(effects));

		await Task.WhenAll(tasks);
	}

	public async Task EmitRewardSelection()
	{
		if (OnRewardSelection == null)
			return;

		var handlers = OnRewardSelection.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach(Func<Task> handler in handlers)
			tasks.Add(handler.Invoke());

		await Task.WhenAll(tasks);
	}

	public async Task EmitRewardSelected(StatusEffect selectedReward)
	{
		if (OnRewardSelected == null)
			return;

		var handlers = OnRewardSelected.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<StatusEffect ,Task> handler in handlers)
			tasks.Add(handler.Invoke(selectedReward));

		await Task.WhenAll(tasks);
	}

	
	public async Task EmitPermanentEffectAdded(StatusEffect statusEffect)
	{
		if (OnPermanentEffectAdded == null)
			return;

		var handlers = OnPermanentEffectAdded.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<StatusEffect, Task> handler in handlers)
			tasks.Add(handler.Invoke(statusEffect));

		await Task.WhenAll(tasks);
	}

	public async Task EmitAssignRewards(List<StatusEffect> rewards)
	{
		if (OnAssignRewards == null)
			return;

		var handlers = OnAssignRewards.GetInvocationList();

		List<Task> tasks = new List<Task>();

		foreach (Func<List<StatusEffect>, Task> handler in handlers)
			tasks.Add(handler.Invoke(rewards));

		await Task.WhenAll(tasks);
	}
}