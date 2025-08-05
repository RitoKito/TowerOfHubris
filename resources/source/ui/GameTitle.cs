using Godot;
using System;
using System.Threading.Tasks;

public partial class GameTitle : MarginContainer
{
	private EventBus _eventBus = null;
	private AnimationPlayer _titlePlayer = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnEnterGame += HandleOnEnterGame;

		_titlePlayer = GetNode<AnimationPlayer>("title_animation_player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnEnterGame()
	{
		//_titlePlayer.Play("title_fade");
		Hide();
		QueueFree();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnEnterGame -= HandleOnEnterGame;
		base._ExitTree();
	}
}
