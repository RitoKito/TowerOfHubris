using Godot;
using System;
using System.Collections;
using System.Diagnostics;

public partial class Unit : Node3D
{
	private Sprite3D _spriteHighlight = null;
	private HpDetails _hpLabel = null;
    private TargetArrow _targetArrow = null;
	public TargetArrow TargetArrow { get { return _targetArrow; } }
    private Unit _enemyTarget = null;

	private int _id = 0;
	public int Id { get { return _id; } set { _id = value; } }


	private string _unitName = "placeHolder";
	public string UnitName {  get { return _unitName; } set { _unitName = Name; } }


	private float _maxHp = 999;
	public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }

	private float _currentHp;

	private Ability[] _abilityList = new Ability[3];
	public Ability[] AbilityList { get { return _abilityList; } set { _abilityList = value; } }

	private Ability _currentAbility = null;
	public Ability CurrentAbility { get { return _currentAbility; } }

	//private float _attackValue = 2;

	private bool _drawTargetArrow = false;
	public bool DrawTargetArrow { set { _drawTargetArrow = value; } }

	public Unit GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemeyTarget(Unit target) {
		_enemyTarget = target;
        _drawTargetArrow = false;
        _targetArrow.HideTargettingUI();
        DrawTargettingCurve();
	}
	public void RemoveEnemyTarget() { _enemyTarget = null; }

	public class UnitBuilder
	{
		Unit _unit = new Unit();

		public UnitBuilder SetId(int id) {
			_unit.Id = id;
			return this;
		}

		public UnitBuilder SetName(string name)
		{
			_unit.Name = name;
			return this;
		}

		public UnitBuilder SetMaxHP(float maxHp)
		{
			_unit.MaxHp = maxHp;
			return this;
		}

		public UnitBuilder SetAbilityList(Ability[] abilityList)
		{
			_unit.AbilityList = abilityList;
			return this;
		}

		public Unit Build()
		{
			return _unit;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetArrow = GetNode<TargetArrow>("target_arrow");
		_spriteHighlight = GetNode<Sprite3D>("unit_select_spr");
        _hpLabel = GetNode<HpDetails>("hp_label");


		_currentHp = _maxHp;

        _hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");

        showSpriteHighlight(false);

		_currentAbility = _abilityList[2];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_drawTargetArrow)
		{
            _targetArrow.DrawTargetArrow();
        }
    }

	public void showSpriteHighlight(bool state)
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
            _enemyTarget.TakeDamage(CurrentAbility.AbilityDamage);
			GD.Print(_currentAbility.AbilityDamage);
            _enemyTarget = null;
        }

		_targetArrow.HideTargettingUI();
	}

	public void TakeDamage(float damageValue)
	{
		_currentHp -= damageValue;

		if(_currentHp <= 0)
		{
			Hide();
		}

		// TODO SIMPLIFY
		_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");
	}

	public void DrawTagettingUI()
	{
		_drawTargetArrow = true;
	}

	public void HideTargettingUI()
	{
        _drawTargetArrow = false;
		_targetArrow.HideTargettingUI();
    }

	public void DrawTargettingCurve() {
		_targetArrow.DrawTargetCurve(_enemyTarget.TargetArrow.TargetCurvePos);
	}

	private void _on_static_body_3d_mouse_entered()
	{
        showSpriteHighlight(true);
	}

	private void _on_static_body_3d_mouse_exited()
	{
        showSpriteHighlight(false);
	}
}
