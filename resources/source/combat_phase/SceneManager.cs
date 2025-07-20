using Godot;
using Godot.Collections;
using System;
using System.Collections;
using static SceneManager;

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

		//TODO MAKE PROPER INIT
		//_playerUnits.Add(GetNode<UnitDetails>("../player_characters/character_pos1/player_delta"));
        //_playerUnits.Add(GetNode<UnitDetails>("../player_characters/character_pos2/player_delta"));
        //_playerUnits.Add(GetNode<UnitDetails>("../player_characters/character_pos3/player_delta"));
        //_playerUnits.Add(GetNode<UnitDetails>("../player_characters/character_pos4/player_delta"));
    }




    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("mouse_left"))
        {
            var rayCastResult = CombatUtils.ShootRayCast(_cameraObj);

            if (rayCastResult != null)
            {
                SelectPlayerUnit(rayCastResult);
            }
        }

        if (Input.IsActionJustReleased("mouse_left") && _selectedPlayerUnit != null)
        {
            _selectedPlayerUnit.HideTargettingUI();
            var rayCastResult = CombatUtils.ShootRayCast(_cameraObj);

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
            // TODO Refactor as part of target selection in the UNIT DETAILS
            var unitCollider = (UnitColliderBody)collider;
            var enemyUnit = unitCollider.GetParentUnitDetails();

            _selectedPlayerUnit.SetEnemeyTarget(enemyUnit);
        }
    }

    private void ProcessNextTask()
    {
        GD.Print("Tasking");

        Action a = (Action)_actionQueue.Dequeue();
        if (a != null)
        {
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
