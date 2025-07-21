using Godot;
using System;

// Base class for abilities
public partial class Ability : Node
{
	private int _abilityId;
	public int AbilityId { get { return _abilityId; } set { _abilityId = value; } }

	private string _abilityName;
	public string AbilityName { get { return _abilityName; } set { _abilityName = value; } }

	private int _abilityTier;
	public int AbilityTier { get { return _abilityTier;  } set { _abilityTier = value; } }

	private float _abilityDamage;
	public float AbilityDamage { get { return _abilityDamage; } set { _abilityDamage = value; } }

	private float _critChance;
	public float CritChance { get { return _critChance; } set { _critChance = value; } }
	
	private float _abilityCritMult;
	public float AbilityCritMult { get { return _abilityCritMult; } set { _abilityCritMult = value; } }

	// Crit mult applied to final instance of damage
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

	public void DealDamage(Unit target)
	{
		Random rnd = new Random();
		var crit = rnd.Next(1, 100);

		if(crit <= _critChance) {
			_attackCritMult += _abilityCritMult;
		}

		var damageFormula = _abilityDamage * _attackCritMult;

		target.TakeDamage(damageFormula);
	}
}
