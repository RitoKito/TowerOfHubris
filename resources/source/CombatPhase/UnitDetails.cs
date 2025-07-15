using Godot;
using System;
using System.Diagnostics;

public partial class UnitDetails : Node3D
{
	private Sprite3D _spriteHighlight = null;
	private HpDetails _hpDetails = null;

	[Export]
	private string _unitName = "placeHolder";
	public string GetUnitName () { return _unitName; }

	[Export]
	private int _maxHp = 10;
	private int _currentHp;
	[Export]
	private int _attackValue = 2;

	private UnitDetails _enemyTarget = null;
	public UnitDetails GetEnemyTarget() { return _enemyTarget; }
	public void SetEnemeyTarget(UnitDetails target) {
		_enemyTarget = target;

		var dirToEnemy = (_enemyTarget.Position - Position) * new Vector3(1, 1, 0);
		GD.Print(dirToEnemy);
	}
	public void RemoveEnemyTarget() { _enemyTarget = null; }



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spriteHighlight = GetNode<Sprite3D>("char_select_spr");

		_hpDetails = GetNode<HpDetails>("hp_label");
		_currentHp = _maxHp;

		_hpDetails.updateHp(_currentHp.ToString() + "/" + _maxHp.ToString());

		showSelectSpr(false);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void showSelectSpr(bool state)
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

	private void _on_static_body_3d_mouse_entered()
	{
		showSelectSpr(true);
	}

	private void _on_static_body_3d_mouse_exited()
	{
		showSelectSpr(false);
	}
}
