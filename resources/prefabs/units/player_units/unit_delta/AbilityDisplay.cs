using Godot;
using System;

public partial class AbilityDisplay : CsgMesh3D
{
	private Label3D _label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label3D>("label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeLabel(int abilityTier)
	{
		switch(abilityTier)
		{
			case 1:
				_label.Text = "I";
				break;
            case 2:
                _label.Text = "II";
                break;
            case 3:
                _label.Text = "III";
                break;
        }
	}
}
