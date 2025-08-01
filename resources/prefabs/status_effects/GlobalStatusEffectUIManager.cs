using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GlobalStatusEffectUIManager : MarginContainer
{
	private EventBus _eventBus = null;

	private PackedScene _cachedEffectContainer = null;
	private HBoxContainer _statusEffectContainer = null;

	private List<StatusEffectUIContainer> _statusEffects = new List<StatusEffectUIContainer>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnPermanentEffectAdded += HandleOnPermanentEffectAdded;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;

		_statusEffectContainer = GetNode<HBoxContainer>("status_effect_container/margin_container/hbox_container");
		_cachedEffectContainer = GD.Load<PackedScene>(PathConstants.STATUS_EFFECT_UI_CONTAINER_GLOBAL);

		//Hide();
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
		_statusEffectContainer.AddChild(statusEffectUIContainer);
		statusEffectUIContainer.SetStatusEffect(statusEffect);
		_statusEffects.Add(statusEffectUIContainer);


		GD.Print("added");

	}

	public void ClearStatusEffects()
	{
		foreach(Node childNode in GetChildren())
		{
			childNode.QueueFree();
		}
	}

	private async Task HandleOnPermanentEffectAdded(StatusEffect statusEffect)
	{
		AddStatusEffect(statusEffect);
		await Task.Yield();
	}

	public async Task HandleOnGameStateChanged(GameState state)
	{
		/*switch (state)
		{
			case GameState.MainMenu:
				//Hide();
				break;
			case GameState.Defeat:
				//Hide();
				break;
			default:
				//Show();
				break;
		}
		*/
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnRewardSelected -= HandleOnPermanentEffectAdded;
		_eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
		base._ExitTree();
	}
}
