using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node3D
{
	private EventBus _eventBus = null;
	private UIManager _uiManager = null;
	private AnimationPlayer _mainMenuAnimationPlayer = null;
	private AudioStreamPlayer _mainMenuAudioStreamPlayer = null;

	private GameState _gameState;
	private GameState _transitionToState;
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_uiManager = GetNode<UIManager>("ui_manager");

		_eventBus.OnEnterGame += HandleOnEnterGame;
		_eventBus.OnEnterCombat += HandleOnEnterCombat;
		_eventBus.OnRewardSelected += HandleOnRewardSelected;
		_eventBus.OnRestart += HandleOnRestart;

		_mainMenuAnimationPlayer = GetNode<AnimationPlayer>("tower_entrance/animation_player_main_menu");
		_mainMenuAudioStreamPlayer = GetNode<AudioStreamPlayer>("tower_entrance/audio_stream_player_main_menu");

		//SetGameState(GameState.LevelTree);
		//SetGameState(GameState.MainMenu);

		_gameState = GameState.MainMenu;
		await _eventBus.EmitGameStateChanged(_gameState);
		await _uiManager.PlayFadeToNormal();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void SetGameState(GameState state)
	{
		_gameState = state;

		await _uiManager.PlayFadeToBlack();

		await _eventBus.EmitGameStateChanged(_gameState);

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

	private async Task HandleOnEnterGame()
	{
		if(_gameState == GameState.MainMenu && _mainMenuAnimationPlayer != null)
		{
			_mainMenuAnimationPlayer.Play("main_menu_start");
			_mainMenuAudioStreamPlayer.Play();
			await ToSignal(_mainMenuAnimationPlayer, "animation_finished");
		}

		SetGameState(GameState.LevelTree);
		await Task.Yield();
	}

	private async Task HandleOnRestart()
	{
		SetGameState(GameState.LevelTree);
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnEnterGame -= HandleOnEnterGame;
		_eventBus.OnEnterCombat -= HandleOnEnterCombat;
		_eventBus.OnRewardSelected -= HandleOnRewardSelected;
		_eventBus.OnRestart -= HandleOnRestart;
		base._ExitTree();
	}
}
