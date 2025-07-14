using Godot;
using System;

public partial class SceneManager : Node3D
{
	TextEdit debugInfo;
	TextEdit debugInfo2;

	Vector2 mousePos;
	Camera3D cam;
	RayCast3D rayCast;

	UnitDetails playerUnit;
	UnitDetails enemyUnit;


	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				var collider = ShootRayCast().GetCollider() as StaticBody3D;

				if(collider != null)
				{
					var colliderParent = collider.GetParent() as Node3D;
					if (colliderParent.GetGroups().Contains("PlayerUnit")){
						playerUnit = (UnitDetails)colliderParent;
					}
				}
			}

			if(mouseButton.Pressed && playerUnit != null)
			{
				debugInfo.Text = $"Player: {playerUnit.getUnitName()}";
				debugInfo2.Text = $"Enemy: None";
			}
			else if(mouseButton.IsReleased())
			{
				var collider = ShootRayCast().GetCollider() as StaticBody3D;

				if (collider != null)
				{
					var colliderParent = collider.GetParent() as Node3D;
					if (colliderParent.GetGroups().Contains("EnemyUnit"))
					{
						enemyUnit = (UnitDetails)colliderParent;
						debugInfo2.Text = $"Enemy: {enemyUnit.getUnitName()}";
						enemyUnit = null;
					}
					else
					{
						debugInfo.Text = $"Player: None";
						debugInfo2.Text = $"Enemy: None";
						playerUnit = null;
						enemyUnit = null;
					}
				}
				else
				{

					debugInfo.Text = $"Player: None";
					debugInfo2.Text = $"Enemy: None";
					playerUnit = null;
					enemyUnit = null;
				}
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		debugInfo = GetNode<TextEdit>("../CombatUI/DebugInfo/VBoxContainer/CombatSelection");
		debugInfo2 = GetNode<TextEdit>("../CombatUI/DebugInfo/VBoxContainer/CombatSelection2");

		cam = GetViewport().GetCamera3D();
		rayCast = cam.GetNode<RayCast3D>("RayCast3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (isRayCollision())
		//{
		//	GD.Print(rayCast.GetCollider());
		//}

		if (Input.IsMouseButtonPressed(MouseButton.Left) && playerUnit != null)
		{
		}
	}

	private RayCast3D ShootRayCast()
	{
		mousePos = GetViewport().GetMousePosition();
		rayCast.TargetPosition = cam.ProjectLocalRayNormal(mousePos) * 100.0f;
		rayCast.ForceRaycastUpdate();


		return rayCast;
	}
}
