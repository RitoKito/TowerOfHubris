using Godot;
using Godot.NativeInterop;
using System;
using System.IO;
using System.Xml;

public partial class Root : Node3D
{
	public static Root Instance = new Root();

	private XmlDocument playerUnitDataXML = new XmlDocument();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		playerUnitDataXML.Load("resources\\data\\player_units\\player_units.xml");

		InstantiatePlayerCharacters();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InstantiatePlayerCharacters()
	{
		var playerContainer = GD.Load<PackedScene>(InstantionPaths._playerContainerPath);
		var playerContainerInstance = playerContainer.Instantiate();
		AddChild(playerContainerInstance);


		var unitPlaceholder = GD.Load<PackedScene>(InstantionPaths._unitPlaceholderPath);
		var deltaInstance = unitPlaceholder.Instantiate();


		var pos = playerUnitDataXML.SelectSingleNode("units/delta/field_pos").InnerText;
		var unitPos = $"unit_pos_{pos}";
		playerContainerInstance.GetNode(unitPos).AddChild(deltaInstance);

        var deltaScript = (Unit)deltaInstance;
        deltaScript.Id = playerUnitDataXML.SelectSingleNode("units/delta/id").InnerText.ToInt();
		deltaScript.Name = playerUnitDataXML.SelectSingleNode("units/delta/name").InnerText;
		var idle_texture = GD.Load(playerUnitDataXML.SelectSingleNode("units/delta/sprites/idle").InnerText);
		var idle_highlight_texture = GD.Load(playerUnitDataXML.SelectSingleNode("units/delta/sprites/idle_highlight").InnerText);
        deltaScript.GetNode<Sprite3D>("unit_spr").Texture = (Texture2D)idle_texture;
        deltaScript.MaxHp = playerUnitDataXML.SelectSingleNode("units/delta/stats/max_hp").InnerText.ToInt();

		Ability[] abilityList = ImportAbilities("delta");
		deltaScript.AbilityList = abilityList;

		deltaScript._Ready();

        SceneManager.Instance.appendPlayerUnit(deltaScript);
	}

	private Ability[] ImportAbilities(string unit)
	{
		Ability[] abilityList = { null, null, null };

		for(int i = 0; i < 3; i++)
		{
			try
			{
				var abilityData = playerUnitDataXML.SelectSingleNode($"units/{unit}/abilities/ability_{i}");
				var ability = new Ability.AbilityBuilder()
						.SetId(abilityData.SelectSingleNode("ability_id").InnerText.ToInt())
						.SetName(abilityData.SelectSingleNode("ability_name").InnerText)
						.SetTier(abilityData.SelectSingleNode("ability_tier").InnerText.ToInt())
						.SetDamage(abilityData.SelectSingleNode("ability_damage").InnerText.ToFloat())
						.SetCritChance(abilityData.SelectSingleNode("crit_chance").InnerText.ToFloat())
						.SetCritMult(abilityData.SelectSingleNode("crit_mult").InnerText.ToFloat())
						.Build();

                abilityList[i] = ability;
            }
            catch (Exception ex)
			{
				GD.Print(ex);
			}

        }

        return abilityList;
    }
}
