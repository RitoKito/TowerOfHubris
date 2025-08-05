using Godot;
using System;
using System.Threading.Tasks;

public partial class DefeatUI : Control
{
	private EventBus _eventBus = null;
	private AnimationPlayer _animationPlayer = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnDefeat += HandleOnDefeat;

		_animationPlayer = GetNode<AnimationPlayer>("animation_player");

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnDefeat()
	{
		Show();

		_animationPlayer.Play("defeat_ui_appear");
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnDefeat -= HandleOnDefeat;
		base._ExitTree();
	}
}
