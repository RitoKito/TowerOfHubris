using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatDie : Node
{
	private List<(Ability ability, float weight)> _abilitiesAndWeights = new List<(Ability ability, float weight)>();
	private RandomNumberGenerator _rng = new RandomNumberGenerator();

	public CombatDie()
	{
		_rng.Randomize();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddAbility(Ability ability, float weight)
	{
		_abilitiesAndWeights.Add((ability, weight));
	}

	public void SetWeight(Ability ability, float newWeight)
	{
		for(int i = 0; i < _abilitiesAndWeights.Count; i++)
		{
			if (_abilitiesAndWeights[i].ability == ability)
			{
				_abilitiesAndWeights[i] = (ability, newWeight);
				return;
			}
		}
	}

	public void ModifyWeight(Ability ability, float delta)
	{
		for (int i = 0; i < _abilitiesAndWeights.Count; i++)
		{
			if (_abilitiesAndWeights[i].ability == ability)
			{
				float newWeight = _abilitiesAndWeights[i].weight + delta;
				return;
			}
		}
	}

	public Ability Roll()
	{
		float total = _abilitiesAndWeights.Sum(entry => entry.weight);
		float roll = _rng.RandfRange(0, total);
		float cumulative = 0;

		// TODO REFACTOR
		for(int i = 0; i < _abilitiesAndWeights.Count; i++)
		{
			cumulative += _abilitiesAndWeights[i].weight;

			if(roll <= cumulative)
			{
				Ability ability = null;
				switch (_abilitiesAndWeights[i].ability.AbilityTier)
				{
					case 1:
						// Increase probability of other abilities
						if (_abilitiesAndWeights.Count > 1)
						{
							ability = _abilitiesAndWeights[i + 1].ability;
							_abilitiesAndWeights[i + 1] = (ability, _abilitiesAndWeights[i].weight + ability.AbilityBaseWeightIncrease);

							if(_abilitiesAndWeights.Count > 2)
							{
								ability = _abilitiesAndWeights[i + 2].ability;
								_abilitiesAndWeights[i + 2] = (ability, _abilitiesAndWeights[i].weight + ability.AbilityBaseWeightIncrease);
							}
						}
						break;
					case 2:
						ability = _abilitiesAndWeights[i].ability;
						_abilitiesAndWeights[i] = (ability, ability.AbilityWeight);
						if (_abilitiesAndWeights.Count > 2)
						{
							ability = _abilitiesAndWeights[i + 1].ability;
							_abilitiesAndWeights[i + 1] = (ability, _abilitiesAndWeights[i + 1].weight + ability.AbilityBaseWeightIncrease);
						}
						break;
					case 3:
						ability = _abilitiesAndWeights[i].ability;
						_abilitiesAndWeights[i] = (ability, ability.AbilityWeight);
						break;
				}

				return _abilitiesAndWeights[i].ability;
			}
		}

		return _abilitiesAndWeights.First().ability;
	}
}
