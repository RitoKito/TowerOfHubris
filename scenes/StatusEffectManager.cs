using Godot;
using System;
using System.Collections.Generic;

public partial class StatusEffectManager : Node3D
{
	private Messenger _meesenger = null;

	private List<StatusEffect> _playerStatusEffects = new List<StatusEffect>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_meesenger = Messenger.Instance;
		_meesenger.OnCombatSceneLoaded += HandleOnCombatSceneLoaded;
		_meesenger.OnRewardSelected += HandleOnRewardSelected;

		AddStatusEffect(new PowerOfPlaceholder());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddStatusEffect(StatusEffect statusEffect)
	{
		if (_playerStatusEffects.Contains(statusEffect))
		{
			for(int i =0; i < _playerStatusEffects.Count; i++)
			{
				if (_playerStatusEffects[i] == statusEffect)
					statusEffect.AddStacks(stackCount: 1);
			}
		}

		_playerStatusEffects.Add(statusEffect);
	}

	public void RemoveStatusEffect(StatusEffect statusEffect) 
	{
		_playerStatusEffects.Remove(statusEffect);
	}


	public void HandleOnCombatSceneLoaded()
	{
		_meesenger.EmitPlayerApplyStatusEffects(_playerStatusEffects);
	}

	public void HandleOnRewardSelected(StatusEffect selectedReward)
	{
		AddStatusEffect(selectedReward);
	}
}
