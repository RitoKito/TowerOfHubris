using Godot;
using Godot.Collections;
using System;

public partial class Messenger : Node
{
	public static Messenger Instance { get; private set; }

	public event Action<Dictionary> OnMouseLeftClick;
	public event Action<Dictionary> OnMouseLeftRelease;
	public event Action OnResolveRound;

	public override void _Ready()
	{
        if (Instance == null)
            Instance = this;
        else
            Free();
    }

	public override void _Process(double delta)
	{
	}

	public void EmitMouseLeftClicked(Dictionary clickedObject)
	{
		OnMouseLeftClick?.Invoke(clickedObject);
	}

	public void EmitMouseLeftReleased(Dictionary clickedObject)
	{
		OnMouseLeftRelease?.Invoke(clickedObject);
	}

	public void EmitResolveRound()
	{
		OnResolveRound?.Invoke();
	}
}