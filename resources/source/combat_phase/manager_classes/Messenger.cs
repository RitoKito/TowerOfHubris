using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Messenger : Node, IMessenger
{
	public static Messenger Instance { get; private set; }

	//public Queue<(Delegate, object[])> _actionQueue = new Queue<(Delegate, object[])>();

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


	private bool _processingQueue = false;

	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();
	}

	public override void _Process(double delta)
	{
		/*if (!_processingQueue && _actionQueue.Count > 0) 
		{
			_processingQueue = true;
			ProcessNextAction();
		}*/
	}

   /* public void EmitActionAcked()
	{
		if(_actionQueue.Count <= 0)
		{
			_processingQueue = false;
			return;
		}

		ProcessNextAction();
	}

	private void ProcessNextAction()
	{
		(Delegate del, object[] args) action = _actionQueue.Dequeue();

		GD.Print("Processing Action Queue");
		action.del.DynamicInvoke(action.args);
	}*/

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

	public void EmitUnitDied(Unit emitter)
	{
		OnUnitDeath?.Invoke(emitter);
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
		//_actionQueue.Enqueue((OnTurnStateChanged, new object[] { state }));
	}

	public void EmitGameStateChanged(GameState state)
	{
		OnGameStateChanged?.Invoke(state);
	}

	public void EmitCombatSceneConcluded()
	{
		OnCombatSceneConcluded?.Invoke();
	}
}