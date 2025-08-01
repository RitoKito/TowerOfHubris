using Godot;
using System;

public partial class RewardCard : Control
{
	private EventBus _eventBus = null;

	private Color _defaultColor;
	private const string YELLOW = "f9e06b";
	private Color _yellowColour = new Color(YELLOW);

	private Panel _rewardCardBackground = null;
	private StyleBoxFlat _rewardCardStyleBox = null;
	private Sprite2D _rewardIcon = null;
	private RichTextLabel _rewardName = null;
	private RichTextLabel _rewardDescription = null;

	private StatusEffect _reward = null;

	private Vector2 _targetTextureSize = new Vector2(60f, 60f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;

		_rewardCardBackground = GetNode<Panel>("card_background");
		_rewardIcon = GetNode<Sprite2D>("card_background/vbox_container/icon");
		_rewardName = GetNode<RichTextLabel>("card_background/vbox_container/name_margin_container/name_label");
		_rewardDescription = GetNode<RichTextLabel>("card_background/vbox_container/description_margin_container/description_label");

		_rewardCardStyleBox = _rewardCardBackground.GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
		_rewardCardBackground.AddThemeStyleboxOverride("panel", _rewardCardStyleBox);
		_defaultColor = _rewardCardStyleBox.BorderColor;

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
		_rewardCardStyleBox.BorderColor = _yellowColour;
	}

	private void _on_mouse_exited()
	{
		_rewardCardStyleBox.BorderColor = _defaultColor;
	}

	public void SetReward(StatusEffect reward)
	{
		_reward = reward;

		if (reward.IconTexture != null)
		{
			_rewardIcon.Texture = reward.IconTexture;
			var _importedTextureSize = _rewardIcon.Texture.GetSize();
			_rewardIcon.Scale = _targetTextureSize / _importedTextureSize;
		}

		_rewardName.Text = $"[center]{reward.EffectName}[center]";
		_rewardDescription.Text = $"[center]{reward.Description}[center]";
	}

	public async override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent 
			&& mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			GD.Print("Selected Reward");
			await _eventBus.EmitRewardSelected(_reward);
		}
	}
}
