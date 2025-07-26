
using Godot.Collections;
using System;

public interface IMessenger
{
	public static Messenger Instance { get; private set; }

	public event Action<Dictionary> OnMouseLeftClick;
	public event Action<Dictionary> OnMouseLeftRelease;

	public event Action<Unit> OnTargetSelected;
	public event Action<Unit> OnTargetDeselected;
	public event Action<Unit> OnUnitDeath;

	public event Action OnTurnInProgress;
	public event Action<GameAction> OnActionCompleted;
	public event Action OnTurnResolved;
	public event Action<TurnState> OnTurnStateChanged;


	public event Action<SceneTransitionState> OnSceneTransition;
	public event Action OnTransitionComplete;

    public event Action OnLevelSelected;
    public event Action<GameState> OnGameStateChanged;
	public event Action OnCombatSceneLoaded;
	public event Action<CombatOutcome> OnCombatSceneConcluded;
	public event Action OnLevelTreeLoaded;


	// public void EmitActionAcked();

	public void EmitMouseLeftClicked(Dictionary clickedObject);
	public void EmitMouseLeftReleased(Dictionary clickedObject);

	public void EmitTargetSelected(Unit emitter);
	public void EmitTargetDeselected(Unit emitter);
	public void EmitUnitDied(Unit unit);
	public void EmitActionCompleted(GameAction emitter);


	public void EmitTurnInProgress();
	public void EmitTurnResolved();
	public void EmitTurnStateChanged(TurnState state);


	public void EmitSceneTransition(SceneTransitionState state);
	public void EmitTransitionComplete();

    public void EmitLevelSelected();
    public void EmitGameStateChanged(GameState state);
	public void EmitCombatSceneLoaded();
	public void EmitCombatSceneConcluded(CombatOutcome outcome);
	public void EmitLevelTreeLoaded();
}
