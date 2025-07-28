using Godot;

public partial class ResolveTurnButton : Button
{
	//TODO EVENT BUS FOR UI
	private EventBus _eventBus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnTurnInProgress += HandleOnTurnOnProgress;
		_eventBus.OnNewTurn += HandleOnTurnResolved;

		Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Pressed()
	{
		EventBus.Instance.EmitExecuteTurn();
	}

	private void HandleOnTurnOnProgress()
	{
		Disabled = true;
	}


	private void HandleOnTurnResolved(int turnCount) 
	{
		Disabled = false;
	}

	public override void _ExitTree()
	{
		_eventBus.OnTurnInProgress -= HandleOnTurnOnProgress;
		_eventBus.OnNewTurn -= HandleOnTurnResolved;
		base._ExitTree();
	}
}
