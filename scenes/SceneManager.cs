using Godot;
using System;
using System.Collections;

enum PlayerTurnState
{
	Idle,
	SelectedUnit
}

public partial class SceneManager : Node3D
{
	public static SceneManager Instance { get; private set; }

	TextEdit _debugInfo;
	TextEdit _debugInfo2;

	Camera3D _cameraObj;
	PlayerTurnState _playerState = PlayerTurnState.Idle;

	private ArrayList _playerUnits = new ArrayList();
	private UnitDetails _selectedPlayerUnit;



	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				var rayCastResult = CombatUtils.ShootRayCast(_cameraObj);

				if(rayCastResult != null)
				{
					var collider = (Node)rayCastResult["collider"];
					if (collider.GetGroups().Contains("PlayerUnit"))
					{
						var unitCollider = (UnitColliderBody)collider;
						var playerUnit = unitCollider.GetParentUnitDetails();
						_selectedPlayerUnit = playerUnit;


						if(_selectedPlayerUnit.GetEnemyTarget() != null)
						{
							_selectedPlayerUnit.RemoveEnemyTarget();
						}

						_selectedPlayerUnit.Select();
						_playerState = PlayerTurnState.SelectedUnit;
						_debugInfo.Text = playerUnit.GetUnitName();
						GD.Print(_selectedPlayerUnit.GetUnitName());
					}
				}
			}

			if(mouseButton.IsReleased())
			{
				var rayCastResult = CombatUtils.ShootRayCast(_cameraObj);
				
				if (_selectedPlayerUnit != null)
				{
					if(rayCastResult != null)
					{
						var collider = (Node)rayCastResult["collider"];

						if (collider.GetGroups().Contains("EnemyUnit"))
						{
							// TODO Refactor as part of target selection in the UNIT DETAILS
							var unitCollider = (UnitColliderBody)collider;
							var enemyUnit = unitCollider.GetParentUnitDetails();

							_selectedPlayerUnit.SetEnemeyTarget(enemyUnit);
						}
					}
					else
					{
						_selectedPlayerUnit.Deselect();

					}

					_selectedPlayerUnit = null;
					_playerState = PlayerTurnState.Idle;
				}
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		_debugInfo = GetNode<TextEdit>("../CombatUI/DebugInfo/VBoxContainer/CombatSelection");
		_debugInfo2 = GetNode<TextEdit>("../CombatUI/DebugInfo/VBoxContainer/CombatSelection2");

		_cameraObj = GetViewport().GetCamera3D();

		//TODO MAKE PROPER INIT
		_playerUnits.Add(GetNode<UnitDetails>("../player_characters/character_pos1/player_delta"));
		GD.Print(_playerUnits[0]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void _on_button_pressed()
	{
		foreach(UnitDetails unit in _playerUnits)
		{
			unit.UseSkill();
		}
	}
}
