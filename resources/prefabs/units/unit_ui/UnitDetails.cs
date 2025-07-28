using Godot;
using System;

public partial class UnitDetails : Sprite3D
{
    private RichTextLabel _nameLabel;
    private RichTextLabel _hpLabel;
	private RichTextLabel _weaknessLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/unit_name_label");
        _hpLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/unit_hp_label");
        _weaknessLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/unit_weakness_label");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateNameLabel(string name)
	{
		_nameLabel.Text = name;
	}

	public void UpdateHpLabel(int currentHP, int maxHP)
	{
		_hpLabel.Text = $"{currentHP} / {maxHP}";
	}

	public void UpdateWeaknessLabel(string weakness)
	{
		_weaknessLabel.Text = weakness;
	}
}
