using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node3D
{
	private EventBus _eventBus = null;
	private UIManager _uiManager = null;

	private GameState _gameState;
	private GameState _transitionToState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_uiManager = GetNode<UIManager>("ui_manager");

		_eventBus.OnEnterCombat += HandleOnEnterCombat;
		_eventBus.OnRewardSelected += HandleOnRewardSelected;


		SetGameState(GameState.LevelTree);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void SetGameState(GameState state)
	{
		_gameState = state;

		await _uiManager.PlayFadeToBlack();

		_eventBus.EmitGameStateChanged(_gameState);

		await _uiManager.PlayFadeToNormal();
	}

	private async Task HandleOnRewardSelected(StatusEffect rewardSelected)
	{
		SetGameState(GameState.LevelTree);
		await Task.Yield();
	}

	private async Task HandleOnEnterCombat()
	{
		SetGameState(GameState.Combat);
		await Task.Yield();
	}

	private void HandleTransitionComplete()
	{
		//SetGameState();
	}
}
