using Godot;
using System;
using System.Threading.Tasks;

public partial class ESCMenu : MarginContainer
{
	private EventBus _eventBus = null;

	private bool _isDisplayed = false;
	private bool _isEnabled = false;

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("open_menu"))
		{
			if (!_isEnabled)
				return;

			if (_isDisplayed)
			{
				Hide();
				_isDisplayed = false;
			}
			else
			{
				Show();
				_isDisplayed = true;
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
		_eventBus.OnDefeat += HandleOnDefeat;
		_eventBus.OnRestart += HandleOnRestart;

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task HandleOnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.MainMenu:
				_isEnabled = false;
				break;
			case GameState.Defeat:
				_isEnabled = false;
				break;
			default:
				_isEnabled = true;
				break;
		}

		await Task.Yield();
	}

	private async Task HandleOnDefeat()
	{
		Hide();
		await Task.Yield();
	}

	private async Task HandleOnRestart()
	{
		Hide();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
		_eventBus.OnRestart -= HandleOnRestart;
		base._ExitTree();
	}
}
