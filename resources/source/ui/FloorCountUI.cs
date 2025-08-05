using Godot;
using System;
using System.Threading.Tasks;

public partial class FloorCountUI : Control
{
	private EventBus _eventBus = null;

	private RichTextLabel _floorCountLabel = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnNewFloor += HandleOnNewFloor;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
		_eventBus.OnRestart += HandleOnRestart;

		_floorCountLabel = GetNode<RichTextLabel>("margin_container/floor_count_label");
		_floorCountLabel.Text = "Floor: 0";

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnNewFloor(int currentFloor)
	{
		_floorCountLabel.Text = $"Floor: {currentFloor}";
		await Task.Yield();
	}

	private async Task HandleOnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.LevelTree:
				Show();
				break;
			default:
				Hide();
				break;
		}

		await Task.Yield();
	}

	private async Task HandleOnRestart()
	{
		Hide();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnNewFloor -= HandleOnNewFloor;
		_eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
		base._ExitTree();
	}
}
