using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GlobalStatusEffectUIManager : MarginContainer
{
	private EventBus _eventBus = null;

	private PackedScene _cachedEffectContainer = null;
	private HBoxContainer _positiveStatusEffectContainer = null;
	private HBoxContainer _negativeStatusEffectContainer = null;

	private List<StatusEffectUIContainer> _statusEffects = new List<StatusEffectUIContainer>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnPlayerPermanentEffectAdded += HandleOnPlayerPermanentEffectAdded;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
		_eventBus.OnDefeat += HandleOnRestart;
		_eventBus.OnRestart += HandleOnRestart;

		_positiveStatusEffectContainer = GetNode<HBoxContainer>("status_effect_container/margin_container/hbox_container/positive_effects");
		_negativeStatusEffectContainer = GetNode<HBoxContainer>("status_effect_container/margin_container/hbox_container/negative_effects");

		_cachedEffectContainer = GD.Load<PackedScene>(PathConstants.STATUS_EFFECT_UI_CONTAINER_GLOBAL);

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void AddStatusEffect(StatusEffect statusEffect)
	{

		foreach(StatusEffectUIContainer effectContainer in _statusEffects)
		{
			if(effectContainer.StatusEffect.Id == statusEffect.Id)
			{
				effectContainer.UpdateStackCountLabel(statusEffect.StackCount);
				return;
			}
		}



		var statusEffectUIContainer = _cachedEffectContainer.Instantiate() as StatusEffectUIContainer;

		if (statusEffect.IsNegative)
		{
			_negativeStatusEffectContainer.AddChild(statusEffectUIContainer);
		}
		else
		{
			_positiveStatusEffectContainer.AddChild(statusEffectUIContainer);
		}

		statusEffectUIContainer.SetStatusEffect(statusEffect);
		_statusEffects.Add(statusEffectUIContainer);
	}

	public void ClearStatusEffects()
	{
		foreach(Node childNode in _positiveStatusEffectContainer.GetChildren())
		{
			childNode.QueueFree();
		}
		foreach (Node childNode in _negativeStatusEffectContainer.GetChildren())
		{
			childNode.QueueFree();
		}

		_statusEffects.Clear();
	}

	private async Task HandleOnPlayerPermanentEffectAdded(StatusEffect statusEffect)
	{
		AddStatusEffect(statusEffect);
		await Task.Yield();
	}

	public async Task HandleOnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.MainMenu:
				Hide();
				break;
			case GameState.Defeat:
				Hide();
				break;
			default:
				Show();
				break;
		}
		
		await Task.Yield();
	}

	private async Task HandleOnRestart()
	{
		ClearStatusEffects();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnRewardSelected -= HandleOnPlayerPermanentEffectAdded;
		_eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
		_eventBus.OnDefeat -= HandleOnRestart;
		_eventBus.OnRestart -= HandleOnRestart;
		base._ExitTree();
	}
}
