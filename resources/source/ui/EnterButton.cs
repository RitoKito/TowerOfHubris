using Godot;
using System;

public partial class EnterButton : MenuButton
{
	private IEventBus _eventBus = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent
			&& mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			_eventBus.EmitEnterGame();
		}
	}
}
