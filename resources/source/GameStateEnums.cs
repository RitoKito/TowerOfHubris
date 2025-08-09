public enum GameState
{
	MainMenu,
	LevelTree,
	CombatInit,
	Combat,
	CombatRewardSelection,
	Defeat,
}

public enum TurnState
{
	SceneInitialization,
	PlayerTurn,
	InProgress,
	TurnResolved,
	SceneComplete,
	Defeat,
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
