using Godot;
using System;

public partial class UnitUIController : Node3D
{
	private HpDetails _hpLabel = null;

	private TargetingUI _targetingUI = null;
	private Unit _targetCurveTarget = null;
	public Unit SetTargetCurveTarget { set { _targetCurveTarget = value; } }
	private AbilityDisplay _abilityDisplay = null;
	
	private bool _drawTargetArrow = false;
	public bool DrawTargetArrow { get { return _drawTargetArrow; } set { _drawTargetArrow = value; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetingUI = GetNode<TargetingUI>("targeting_ui");
		_hpLabel = GetNode<HpDetails>("hp_label");
		_abilityDisplay = GetNode<AbilityDisplay>("ability_display");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_drawTargetArrow)
		{
			_targetingUI.DrawTargetArrow();
		}
	}

	public void DrawTargetingUI()
	{
		_drawTargetArrow = true;
	}

	public void HideTargetingUI()
	{
		_drawTargetArrow = false;
		_targetingUI.HideTargetingUI();
	}

	public void DrawTargetingCurve(bool drawHalf = false)
	{
		_targetingUI.DrawTargetCurve(_targetCurveTarget.GetTargetCurvePos(), drawHalf);
	}

	public Vector3 GetTargetCurvePos()
	{
		return _targetingUI.TargetCurvePos;
	}

	public void UpdateAbilityDisplay(int abilityTier)
	{
		_abilityDisplay.ChangeLabel(abilityTier);
		_abilityDisplay.Show();
	}

	public void HideAbilityDisplay()
	{
		_abilityDisplay.Hide();
	}

	public void UpdateHpLabel(int currentHp, int maxHp)
	{
		_hpLabel.UpdateHpLabel(currentHp, maxHp);
	}
}
