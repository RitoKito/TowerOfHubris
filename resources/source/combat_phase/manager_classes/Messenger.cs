using Godot;
using Godot.Collections;
using System;

public partial class Messenger : Node, IMessenger
{
	public static Messenger Instance { get; private set; }

	public event Action<Dictionary> OnMouseLeftClick;
	public event Action<Dictionary> OnMouseLeftRelease;

	public event Action<Unit> OnTargetSelected;
    public event Action<Unit> OnTargetDeselected;

    public event Action OnTurnInProgress;
	public event Action<GameAction> OnActionCompleted;
	public event Action OnTurnResolved;

	public event Action<TurnState> OnTurnStateChanged;

	public event Action<Unit> OnUnitDeath;

	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();
	}

	public override void _Process(double delta)
	{
	}

	public void EmitMouseLeftClicked(Dictionary clickedObject)
	{
		OnMouseLeftClick?.Invoke(clickedObject);
	}

	public void EmitMouseLeftReleased(Dictionary clickedObject)
	{
		OnMouseLeftRelease?.Invoke(clickedObject);
	}

	public void EmitTargetSelected(Unit emitter)
	{
		OnTargetSelected?.Invoke(emitter);
	}

	public void EmitTargetDeselected(Unit emitter)
	{
		OnTargetDeselected?.Invoke(emitter);
	}

	// Turn starts
	public void EmitTurnInProgress()
	{
		OnTurnInProgress?.Invoke();
	}

	public void EmitActionCompleted(GameAction emitter)
	{
		OnActionCompleted?.Invoke(emitter);
	}

	// Turn resolved
	public void EmitTurnResolved()
	{
        OnTurnResolved?.Invoke();
	}

	public void EmitTurnStateChanged(TurnState state)
	{
		OnTurnStateChanged?.Invoke(state);
	}

	public void EmitUnitDied(Unit unit)
	{
		OnUnitDeath?.Invoke(unit);
	}
}