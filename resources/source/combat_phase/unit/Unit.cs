using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract partial class Unit : Node3D
{
	protected IMessenger _messenger;
	public IMessenger Messenger { get { return _messenger; } private set {} }

	protected Sprite3D _spriteHighlight = null;
	protected HpDetails _hpLabel = null;
	protected TargetArrow _targetArrow = null;
	protected Unit _enemyTarget = null;
	// In case primary target dies before action executed
	// it will be redirected to one of _fallbackUnits
	protected List<Unit> _alternativeTargets = new List<Unit>();
	public IReadOnlyList<Unit> FallBackEnemyTargets {  get { return _alternativeTargets.AsReadOnly(); } }

	// Tag variable set in package editor
	[Export]
	private UnitEnums.UnitTag _tag;
	public UnitEnums.UnitTag Tag {  get { return _tag; } }

	[Export]
	protected int _id = -1;

	[Export]
	protected int _unitPos = 0;
	public int UnitPos { get { return _unitPos; } }

	[Export]
	protected string _unitName = "The Mighty Placeholder";
	public string UnitName {  get { return _unitName; }}

	[Export]
	protected float _maxHp = 999;
	public float MaxHp { get { return _maxHp; } }

	protected float _currentHp;

	protected bool _isDead = false;
	public bool IsDead {  get { return _isDead; } private set { } }

	// Damage Resistance is in % form
	[Export]
	protected float _damageResistanceMultiplier = 0;

	// TODO Implement better structure for affinities
	[Export]
	protected Array<Affinity> _affinityWeaknesses = new Array<Affinity>();
	[Export]
	protected Array<Affinity> _affinityStrengths = new Array<Affinity>();

	[Export]
	protected Array<Ability> _abilities = new Array<Ability>();

	protected CombatDie _combatDie = new CombatDie();

	protected Ability _currentAbility = null;
	public Ability CurrentAbility { get { return _currentAbility; } }

	private AbilityDisplay _abilityDisplay = null;

	protected bool _drawTargetArrow = false;
	public bool DrawTargetArrow { set { _drawTargetArrow = value; } }

	public void SetMessenger(IMessenger messenger)
	{
		_messenger = messenger;
		_messenger.OnTurnStateChanged += HandleTurnStateChanged;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetArrow = GetNode<TargetArrow>("target_arrow");
		_spriteHighlight = GetNode<Sprite3D>("unit_select_spr");
		_hpLabel = GetNode<HpDetails>("hp_label");
		_abilityDisplay = GetNode<AbilityDisplay>("ability_display");

		_currentHp = _maxHp;

		_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");

		// TODO Make ability assignment into function
		// Handle invalid ability lists
		if (_abilities[0] != null)
		{
			//_currentAbility = _abilityList.GetValue(0) as Ability;
			_currentAbility = _abilities[0];
		}
		else
		{
			GD.PrintErr($"Ability List of {_unitName} May Be Empty");
		}

		ShowSpriteHighlight(false);


		// TODO REFACTOR
		for(int i = 0; i < _abilities.Count; i++)
		{
			switch (i)
			{
				case 0:
					_combatDie.AddAbility(_abilities[0], 0.8f);
					continue;
				case 1:
					_combatDie.AddAbility(_abilities[1], 0.2f);
					continue;
				case 2:
					_combatDie.AddAbility(_abilities[2], 0.05f);
					continue;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_drawTargetArrow)
		{
			_targetArrow.DrawTargetArrow();
		}
	}

	public Unit GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemyTarget(Unit target)
	{
		_enemyTarget = target;
		_drawTargetArrow = false;
		_targetArrow.HideTargetingUI();

		if (_enemyTarget.GetEnemyTarget() == this)
		{
			// Bool param sets the curve to half
			DrawTargetingCurve(true);
			_enemyTarget.DrawTargetingCurve(true);
		}
		else
		{
			DrawTargetingCurve();
		}

		_messenger.EmitTargetSelected(this);
	}

	protected void HandleTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.PlayerTurn:
                //_combatDie.ShowOjbect();
                _currentAbility = _combatDie.Roll();
				_abilityDisplay.ChangeLabel(_currentAbility.AbilityTier);
                _abilityDisplay.Show();
                break;
			case TurnState.InProgress:
				_abilityDisplay.Hide();
				HideTargetingUI();
				break;
		}
	}

	// When setting fallback target the UI is not needed
	public void SelectAlternativeTarget()
	{
		/*Random rnd = new Random();

		int targetIndex = rnd.Next(0, _alternativeTargets.Count);
		Unit alternativeTarget = _alternativeTargets[targetIndex];

		if (alternativeTarget.IsDead)
		{
			_alternativeTargets.Remove(alternativeTarget);
			SelectAlternativeTarget(del);
			return;
		}

		_enemyTarget = alternativeTarget;*/


		foreach (Unit alternativeTarget in _alternativeTargets)
		{
			if (!alternativeTarget.IsDead)
			{
				_enemyTarget = alternativeTarget;
				return;
			}
		}

		_enemyTarget = null;
	}

	// TODO Encapsulate the method 
	public void SetAlternativeTargets(IReadOnlyList<Unit> aliveEnemyTargets)
	{
		_alternativeTargets.AddRange(aliveEnemyTargets.Where(t => t != _enemyTarget));
	}

	public void RemoveEnemyTarget()
	{
		if (_enemyTarget.GetEnemyTarget() == this)
		{
			_enemyTarget.DrawTargetingCurve(false);
		}

		_enemyTarget = null;
		_messenger.EmitTargetDeselected(this);
	}

	public void ShowSpriteHighlight(bool state)
	{
		if (state)
		{
			_spriteHighlight.Visible = true;
		}
		else
		{
			_spriteHighlight.Visible = false;
		}
	}

	public void UseAbility()
	{
		if(_enemyTarget != null)
		{
			_enemyTarget.TakeDamage(CurrentAbility.CalculateDamageInstance());
			GD.Print($"{_unitName} performed {_currentAbility.AbilityName}");
			_enemyTarget = null;
		}

		_targetArrow.HideTargetingUI();
	}

	public void TakeDamage((float damageValue, Array<Affinity> affinties) damageInstance)
	{
		float weaknessMultiplier = 1;
		float streghtMultiplier = 1;
		int numberOfWeakAffintiies = _affinityWeaknesses.Intersect(damageInstance.affinties).Count();
		int numberOfStrongAffinties = _affinityStrengths.Intersect(damageInstance.affinties).Count();


		if (numberOfWeakAffintiies > 0)
		{
			weaknessMultiplier += numberOfWeakAffintiies / 10f;
		}

		if(numberOfStrongAffinties > 0)
		{
			streghtMultiplier = 1 - (numberOfStrongAffinties / 10f);
		}



		_damageResistanceMultiplier = (100 - _damageResistanceMultiplier) / 100;
		float totalDamage = damageInstance.damageValue * weaknessMultiplier * streghtMultiplier * _damageResistanceMultiplier;
		_currentHp -= totalDamage;

		if (_currentHp <= 0)
		{
			Die();
		}

		// TODO simplify
		_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");
	}

	protected void Die()
	{
		_isDead = true;

		// TODO Make Dead Sprite
		Hide();
		_messenger.EmitUnitDied(this);
	}


	// TODO Split UI
	public void DrawTargetingUI()
	{
		_drawTargetArrow = true;
	}

	public void HideTargetingUI()
	{
		_drawTargetArrow = false;
		_targetArrow.HideTargetingUI();
	}

	public void DrawTargetingCurve(bool drawHalf = false) 
	{
		_targetArrow.DrawTargetCurve(_enemyTarget.GetTargetCurvePos(), drawHalf);
	}

	public Vector3 GetTargetCurvePos()
	{
		return _targetArrow.TargetCurvePos;
	}

	//TODO Implement C# signals
	protected void _on_static_body_3d_mouse_entered()
	{
		ShowSpriteHighlight(true);
	}

	protected void _on_static_body_3d_mouse_exited()
	{
		ShowSpriteHighlight(false);
	}

	public override void _ExitTree()
	{
		_messenger.OnTurnStateChanged -= HandleTurnStateChanged;
	}
}
