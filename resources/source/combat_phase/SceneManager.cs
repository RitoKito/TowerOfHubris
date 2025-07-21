using Godot;
using Godot.Collections;
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
	private Unit _selectedPlayerUnit;

	private Queue _actionQueue = new Queue();

	public delegate void ActionDelegate();
	private bool _processingTask = false;

	public void appendPlayerUnit(Node unit)
	{
		_playerUnits.Add(unit);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		
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
		if (Input.IsActionJustPressed("mouse_left"))
		{
			var rayCastResult = Utils.ShootRayCast(_cameraObj);

			if (rayCastResult != null)
			{
				SelectPlayerUnit(rayCastResult);
			}
		}

		if (Input.IsActionJustReleased("mouse_left") && _selectedPlayerUnit != null)
		{
			_selectedPlayerUnit.HideTargettingUI();
			var rayCastResult = Utils.ShootRayCast(_cameraObj);

			if (_selectedPlayerUnit != null && rayCastResult != null)
			{
				SelectEnemyUnit(rayCastResult);
			}

			_selectedPlayerUnit = null;
			_playerState = PlayerTurnState.Idle;
		}
	}

	private void SelectPlayerUnit(Dictionary rayCastResult)
	{
		var collider = (Node)rayCastResult["collider"];
		if (collider.GetGroups().Contains("PlayerUnit"))
		{
			var unitCollider = (UnitColliderBody)collider;
			var playerUnit = unitCollider.GetParentUnitDetails();
			_selectedPlayerUnit = playerUnit;


			if (_selectedPlayerUnit.GetEnemyTarget() != null)
			{
				_selectedPlayerUnit.RemoveEnemyTarget();
			}

			_selectedPlayerUnit.DrawTagettingUI();
			_playerState = PlayerTurnState.SelectedUnit;
		}
	}

	private void SelectEnemyUnit(Dictionary rayCastResult)
	{
		var collider = (Node)rayCastResult["collider"];

		if (collider.GetGroups().Contains("EnemyUnit"))
		{
			var unitCollider = (UnitColliderBody)collider;
			var enemyUnit = unitCollider.GetParentUnitDetails();

			_selectedPlayerUnit.SetEnemeyTarget(enemyUnit);
		}
	}

	private void ProcessNextTask()
	{
		Action a = (Action)_actionQueue.Dequeue();
		if (a != null)
		{
			GD.Print("Tasking");
			a.Execute(() =>
			{
				if(_actionQueue.Count > 0)
				{
					ProcessNextTask();
				}
			});
		}
	}

	private void EnqueueTask(Action action)
	{
		_actionQueue.Enqueue(action);
	}

	// TODO Implement C# signals instead
	private void _on_button_pressed()
	{
		foreach(Unit unit in _playerUnits)
		{
			unit.HideTargettingUI();
			// TODO ALLOW ONLY WHEN ALL UNITS HAVE TARGET
			if(unit.GetEnemyTarget() != null)
			{
				Action unitAction = new UnitAttackAction(unit);
				AddChild((unitAction));
				EnqueueTask(unitAction);
			}
		}

		ProcessNextTask();
	}
}
