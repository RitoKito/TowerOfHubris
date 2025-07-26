using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatDie : Node
{
	private List<(Ability ability, float weight)> _entries = new List<(Ability ability, float weight)>();
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
		_entries.Add((ability, weight));
	}

	public void SetWeight(Ability ability, float newWeight)
	{
		for(int i = 0; i < _entries.Count; i++)
		{
			if (_entries[i].ability == ability)
			{
				_entries[i] = (ability, newWeight);
				return;
			}
		}
	}

	public void ModifyWeight(Ability ability, float delta)
	{
        for (int i = 0; i < _entries.Count; i++)
        {
            if (_entries[i].ability == ability)
            {
				float newWeight = _entries[i].weight + delta;
                return;
            }
        }
    }

	public Ability Roll()
	{
		float total = _entries.Sum(entry => entry.weight);
		float roll = _rng.RandfRange(0, total);
		float cumulative = 0;

		// TODO REFACTOR
		for(int i = 0; i < _entries.Count; i++)
		{
			cumulative += _entries[i].weight;

			if(roll <= cumulative)
			{
                switch (_entries[i].ability.AbilityTier)
				{
					case 1:
						if (_entries.Count > 1)
						{
							_entries[i + 1] = (_entries[i + 1].ability, _entries[i].weight + 0.6f);

							if(_entries.Count > 2)
							{
                                _entries[i + 2] = (_entries[i + 2].ability, _entries[i].weight + 0.3f);
                            }
                        }
                        break;
					case 2:
                        _entries[i] = (_entries[i].ability, 0.3f);
                        if (_entries.Count > 2)
                        {
                            _entries[i + 1] = (_entries[i + 1].ability, _entries[i].weight + 0.3f);
                        }
                        break;
					case 3:
                        _entries[i] = (_entries[i].ability, 0.05f);
                        break;
				}

				return _entries[i].ability;
			}
		}

		return _entries.First().ability;
	}
}
