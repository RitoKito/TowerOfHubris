using Godot;
using System;
using System.Diagnostics;

public partial class UnitDetails : Node3D
{
	private Sprite3D _spriteHighlight = null;
	private HpDetails _hpLabel = null;
    private TargetArrow _targetArrow = null;
	public TargetArrow GetTargetArrow { get { return _targetArrow; } }
    private UnitDetails _enemyTarget = null;

    [Export]
	private string _unitName = "placeHolder";
	public string GetUnitName () { return _unitName; }

	[Export]
	private float _maxHp = 10;
	private float _currentHp;
	[Export]
	private float _attackValue = 2;

	private bool _drawTargetArrow = false;
	public void SetDrawTargetArrow(bool state) {  _drawTargetArrow = state; }


	public UnitDetails GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemeyTarget(UnitDetails target) {
		_enemyTarget = target;
        _drawTargetArrow = false;
        _targetArrow.ClearTargetGizmos();
        DrawTargetCurve();
	}
	public void RemoveEnemyTarget() { _enemyTarget = null; }



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetArrow = GetNode<TargetArrow>("target_arrow");
		_spriteHighlight = GetNode<Sprite3D>("char_select_spr");
        _hpLabel = GetNode<HpDetails>("hp_label");


		_currentHp = _maxHp;

        _hpLabel.updateHpLabel($"{_currentHp}/{_maxHp}");

        showSpriteHighlight(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_drawTargetArrow)
		{
            _targetArrow.DrawTargetArrow();
            GD.Print($"{Name}");
        }

		//GD.Print($"{Name}: {_drawTargetArrow}");
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

	public void UseSkill()
	{
		// NOTE: NULL ONLY IF DEAD, OTHERWISE FORCE ALL LIVING MEMBERS TO SET A TARGET
		if(_enemyTarget != null)
		{
            _enemyTarget.TakeDamage(_attackValue);
            _enemyTarget = null;
        }

		_targetArrow.ClearTargetGizmos();
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

	public void Select()
	{
		GD.Print($"Selected: {Name}");
		_drawTargetArrow = true;
	}

	public void Deselect()
	{
        _drawTargetArrow = false;
		_targetArrow.ClearTargetGizmos();
    }

	public void DrawTargetCurve() {
		_targetArrow.DrawTargetCurve(_enemyTarget.GetTargetArrow.GetTargetCurvePos);
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
