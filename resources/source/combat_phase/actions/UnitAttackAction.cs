using Godot;
using System.Threading.Tasks;

public partial class UnitAttackAction : GameAction
{
	enum State
	{
		AwaitingDeletion,
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

	public UnitAttackAction(Unit authorUnit, IMessenger messenger)
	{
		Name = $"{authorUnit.UnitName} Performing {authorUnit.CurrentAbility.AbilityName}";

		_messenger = messenger;
        _creator = authorUnit;
		_homePosition = authorUnit.GlobalPosition;

        _actionTarget = authorUnit.GetEnemyTarget();

        _offset *= _homePosition.DirectionTo(_targetPosition);
		_enemyTargetPosition = _actionTarget.GlobalPosition;
		_targetPosition = _enemyTargetPosition - _offset;
        _state = State.AwaitingDeletion;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public async override void _Process(double delta)
	{
		if (_state == State.MovingToEnemy)
		{
            _creator.GlobalPosition = _creator.GlobalPosition.Lerp(_targetPosition, _moveSpeed * (float)delta);

			if (_creator.GlobalPosition.DistanceTo(_targetPosition) <= 0.02f)
			{
                _creator.GlobalPosition = _targetPosition;
				_state = State.Attacking;
			}
		}

		// TODO ANIMATION
		if(_state == State.Attacking)
		{
			// Arbitrary wait
			// To be replaced with animations

			await Task.Delay(100);
            _creator.UseAbility();
			await Task.Delay(100);
			_state = State.MovingHome;
		}

		if (_state == State.MovingHome)
		{
            _creator.GlobalPosition = _creator.GlobalPosition.Lerp(_homePosition, _moveSpeed * (float)delta);

			if(_creator.GlobalPosition.DistanceTo(_homePosition) <= 0.02f)
			{
                _creator.GlobalPosition = _homePosition;
                _messenger.EmitActionCompleted(this);
                _state = State.Completed;
			}
		}

		if(_state == State.Completed)
		{
            // The node is queued for safe deletion
            // Until then it will remain in an idle state
            _state = State.AwaitingDeletion;
            QueueFree();
		}
	}

	public override void Execute()
	{
		_state = State.MovingToEnemy;
	}
}
