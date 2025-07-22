using Godot;
using System;

// Base class for abilities
public partial class Ability : Node
{
	[Export]
	private int _abilityId;
	public int AbilityId { get { return _abilityId; } set { } }

	[Export]
	private string _abilityName;
	public string AbilityName { get { return _abilityName; } set { } }

	[Export]
	private int _abilityTier;
	public int AbilityTier { get { return _abilityTier;  } set { } }

	[Export]
	private float _abilityDamage;
	public float AbilityDamage { get { return _abilityDamage; } set { } }

	[Export]
	private float _critChance;
	public float CritChance { get { return _critChance; } set { } }

	[Export]
	private float _abilityCritMult;
	public float AbilityCritMult { get { return _abilityCritMult; } set { } }

	// Crit mult applied to final instance of damage
	[Export]
	private float _attackCritMult = 1;
	// animation

	public class AbilityBuilder
	{
		private Ability _ability = new Ability();

		public AbilityBuilder SetId(int id) 
		{
			_ability.AbilityId = id;
			return this;
		}
		public AbilityBuilder SetName(string name)
		{
			_ability.AbilityName= name;
			return this;
		}
		public AbilityBuilder SetTier(int tier)
		{
			_ability.AbilityTier = tier;
			return this;
		}
		public AbilityBuilder SetDamage(float damage)
		{
			_ability.AbilityDamage = damage;
			return this;
		}
		public AbilityBuilder SetCritChance(float critChance)
		{
			_ability.CritChance = critChance;
			return this;
		}
		public AbilityBuilder SetCritMult(float critMult)
		{
			_ability.AbilityCritMult = critMult;
			return this;
		}

		public Ability Build()
		{
			return _ability;
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public float CalculateDamageInstance()
	{
		Random rnd = new Random();
		//rnd func is exclusive of upper bound
		float crit = rnd.Next(1, 101);

		float finalCritMult = 1;
		if(crit >= 100 - _critChance) {
			finalCritMult = _abilityCritMult;
		}

		float damageFormula = _abilityDamage * finalCritMult;

		return damageFormula;
	}
}
