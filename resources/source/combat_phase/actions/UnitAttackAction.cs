using Godot;
using System.Threading.Tasks;
using static ActionManager;

public partial class UnitAttackAction : GameAction
{
	enum State
	{
		Idle,
		MovingToEnemy,
		Attacking,
		MovingHome,
		Completed
	}

	private State _state;
	private float _moveSpeed = 5f;
	private Vector3 _offset = new Vector3(1f, 0, 0);
	private Vector3 _homePosition;
	private Unit _actionTarget = null;
	private Vector3 _enemyTargetPosition;
	private Vector3 _targetPosition;
	private Vector3 destinationThreshold = new Vector3(0.01f, 0, 0);
	private bool _completed = false;

	private ActionDelegate _actionDelegate;

	public UnitAttackAction(Unit authorUnit)
	{
		Name = $"{authorUnit.UnitName} Performing {authorUnit.CurrentAbility.AbilityName}";

        _authorUnit = authorUnit;
		_homePosition = authorUnit.GlobalPosition;

        _actionTarget = authorUnit.GetEnemyTarget();

        _offset *= _homePosition.DirectionTo(_targetPosition);
		_enemyTargetPosition = _actionTarget.GlobalPosition;
		_targetPosition = _enemyTargetPosition - _offset;
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
            _authorUnit.GlobalPosition = _authorUnit.GlobalPosition.Lerp(_targetPosition, _moveSpeed * (float)delta);

			if (_authorUnit.GlobalPosition.DistanceTo(_targetPosition) <= 0.02f)
			{
                _authorUnit.GlobalPosition = _targetPosition;
				_state = State.Attacking;
			}
		}

		// TODO ANIMATION
		if(_state == State.Attacking)
		{
			// Arbitrary wait
			// To be replaced with animations

			await Task.Delay(100);
            _authorUnit.UseAbility();
			await Task.Delay(100);
			_state = State.MovingHome;
		}

		if (_state == State.MovingHome)
		{
            _authorUnit.GlobalPosition = _authorUnit.GlobalPosition.Lerp(_homePosition, _moveSpeed * (float)delta);

			if(_authorUnit.GlobalPosition.DistanceTo(_homePosition) <= 0.02f)
			{
                _authorUnit.GlobalPosition = _homePosition;
				_state = State.Completed;
			}
		}

		if(_state == State.Completed)
		{
			// The node is queued for safe deletion
			// Until then it will remain in an idle state
			_state = State.Idle;
			_actionDelegate.Invoke();
			QueueFree();
		}
	}

	public void SelectDifferentTarget()
	{
		if(_authorUnit.FallBackEnemyTargets.Count <= 0)
		{
			_state = State.Completed;
		}

		foreach(Unit fallbackTarget in _authorUnit.FallBackEnemyTargets)
		{
			if(fallbackTarget.CurrentState != UnitState.Dead) 
			{
				_actionTarget = fallbackTarget;
				_authorUnit.SetFallbackTarget(fallbackTarget);
                _offset *= _homePosition.DirectionTo(_targetPosition);
                _enemyTargetPosition = _actionTarget.GlobalPosition;
                _targetPosition = _enemyTargetPosition - _offset;
				break;
			}
		}
	}

	public override void Execute(ActionDelegate actionDelegate)
	{
        _actionDelegate = actionDelegate;

        if (_actionTarget.CurrentState == UnitState.Dead)
        {
            SelectDifferentTarget();
        }

		_state = State.MovingToEnemy;
	}
}
