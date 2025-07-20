using Godot;
using System;
using System.Threading.Tasks;
using static SceneManager;

public partial class UnitAttackAction : Action
{
	enum State
	{
		Idle,
		MovingToEnemy,
		Attacking,
		MovingHome,
		Completed
	}

	public override string Name {  get; set; }


	private State _state;
	private Unit _unit;
	private float _moveSpeed = 5f;
	private float _offsetX = 1f;
	private Vector3 _homePosition;
	private Vector3 _enemyTargetPosition;
	private Vector3 _targetPosition;
	private bool _completed = false;

	private ActionDelegate _actionDelegate;

	public UnitAttackAction(Unit unit)
	{
		Name = $"{unit.UnitName} Attack";

        _unit = unit;
		_homePosition = unit.GlobalPosition;
		_enemyTargetPosition = unit.GetEnemyTarget().GlobalPosition;
		_targetPosition = _enemyTargetPosition - new Vector3(_offsetX, 0, 0);
		_state = State.Idle;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public async override void _Process(double delta)
	{
		if (_state == State.MovingToEnemy)
		{
			_unit.GlobalPosition = _unit.GlobalPosition.Lerp(_targetPosition, _moveSpeed * (float)delta);

			if (_unit.GlobalPosition >= _targetPosition - new Vector3(0.01f, 0, 0))
			{
				_unit.GlobalPosition = _targetPosition;
				_state = State.Attacking;
			}
		}

		// TODO ANIMATION
		if(_state == State.Attacking)
		{
            await Task.Delay(100);
            _unit.UseAbility();
            await Task.Delay(100);
            _state = State.MovingHome;
        }

        if (_state == State.MovingHome)
        {
			_unit.GlobalPosition = _unit.GlobalPosition.Lerp(_homePosition, _moveSpeed * (float)delta);

			if( _unit.GlobalPosition <= _homePosition + new Vector3(0.01f, 0, 0))
			{
				_unit.GlobalPosition = _homePosition;
				_state = State.Completed;
			}
        }

		if(_state == State.Completed)
		{
			// The node is queued for safe deletion
			// Until it is deleted it will remain in idle state
            _state = State.Idle;
			base.Execute(_actionDelegate);
            QueueFree();
        }
    }


    public override void Execute(ActionDelegate actionDelegate)
    {
		_actionDelegate = actionDelegate; 
		_state = State.MovingToEnemy;
    }
}
