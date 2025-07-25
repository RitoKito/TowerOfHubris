using Godot;
using Godot.Collections;

public partial class UnitSelectionManager : Node3D
{
	private Messenger _messenger;
	private SceneManager _sceneManager;
	private InputHandler _inputHandler;

	private Camera3D _cameraObj;
	private Unit _selectedPlayerUnit;
	private Dictionary _clickedObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_messenger.OnMouseLeftClick += HandleMouseLeftClick;
		_messenger.OnMouseLeftRelease += HandleMouseLeftRelease;

		_inputHandler = InputHandler.Instance;
		_cameraObj = GetViewport().GetCamera3D();

		_sceneManager = GetParent<Node3D>().GetNode<SceneManager>("scene_manager");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleMouseLeftClick(Dictionary rayCastResult)
	{
		Node collider = (Node)rayCastResult["collider"];

		if (collider.GetGroups().Contains("PlayerUnit"))
		{
			UnitColliderBody unitCollider = collider as UnitColliderBody;
			Unit playerUnit = unitCollider.GetParentUnitDetails();

			if (!playerUnit.IsDead)
			{
				_selectedPlayerUnit = playerUnit;

				if (_selectedPlayerUnit.GetEnemyTarget() != null)
				{
					_selectedPlayerUnit.RemoveEnemyTarget();
				}

				_selectedPlayerUnit.DrawTargetingUI();
			}
		}
	}

	private void HandleMouseLeftRelease(Dictionary rayCastResult)
	{
		if (_selectedPlayerUnit == null) 
			return;

		_selectedPlayerUnit.HideTargetingUI();

		if(rayCastResult != null)
		{
			Node collider = (Node)rayCastResult["collider"];

			if (collider.GetGroups().Contains("EnemyUnit"))
			{
				UnitColliderBody unitCollider = collider as UnitColliderBody;
				Unit enemyUnit = unitCollider.GetParentUnitDetails();

				if(!enemyUnit.IsDead) 
				{
					_selectedPlayerUnit.SetEnemyTarget(enemyUnit);
					_selectedPlayerUnit.SetAlternativeTargets(_sceneManager.GetAliveEnemyUnits());
				}
			}
		}
		_selectedPlayerUnit = null;
	}

	public override void _ExitTree()
	{
		_messenger.OnMouseLeftClick += HandleMouseLeftClick;
		_messenger.OnMouseLeftRelease += HandleMouseLeftRelease;
	}
}
