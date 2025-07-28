public enum GameState
{
	MainMenu,
	LevelTree,
	Combat,
	Rewards,
	Defeat,
}

public enum TurnState
{
	SceneInitialization,
	PlayerTurn,
	InProgress,
	TurnResolved,
	SceneComplete,
}

public enum CombatOutcome
{
	Victory,
	Defeat,
}

public enum TransitionState
{
	Normal,
	Black,
}
