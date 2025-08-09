using Godot;
using System;
using System.Threading.Tasks;

public partial class LoadingScreen : ColorRect
{
	private EventBus _eventBus = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnGameStateChanged(GameState state)
	{
		switch(state)
		{
			case GameState.CombatInit:
				Show();
				break;
			default:
				Hide();
				break;
		}

		await Task.Yield();
	}
}
