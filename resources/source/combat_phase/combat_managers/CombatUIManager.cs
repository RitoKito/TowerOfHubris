using Godot;
using System;
using System.Threading.Tasks;

public partial class CombatUIManager : Control
{
	private EventBus _eventBus = null;

	private Label _turnCounterLabel = null;
	private MarginContainer _resolveButton = null;
	private AnimationPlayer _tBarsAnimationPlayer = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnNewTurn += HandleOnNewTurn;
		_eventBus.OnTurnInProgress += HandleOnTurnInProgress;
		_eventBus.OnGameStateChanged += HandleOnGameStateChanged;
		//_eventBus.OnRewardSelection += HandleOnRewardSelection;
		_eventBus.OnDefeat += HandleOnDefeat;

		_turnCounterLabel = GetNode<Label>("turn_counter/turn_counter_label");
		_resolveButton = GetNode<MarginContainer>("resolve_round_btn");
		_tBarsAnimationPlayer = GetNode<AnimationPlayer>("theater_bars/tbars_animation_player");

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleOnNewTurn(int turnCount)
	{
		_turnCounterLabel.Text = $"Turn: {turnCount}";

		_turnCounterLabel.Show();
		_resolveButton.Show();
		_tBarsAnimationPlayer.Play("theater_bars_hide");
	}

	private void HandleOnTurnInProgress()
	{
		_turnCounterLabel.Hide();
		_resolveButton.Hide();
		_tBarsAnimationPlayer.Play("theater_bars_show");
	}

	private void HandleExitCombat(CombatOutcome outcome)
	{
		Hide();
	}

	private async Task HandleOnDefeat()
	{
		Hide();
		await Task.Yield();
	}

	private async Task HandleOnGameStateChanged(GameState state)
	{
		switch(state)
		{
			case GameState.Combat:
				Show();
				break;
			case GameState.CombatRewardSelection:
				Hide();
				break;
		}

		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnNewTurn -= HandleOnNewTurn;
		_eventBus.OnTurnInProgress -= HandleOnTurnInProgress;
		_eventBus.OnDefeat -= HandleOnDefeat;
        _eventBus.OnGameStateChanged -= HandleOnGameStateChanged;
        base._ExitTree();
	}
}
