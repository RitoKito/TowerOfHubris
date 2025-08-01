using Godot;
using System;
using System.Threading.Tasks;

public partial class StatusEffectUIContainer : MarginContainer
{
	private StatusEffect _statusEffect = null;
	public StatusEffect StatusEffect => _statusEffect;
	private Sprite2D _effectIcon = null;
	private RichTextLabel _stackCountLabel = null;

	//in pixels
	private Vector2 _targetTextureSize = new Vector2(30f, 30f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_effectIcon = GetNode<Sprite2D>("effect_icon");
		_stackCountLabel = GetNode<RichTextLabel>("stack_count_container/stack_count_label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateStackCountLabel(int stacks)
	{
		if (stacks <= 1)
			return;

		_stackCountLabel.Text = $"[right]{stacks}[right]";
	}

	public void SetStatusEffect(StatusEffect statusEffect)
	{
		_statusEffect = statusEffect;

		if (statusEffect.IconTexture != null)
		{
			_effectIcon.Texture = statusEffect.IconTexture;
			var _importedTextureSize = _effectIcon.Texture.GetSize();
			_effectIcon.Scale = _targetTextureSize/_importedTextureSize;
		}

		UpdateStackCountLabel(statusEffect.StackCount);
	}
}
