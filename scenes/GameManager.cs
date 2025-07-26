using Godot;
using System;

public partial class GameManager : Node3D
{
	private Messenger _messenger = null;

	private GameState _gameState;
	private GameState _transitionToState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_messenger.OnLevelSelected += HandleLevelSelected;
		_messenger.OnTransitionComplete += HandleTransitionComplete;
        _messenger.OnCombatSceneLoaded += HandleOnCombatSceneLoaded;
        _messenger.OnCombatSceneConcluded += HandleCombatSceneConcluded;
		_messenger.OnLevelTreeLoaded += HandleLevelTreeLoaded;

		_gameState = GameState.LevelTree;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void SetGameState()
    {
		if(_gameState == _transitionToState) {  return; }

        _gameState = _transitionToState;
        _messenger.EmitGameStateChanged(_gameState);
    }

	private void HandleCombatSceneConcluded(CombatOutcome outcome)
	{
		switch (outcome)
		{
			case CombatOutcome.Victory:
				_transitionToState = GameState.LevelTree;
				_messenger.EmitSceneTransition(SceneTransitionState.Black);
                break;
			case CombatOutcome.Defeat:
				//handle defeat screen
				break;
		}
	}

	private void HandleLevelSelected()
	{
		_transitionToState = GameState.Combat;
		_messenger.EmitSceneTransition(SceneTransitionState.Black);
    }

	private void HandleTransitionComplete()
	{
		SetGameState();
	}

	private void HandleOnCombatSceneLoaded()
	{
		_messenger.EmitSceneTransition(SceneTransitionState.Normal);
	}

	private void HandleLevelTreeLoaded()
	{
		_messenger.EmitSceneTransition(SceneTransitionState.Normal);
	}
}
