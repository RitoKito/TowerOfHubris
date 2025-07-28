using Godot;
using System;

public partial class AbilityDetails : Sprite3D
{
    private RichTextLabel _nameLabel;
	private RichTextLabel _descriptionLabel;
	private RichTextLabel _critLabel;
	private RichTextLabel _statusEffectLabel;
	private RichTextLabel _affinityLabel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/ability_name_label");
        _descriptionLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/ability_description_label");
        _critLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/ability_crit_label");
        _statusEffectLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/ability_status_effects_label");
        _affinityLabel = GetNode<RichTextLabel>("SubViewport/unit_details/border_container/border/fill_box_container/fill_box/item_container/VBoxContainer/ability_affinity_label");

        Hide();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateNameLabel(string name)
    {
        _nameLabel.Text = name;
    }

    public void UpdateDescriptionLabel(string description)
    {
        _descriptionLabel.Text = description;
    }

    public void UpdateCritLabel(string crit)
    {
        _critLabel.Text = crit;
    }

    public void UpdateStatusEffectLabel(string statusEffect)
    {
        _statusEffectLabel.Text = statusEffect;
    }

    public void UpdateAffinityLabel(string affinity)
    {
        _affinityLabel.Text = affinity;
    }
}
