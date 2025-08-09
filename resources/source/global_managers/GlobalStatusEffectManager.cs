using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GlobalStatusEffectManager : Node3D
{
	private EventBus _eventBus = null;

	private List<StatusEffect> _playerStatusEffects = new List<StatusEffect>();
	private List<StatusEffect> _enemyStatusEffects = new List<StatusEffect>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnCombatSceneLoaded += HandleOnCombatSceneLoaded;
		_eventBus.OnRewardSelected += HandleOnRewardSelected;
		_eventBus.OnNewFloor += HandleOnNewFloor;
		_eventBus.OnRestart += HandleOnRestart;
		_eventBus.OnDefeat += HandleOnRestart;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task AddPlayerStatusEffect(StatusEffect statusEffect)
	{
		for (int i = 0; i < _playerStatusEffects.Count; i++)
		{
			if (_playerStatusEffects[i].Id == statusEffect.Id)
			{
				_playerStatusEffects[i].AddStacks(stackCount: 1);
				await _eventBus.EmitPlayerPermanentEffectAdded(_playerStatusEffects[i]);
				return;
			}
		}

		_playerStatusEffects.Add(statusEffect);

		await _eventBus.EmitPlayerPermanentEffectAdded(statusEffect);

		await Task.Yield();
	}

	private void RemovePlayerStatusEffect(StatusEffect statusEffect)
	{
		_playerStatusEffects.Remove(statusEffect);
	}

	private async Task AddEnemyStatusEffect(StatusEffect statusEffect)
	{
		for (int i = 0; i < _enemyStatusEffects.Count; i++)
		{
			if (_enemyStatusEffects[i].Id == statusEffect.Id)
			{
				_enemyStatusEffects[i].AddStacks(stackCount: 1);
				await _eventBus.EmitEnemyPermanentEffectAdded(_enemyStatusEffects[i]);
				return;
			}
		}

		_enemyStatusEffects.Add(statusEffect);

		await _eventBus.EmitEnemyPermanentEffectAdded(statusEffect);

		await Task.Yield();
	}

	private void RemoveEnemyStatusEffect(StatusEffect statusEffect)
	{
		_enemyStatusEffects.Remove(statusEffect);
	}

	public async Task HandleOnCombatSceneLoaded()
	{
		await _eventBus.EmitPlayerApplyStatusEffects(_playerStatusEffects);
		await _eventBus.EmitEnemyApplyStatusEffects(_enemyStatusEffects);
		await _eventBus.EmitCombatSceneReady();
		await Task.Yield();
	}

	private async Task HandleOnRewardSelected(StatusEffect selectedReward)
	{
		await AddPlayerStatusEffect(selectedReward);
		await Task.Yield();
	}

	private async Task HandleOnNewFloor(int currentFloor){
		await AddPlayerStatusEffect(new StatusEffectCorrosion());
		await AddEnemyStatusEffect(new StatusEffectAmplifiedDarkness());
	}

	private async Task HandleOnRestart()
	{
		_playerStatusEffects.Clear();
		_enemyStatusEffects.Clear();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		//_eventBus.OnCombatSceneLoaded -= HandleOnCombatSceneLoaded;
		_eventBus.OnRewardSelected -= HandleOnRewardSelected;
		_eventBus.OnNewFloor -= HandleOnNewFloor;
		_eventBus.OnRestart -= HandleOnRestart;
		_eventBus.OnDefeat -= HandleOnRestart;
		base._ExitTree();
	}
}
