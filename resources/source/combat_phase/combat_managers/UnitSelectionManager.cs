using Godot;
using Godot.Collections;
using System.Threading.Tasks;

public partial class UnitSelectionManager : Node3D
{
	private EventBus _eventBus;
	private SceneManager _sceneManager;
	private TurnManager _turnManager;
	private InputHandler _inputHandler;

	private Camera3D _cameraObj;
	private Unit _selectedPlayerUnit;
	private Dictionary _clickedObject;

	private bool _selectionEnabled = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_turnManager = GetParent().GetNode<TurnManager>("turn_manager");
		_turnManager.OnTurnStateChanged += HandleOnTurnStateChanged;

		_eventBus = EventBus.Instance;
		_eventBus.OnMouseLeftClick += HandleMouseLeftClick;
		_eventBus.OnMouseLeftRelease += HandleMouseLeftRelease;

		_inputHandler = InputHandler.Instance;
		_cameraObj = GetViewport().GetCamera3D();

		_sceneManager = GetParent().GetNode<SceneManager>("scene_manager");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleMouseLeftClick(Dictionary rayCastResult)
	{
		if(!_selectionEnabled)
			return;

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
					_selectedPlayerUnit.SetAlternativeTargets(_sceneManager.GetEnemyUnitsAlive());
				}
			}
		}
		_selectedPlayerUnit = null;
	}

	private async Task HandleOnTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.InProgress:
				_selectionEnabled = false;
				break;
			case TurnState.PlayerTurn:
				_selectionEnabled = true;
				break;
		}

		await Task.Yield();
	}


	public override void _ExitTree()
	{
		_turnManager.OnTurnStateChanged -= HandleOnTurnStateChanged;

		_eventBus.OnMouseLeftClick -= HandleMouseLeftClick;
		_eventBus.OnMouseLeftRelease -= HandleMouseLeftRelease;
		base._ExitTree();
	}
}
