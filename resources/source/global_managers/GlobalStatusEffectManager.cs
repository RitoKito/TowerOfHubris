using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GlobalStatusEffectManager : Node3D
{
	private EventBus _meesenger = null;

	private List<StatusEffect> _playerStatusEffects = new List<StatusEffect>();

	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		_meesenger = EventBus.Instance;
		_meesenger.OnCombatSceneLoaded += HandleOnCombatSceneLoaded;
		_meesenger.OnRewardSelected += HandleOnRewardSelected;

		await AddStatusEffect(new PowerOfPlaceholder());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task AddStatusEffect(StatusEffect statusEffect)
	{
		for(int i = 0; i < _playerStatusEffects.Count; i++)
		{
			if (_playerStatusEffects[i].Id == statusEffect.Id)
			{
				_playerStatusEffects[i].AddStacks(stackCount: 1);
				await _meesenger.EmitPermanentEffectAdded(_playerStatusEffects[i]);
				return;
			}
		}

		_playerStatusEffects.Add(statusEffect);

		await _meesenger.EmitPermanentEffectAdded(statusEffect);

		await Task.Yield();
	}

	public void RemoveStatusEffect(StatusEffect statusEffect) 
	{
		_playerStatusEffects.Remove(statusEffect);
	}


	public async Task HandleOnCombatSceneLoaded()
	{
		await _meesenger.EmitPlayerApplyStatusEffects(_playerStatusEffects);
		await Task.Yield();
	}

	public async Task HandleOnRewardSelected(StatusEffect selectedReward)
	{
		await AddStatusEffect(selectedReward);
		await Task.Yield();
	}
}
