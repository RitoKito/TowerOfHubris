using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract partial class Unit : Node3D
{
	protected IEventBus _eventBus;
	public IEventBus Messenger { get { return _eventBus; } private set {} }

	protected TurnManager _turnManager;

	protected Sprite3D _spriteHighlight = null;
	protected UnitUIController _uiController = null;
	protected StatusEffectController _statusEffectController = null;


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

	
	protected Unit _enemyTarget = null;
	// In case primary target dies before action executed
	// it will be redirected to one of _alternativeTagets
	protected List<Unit> _alternativeTargets = new List<Unit>();
	public IReadOnlyList<Unit> AlternativeTargets { get { return _alternativeTargets.AsReadOnly(); } }

	public virtual void Init(TurnManager turnManager, IEventBus messenger)
	{
		_eventBus = messenger;
		_turnManager = turnManager;
		_turnManager.OnTurnStateChanged += HandleTurnStateChanged;
		_turnManager.OnNewTurn += HandleNewTurn;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spriteHighlight = GetNode<Sprite3D>("unit_select_spr");
		_uiController = GetNode<UnitUIController>("unit_ui_controller");
		_statusEffectController = GetNode<StatusEffectController>("status_effect_controller");

		_currentHp = _maxHp;
		_uiController.UpdateHpLabel((int)_currentHp, (int)_maxHp);
		_uiController.UpdateUnitDetailsNameLabel(_unitName);

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

	public void ModifyMaxHP(float delta)
	{
		_maxHp += delta;
		_currentHp = _maxHp;
		_uiController.UpdateHpLabel((int)_currentHp, (int)_maxHp);
	}

	public Unit GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemyTarget(Unit target)
	{
		_enemyTarget = target;
		_uiController.SetTargetCurveTarget = _enemyTarget;
		_uiController.HideTargetingUI();

		if (_enemyTarget.GetEnemyTarget() == this)
		{
			DrawTargetingCurve(drawHalf: true);
			_enemyTarget.DrawTargetingCurve(drawHalf: true);
		}
		else
		{
			DrawTargetingCurve();
		}

		_eventBus.EmitTargetSelected(this);
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
		_uiController.SetTargetCurveTarget = null;
		_eventBus.EmitTargetDeselected(this);
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

		_uiController.HideTargetingUI();
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

		_uiController.UpdateHpLabel((int)_currentHp, (int)_maxHp);
	}

	protected void Die()
	{
		_isDead = true;

		// TODO Make Dead Sprite
		Hide();
		_eventBus.EmitUnitDied(this);
	}


	public void DrawTargetingUI()
	{
		_uiController.DrawTargetArrow = true;
	}

	public void HideTargetingUI()
	{
		_uiController.DrawTargetArrow = false;
		_uiController.HideTargetingUI();
	}

	public void DrawTargetingCurve(bool drawHalf = false) 
	{
		_uiController.DrawTargetingCurve(drawHalf);
	}

	public Vector3 GetTargetCurvePos()
	{
		return _uiController.GetTargetCurvePos();
	}

	protected async Task HandleNewTurn(int turnCount)
	{
		//if (!IsInstanceValid(this))
		//	return;

		_currentAbility = _combatDie.Roll();
		_uiController.UpdateAbilityDisplay(_currentAbility.AbilityTier);
		_uiController.UpdateAbilityDetails(_currentAbility);

		await Task.Yield();
	}

	protected async Task HandleTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.InProgress:
				_uiController.HideAbilityDisplay();
				_uiController.HideTargetingUI();
				_uiController.SuppressUI = true;
				break;
			case TurnState.PlayerTurn:
                _uiController.SuppressUI = false;
                break;
		}

		await Task.Yield();
	}

	//TODO Implement C# signals
	protected void _on_static_body_3d_mouse_entered()
	{
		if (_uiController.SuppressUI)
			return;

			ShowSpriteHighlight(true);
			_uiController.ShowUnitDetails();
	}

	protected void _on_static_body_3d_mouse_exited()
	{
		ShowSpriteHighlight(false);
		_uiController.HideUnitDetails();
	}

	public override void _ExitTree()
	{
		_turnManager.OnTurnStateChanged -= HandleTurnStateChanged;
		_turnManager.OnNewTurn -= HandleNewTurn;

		base._ExitTree();
	}
}
