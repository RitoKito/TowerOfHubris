using Godot;
using System;

public partial class UnitUIController : Node3D
{
	private HpDetails _hpLabel = null;

	private TargetingUI _targetingUI = null;
	private Unit _targetCurveTarget = null;
	public Unit SetTargetCurveTarget { set { _targetCurveTarget = value; } }
	private AbilityDisplay _abilityDisplay = null;
	private UnitDetails _unitDetails = null;
	private AbilityDetails _abilityDetails = null;
	private float _unitDetailsZOffset = 4f;
	
	private bool _drawTargetArrow = false;
	public bool DrawTargetArrow { get { return _drawTargetArrow; } set { _drawTargetArrow = value; } }

	private bool _suppressUI = true;
	public bool SuppressUI { get { return _suppressUI; } set { _suppressUI = value; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetingUI = GetNode<TargetingUI>("targeting_ui");
		_hpLabel = GetNode<HpDetails>("hp_label");
		_unitDetails = GetNode<UnitDetails>("unit_details_3d");
		_abilityDisplay = GetNode<AbilityDisplay>("ability_display");
		_abilityDetails = GetNode<AbilityDetails>("ability_details_3d");

		HideUnitDetails();
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

	public void UpdateUnitDetailsNameLabel(string name)
	{
		_unitDetails.UpdateNameLabel(name);
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
		_unitDetails.UpdateHpLabel(currentHp, maxHp);
	}

	public void UpdateDamageResLabel(float damageRes)
	{
		_unitDetails.UpdateDamageResLabel(damageRes);
	}

	public void UpdateAbilityDetails(Ability currentAbility)
	{
		_abilityDetails.UpdateNameLabel(currentAbility.AbilityName);
		//Placeholder text
		_abilityDetails.UpdateDescriptionLabel($"Deals {Math.Round(currentAbility.AbilityDamage, 1)} damage to the target");
		_abilityDetails.UpdateCritLabel($"CR: {Math.Round(currentAbility.CritChance, 1)}% | CD: {Math.Round(currentAbility.AbilityCritMult*100, 1)}%");
		//Placeholder
		//_abilityDetails.UpdateStatusEffectLabel("-");
		//_abilityDetails.UpdateAffinityLabel("-");
	}

	public void ShowUnitDetails()
	{
		_unitDetails.Show();
	}

	public void HideUnitDetails()
	{
		_unitDetails.Hide();
	}

	public void HideHPDetails()
	{
		_hpLabel.Hide();
	}

	private void _on_ability_static_body_mouse_entered()
	{
		if(!_suppressUI)
			_abilityDetails.Show();
	}

	private void _on_ability_static_body_mouse_exited()
	{
		_abilityDetails.Hide();
	}
}
