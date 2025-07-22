using Godot;
using System.Collections.Generic;

public abstract partial class Unit: Node3D
{
	protected Sprite3D _spriteHighlight = null;
	protected HpDetails _hpLabel = null;
	protected TargetArrow _targetArrow = null;
	protected Unit _enemyTarget = null;

	[Export]
	protected int _id = -1;

	[Export]
	protected string _unitName = "The Mighty Placeholder";
	public string UnitName {  get { return _unitName; }}

	[Export]
	protected float _maxHp = 999;
	public float MaxHp { get { return _maxHp; } }

	protected float _currentHp;

	[Export]
	protected Ability _abilityTier1;

	protected Ability _currentAbility = null;
	public Ability CurrentAbility { get { return _currentAbility; } }

	protected Dictionary<string, Texture2D> _unitTextureList = new Dictionary<string, Texture2D>();
	public Dictionary<string, Texture2D> UnitTextures { get { return  _unitTextureList; } private set { } }

	//private float _attackValue = 2;

	protected bool _drawTargetArrow = false;
	public bool DrawTargetArrow { set { _drawTargetArrow = value; } }

	public Unit GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemyTarget(Unit target) {
		_enemyTarget = target;
		_drawTargetArrow = false;
		_targetArrow.HideTargetingUI();

		if(_enemyTarget.GetEnemyTarget() == this)
		{
			// Bool param sets the curve to half
			DrawTargetingCurve(true);
			_enemyTarget.DrawTargetingCurve(true);
		}
		else
			DrawTargetingCurve();
	}
	public void RemoveEnemyTarget() {
		if(_enemyTarget.GetEnemyTarget() == this)
		{
			_enemyTarget.DrawTargetingCurve(false);
		}

		_enemyTarget = null; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetArrow = GetNode<TargetArrow>("target_arrow");
		_spriteHighlight = GetNode<Sprite3D>("unit_select_spr");
		_hpLabel = GetNode<HpDetails>("hp_label");

		_currentHp = _maxHp;

		_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");

		// TODO Make ability assignment into function
		// Handle invalid ability lists
		if (_abilityTier1 != null)
		{
			//_currentAbility = _abilityList.GetValue(0) as Ability;
			_currentAbility = _abilityTier1;
		}

		ShowSpriteHighlight(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_drawTargetArrow)
		{
			_targetArrow.DrawTargetArrow();
		}
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
			GD.Print(_currentAbility.AbilityName);
			_enemyTarget = null;
		}

		_targetArrow.HideTargetingUI();
	}

	public void TakeDamage(float damageValue)
	{
		_currentHp -= damageValue;

		if(_currentHp <= 0)
		{
			Hide();
		}

		// TODO simplify
		_hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");
	}

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
}
