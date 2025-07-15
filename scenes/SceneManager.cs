using Godot;
using System;

public partial class SceneManager : Node3D
{
	public static SceneManager Instance { get; private set; }

	TextEdit _debugInfo;
	TextEdit _debugInfo2;

	Camera3D _cameraObj;

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
						var playerUnit = unitCollider.getParentUnitDetails();
						_selectedPlayerUnit = playerUnit;


						if(_selectedPlayerUnit.GetEnemyTarget() != null)
						{
							_selectedPlayerUnit.RemoveEnemyTarget();
						}

						_debugInfo.Text = playerUnit.GetUnitName();
					}
				}
			}

			if(mouseButton.IsReleased())
			{
                var rayCastResult = CombatUtils.ShootRayCast(_cameraObj);
				
				if (rayCastResult != null && _selectedPlayerUnit != null)
				{
					var collider = (Node)rayCastResult["collider"];
					
					if (collider.GetGroups().Contains("EnemyUnit"))
					{
						var unitCollider = (UnitColliderBody)collider;
						var enemyUnit = unitCollider.getParentUnitDetails();

						_selectedPlayerUnit.SetEnemeyTarget(enemyUnit);
					}
				}

				_selectedPlayerUnit = null;
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (isRayCollision())
		//{
		//	GD.Print(rayCast.GetCollider());
		//}

		//if (Input.IsMouseButtonPressed(MouseButton.Left) && playerUnit != null)
		//{
		//}
	}

	private RayCast3D ShootRayCast()
	{
		//mousePos = GetViewport().GetMousePosition();
		//rayCast.TargetPosition = cam.ProjectLocalRayNormal(mousePos) * 100.0f;
		//rayCast.ForceRaycastUpdate();


		//return rayCast;

		return null;
	}
}
