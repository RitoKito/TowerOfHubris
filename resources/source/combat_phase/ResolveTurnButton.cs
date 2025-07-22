using Godot;

public partial class ResolveTurnButton : Button
{
	private Messenger _messenger;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;

		_messenger.OnTurnStateChanged += HandleTurnStateChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Pressed()
	{
		Messenger.Instance.EmitResolveTurn();
	}

	private void HandleTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.PlayerTurn:
				Disabled = false;
				break;
			case TurnState.InProgress:
				Disabled = true;
				break;
		}

	}
}
