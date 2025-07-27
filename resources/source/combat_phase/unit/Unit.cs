using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract partial class Unit : Node3D
{
	protected IMessenger _messenger;
	public IMessenger Messenger { get { return _messenger; } private set {} }
    private Sprite3D _spriteHighlight = null;
    protected UnitUIController _unitUIManager = null;
	protected Unit _enemyTarget = null;

	// In case primary target dies before action executed
	// it will be redirected to one of _alternativeTagets
	protected List<Unit> _alternativeTargets = new List<Unit>();
	public IReadOnlyList<Unit> AlternativeTargets {  get { return _alternativeTargets.AsReadOnly(); } }

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

	public void SetMessenger(IMessenger messenger)
	{
		_messenger = messenger;
		_messenger.OnTurnStateChanged += HandleTurnStateChanged;
		_messenger.OnNewTurn += HandleNewTurn;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _spriteHighlight = GetNode<Sprite3D>("unit_select_spr");
        _unitUIManager = GetNode<UnitUIController>("unit_ui_controller");

        _currentHp = _maxHp;

		//_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");
		_unitUIManager.UpdateHpLabel((int)_currentHp, (int)_maxHp);

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
	}

	public Unit GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemyTarget(Unit target)
	{
		_enemyTarget = target;
		_unitUIManager.SetTargetCurveTarget = _enemyTarget;
		_unitUIManager.HideTargetingUI();

		if (_enemyTarget.GetEnemyTarget() == this)
		{
			DrawTargetingCurve(drawHalf: true);
			_enemyTarget.DrawTargetingCurve(drawHalf: true);
		}
		else
		{
			DrawTargetingCurve();
		}

		_messenger.EmitTargetSelected(this);
	}

	protected void HandleNewTurn(int _turnCount)
	{
        _currentAbility = _combatDie.Roll();
		_unitUIManager.UpdateAbilityDisplay(_currentAbility.AbilityTier);
    }

	protected void HandleTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.InProgress:
				_unitUIManager.HideAbilityDisplay();
				_unitUIManager.HideTargetingUI();
				break;
		}
	}

	// When setting fallback target the UI is not needed
	public void SelectAlternativeTarget()
	{
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
			_enemyTarget.DrawTargetingCurve(drawHalf: false);
		}

		_enemyTarget = null;
        _unitUIManager.SetTargetCurveTarget = null;
        _messenger.EmitTargetDeselected(this);
	}

    public void ShowSpriteHighlight(bool show)
    {
        if (show)
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
			GD.Print($"{_unitName} performing {_currentAbility.AbilityName}");
			_enemyTarget = null;
		}

		_unitUIManager.HideTargetingUI();
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

		_unitUIManager.UpdateHpLabel((int)_currentHp, (int)_maxHp);
	}

	protected void Die()
	{
		_isDead = true;

		// TODO Make Dead Sprite
		Hide();
		_messenger.EmitUnitDied(this);
	}

	public void DrawTargetingUI()
	{
		_unitUIManager.DrawTargetArrow = true;
	}

	public void HideTargetingUI()
	{
        _unitUIManager.DrawTargetArrow = false;
		_unitUIManager.HideTargetingUI();
	}

	public void DrawTargetingCurve(bool drawHalf = false) 
	{
		_unitUIManager.DrawTargetingCurve(drawHalf);
	}

	public Vector3 GetTargetCurvePos()
	{
		return _unitUIManager.GetTargetCurvePos();
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
        _messenger.OnNewTurn -= HandleNewTurn;
    }
}
