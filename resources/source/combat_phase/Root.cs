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


	// TODO Make separate function for unit instantiation
	public void InstantiatePlayerCharacters()
	{
		PackedScene playerContainer = GD.Load<PackedScene>(PathConstants._playerContainerPath);
		Node playerContainerInstance = playerContainer.Instantiate();
		AddChild(playerContainerInstance);


		PackedScene unitPlaceholder = GD.Load<PackedScene>(PathConstants._unitPlaceholderPath);
		Node deltaInstance = unitPlaceholder.Instantiate();


		string pos = playerUnitDataXML.SelectSingleNode("units/delta/field_pos").InnerText;
		string unitPos = $"unit_pos_{pos}";
		playerContainerInstance.GetNode(unitPos).AddChild(deltaInstance);

		Unit delta = (Unit)deltaInstance;
		delta.Id = playerUnitDataXML.SelectSingleNode("units/delta/id").InnerText.ToInt();
		delta.UnitName = playerUnitDataXML.SelectSingleNode("units/delta/name").InnerText;

		Texture2D idle_texture = (Texture2D) GD.Load(playerUnitDataXML.SelectSingleNode("units/delta/sprites/idle").InnerText);
		Texture2D idle_highlight_texture = (Texture2D) GD.Load(playerUnitDataXML.SelectSingleNode("units/delta/sprites/idle_highlight").InnerText);

		delta.UnitTextures.Add("idle", idle_texture);
		delta.UnitTextures.Add("idle_highlight", idle_highlight_texture);

		//deltaScript.GetNode<Sprite3D>("unit_spr").Texture = idle_texture;


		delta.MaxHp = playerUnitDataXML.SelectSingleNode("units/delta/stats/max_hp").InnerText.ToInt();

		Ability[] abilityList = ImportAbilities("delta");
		delta.AbilityList = abilityList;

		delta._Ready();

		SceneManager.Instance.AppendPlayerUnit(delta);
	}

	private Ability[] ImportAbilities(string unit)
	{
		Ability[] abilityList = { null, null, null };

		for(int i = 0; i < 3; i++)
		{
			try
			{
				XmlNode abilityData = playerUnitDataXML.SelectSingleNode($"units/{unit}/abilities/ability_{i}");
				Ability ability = new Ability.AbilityBuilder()
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
