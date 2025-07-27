using Godot;
using Godot.Collections;
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
	private float _attackCritMult = 1;

	[Export]
	private string _description = "Description";

	// animation

	[Export]
	private Array<Affinity> _affinities = new Array<Affinity>();
	public Array<Affinity> Affinities { get { return _affinities; } }

	public void AddAffinity(Affinity affinity)
	{
		if (_affinities.Contains(affinity))
		{
			GD.PrintErr($"Ability {_abilityName} already posses {affinity} affinity");
			return;
		}

		_affinities.Add(affinity);
	}

	public void RemoveAffinity(Affinity affinity)
	{
		_affinities.Remove(affinity);
	}


	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ModifyBaseDamage(float delta)
	{
		_abilityDamage += delta;
	}

	public void ModifyCritChance(float delta) 
	{
		_critChance += delta;
	}

	public void ModifyCritMult(float delta)
	{
		_abilityCritMult += delta;
	}

	public void ModifyAttackCritMult(float delta)
	{
		_attackCritMult += delta;
	}

	public (float, Array<Affinity>) CalculateDamageInstance()
	{
		Random rnd = new Random();
		//rnd func is exclusive of upper bound
		float crit = rnd.Next(1, 101);

		float finalCritMult = 1;
		if(crit >= 100 - _critChance) {
			finalCritMult = _abilityCritMult;
		}

		float damageFormula = _abilityDamage * finalCritMult * _attackCritMult;
		(float, Array<Affinity>) damageInstance = (damageFormula, _affinities);

		return damageInstance;
	}
}
