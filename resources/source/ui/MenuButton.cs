using Godot;
using System;

public abstract partial class MenuButton : RichTextLabel
{
	private Color _defaultColour = new Color(1f, 1f, 1f);
	private Color _yellowColour = new Color(1.0f, 1.0f, 0f);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_defaultColour = GetThemeColor("default_color");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_mouse_entered()
	{
		AddThemeColorOverride("default_color", _yellowColour);
	}

	private void _on_mouse_exited()
	{
		AddThemeColorOverride("default_color", _defaultColour);
	}
}
