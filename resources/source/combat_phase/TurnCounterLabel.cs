using Godot;
using System;

public partial class TurnCounterLabel : Label
{
	private Messenger _messenger;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_messenger.OnTurnStateChanged += HandleTurnStateChanged;

		UpdateTurnCount(TurnManager.Instance.TurnCount);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleTurnStateChanged(TurnState state)
	{
		if (state == TurnState.PlayerTurn)
			UpdateTurnCount(TurnManager.Instance.TurnCount);
	}

	public void UpdateTurnCount(int turnCount)
	{
		Text = $"Turn: {turnCount}";
	}
}
