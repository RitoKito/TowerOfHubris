using Godot;
using System;

public partial class RewardContainer : MarginContainer
{
	private Messenger _messenger = null;

	private ColorRect _border;
	private Color _defaultColor;
	private const string YELLOW = "f9e06b";
	private Color _yellowColour = new Color(YELLOW);

	private RichTextLabel _rewardName = null;
	private RichTextLabel _rewardDescription = null;

	private StatusEffect _reward = null;
	public StatusEffect Reward {  get { return _reward; }  set { _reward = value; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;

		_border = GetNode<ColorRect>("border");
		_rewardName = GetNode<RichTextLabel>("border/items/VBoxContainer/name_container/name_label");
		_rewardDescription = GetNode<RichTextLabel>("border/items/VBoxContainer/description_container/description_label");

		_defaultColor = _border.Color;

		//Hardcoded reward for now
		// TODO implement random reward generation
		// when rewards implemented
		SetReward(new PowerOfPlaceholder());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_mouse_entered()
	{
		_border.Color = _yellowColour;
	}

	private void _on_mouse_exited()
	{
		_border.Color = _defaultColor;
	}

	public void SetReward(StatusEffect reward)
	{
		_reward = reward;
		_rewardName.Text = $"[center]{reward.EffectName}";
		_rewardDescription.Text = $"[center]{reward.Description}[center]";
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent 
			&& mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			GD.Print("Selected Reward");
			_messenger.EmitRewardSelected(_reward);
		}
	}
}
