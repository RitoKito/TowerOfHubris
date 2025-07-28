using Godot;
using System;
using System.Threading.Tasks;

public partial class CombatUIManager : Control
{
	private EventBus _eventBus;

	private Label _turnCounterLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnNewTurn += HandleOnNewTurn;
		_eventBus.OnCombatSceneLoaded += HandleCombatSceneLoaded;

		_turnCounterLabel = GetNode<Label>("turn_counter/HBoxContainer/turn_counter_label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleOnNewTurn(int turnCount)
	{
		_turnCounterLabel.Text = $"Turn: {turnCount}";
	}

	private async Task HandleCombatSceneLoaded()
	{
		Show();
		await Task.Yield();
	}

	private void HandleExitCombat(CombatOutcome outcome)
	{
		Hide();
	}

	public override void _ExitTree()
	{
		_eventBus.OnNewTurn -= HandleOnNewTurn;
		_eventBus.OnCombatSceneLoaded -= HandleCombatSceneLoaded;
		base._ExitTree();
	}
}
