using Godot;
using System;

public partial class CombatUIManager : Control
{
    private Messenger _messenger;

    private Label _turnCounterLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _messenger = Messenger.Instance;
        _messenger.OnCombatSceneLoaded += HandleCombatSceneLoaded;
        _messenger.OnNewTurn += HandleNewTurn;
        _messenger.OnExitCombat += HandleExitCombat;

        _turnCounterLabel = GetNode<Label>("turn_counter/HBoxContainer/turn_counter_label");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void HandleNewTurn(int turnCount)
    {
        _turnCounterLabel.Text = $"Turn: {turnCount}";
    }

    private void HandleCombatSceneLoaded()
    {
        Show();
    }

    private void HandleExitCombat(CombatOutcome outcome)
    {
        Hide();
    }

    public override void _ExitTree()
    {
        _messenger.OnCombatSceneLoaded -= HandleCombatSceneLoaded;
        _messenger.OnNewTurn -= HandleNewTurn;
        _messenger.OnExitCombat -= HandleExitCombat;
    }
}
