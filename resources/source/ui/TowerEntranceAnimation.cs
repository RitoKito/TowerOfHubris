using Godot;
using System;
using System.Threading.Tasks;

public partial class TowerEntranceAnimation : Node3D
{
	private EventBus _eventBus;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnGameStateChanged += HandleGameStateChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.MainMenu:
				Show();
				break;
			default:
				Hide();
				break;
		}

		await Task.Yield();
	}
}
