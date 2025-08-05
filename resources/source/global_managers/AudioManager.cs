using Godot;
using System;
using System.Threading.Tasks;
using static Godot.WebSocketPeer;

public partial class AudioManager : Node3D
{
	private EventBus _eventBus = null;

	private AudioStreamPlayer _levelTreePlayer = null;
	private AudioStreamPlayer _combatPlayer = null;
	private AudioStreamPlayer _currentPlayer = null;

	private AudioStream _levelTreeStream = null;
	private AudioStream _combatStream = null;

	private bool _isPlayingLevelTree = false;

	private float _fadeDuration = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnRewardSelection += HandleOnRewardSelection;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
		_eventBus.OnDefeat += HandleOnDefeat;

		_levelTreePlayer = GetNode<AudioStreamPlayer>("music_player_level_tree");
		_combatPlayer = GetNode<AudioStreamPlayer>("music_player_combat");

		_levelTreeStream = GD.Load<AudioStream>(PathConstants.AUDIO_LEVEL_TREE);
		_combatStream = GD.Load<AudioStream>(PathConstants.AUDIO_COMBAT);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SwitchTracks(AudioStreamPlayer fromPlayer, AudioStreamPlayer toPlayer, GameState state)
	{
		_currentPlayer = toPlayer;

		AudioStream newStream = null;
		if (state == GameState.Combat)
			newStream = _combatStream;
		else
			newStream = _levelTreeStream;

		toPlayer.Stream = newStream;
		toPlayer.VolumeDb = -80;
		toPlayer.Play();

		var tween = CreateTween();

		tween.TweenProperty(fromPlayer, "volume_db", -80f, _fadeDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		tween.TweenProperty(toPlayer, "volume_db", -30f, _fadeDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		tween.TweenCallback(Callable.From(() => fromPlayer.Stop()));

		_isPlayingLevelTree = !_isPlayingLevelTree;
	}

	private void StopMusic()
	{
		var currentPlayer = _currentPlayer;
		var tween = CreateTween();
		var fadeDuration = 3f;
		tween.TweenProperty(currentPlayer, "volume_db", -80, fadeDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		tween.TweenCallback(Callable.From(() => currentPlayer.Stop()));

		_isPlayingLevelTree = !_isPlayingLevelTree;
	}

	private async Task HandleOnRewardSelection()
	{
		StopMusic();
		await Task.Yield();
	}

	private async Task HandleOnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.LevelTree:
				SwitchTracks(_combatPlayer, _levelTreePlayer, state);
				break;
			case GameState.Combat:
				SwitchTracks(_levelTreePlayer, _combatPlayer, state);
				break;
		}

		await Task.Yield();
	}

	private async Task HandleOnDefeat()
	{
		SwitchTracks(_combatPlayer, _levelTreePlayer, GameState.LevelTree);
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnRewardSelection -= HandleOnRewardSelection;
		_eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
		_eventBus.OnDefeat -= HandleOnDefeat;
		base._ExitTree();
	}
}
