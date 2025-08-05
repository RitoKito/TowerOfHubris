using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenuUI : MarginContainer
{
	private EventBus _eventBus = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnEnterGame += HandleOnEnterGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnEnterGame()
	{
		Hide();

		await Task.Yield();
	}
}
