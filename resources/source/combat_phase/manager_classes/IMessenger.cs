
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

	public event Action<GameState> OnGameStateChanged;
	public event Action OnCombatSceneConcluded;


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

	public void EmitGameStateChanged(GameState state);
	public void EmitCombatSceneConcluded();
}
