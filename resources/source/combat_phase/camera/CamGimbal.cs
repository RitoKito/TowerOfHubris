using Godot;
using System;
using System.Threading.Tasks;

public partial class CamGimbal : Node3D
{
	public static CamGimbal Instance { get; private set; }

	private IEventBus _eventBus;
	private Camera3D _cameraObj;
	private InputHandler _inputHandler;

	[Export] private float _rotationSpeed = 0.05f;
	private float _mouseMotionY = 0;
	private float _mouseMotionX = 0;
	private float _currentRotationX = 0;

	private float _currentPosX = 0.3f;

	private float _targetPositionH = 0;
	private float _targetPositionV = 0;

	private float _cameraMoveSpeed = 1f;
	private float _cameraMinY = 1.5f;
	private float _cameraMaxY = 3.5f;
	private float _maxCamAngle = Mathf.DegToRad(-20f);

	private Vector3 _levelTreePos = new Vector3(0f, 27f, -15f);
	private Vector3 _levelTreeRot = Vector3.Zero;
	private Vector3 _combatPos = new Vector3(0f, 1.815f, 4.9f);
	private Vector3 _combatRot = new Vector3(-13.5f, 0, 0);

	private bool _inCombat = false;
	// Boolean set by checking if the player clicked on an object
	private bool _clickedOnCollider = false;


	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_mouseMotionY = mouseMotion.Relative.Y;
			_mouseMotionX = mouseMotion.Relative.X;
		}

		CheckClickedObject(@event);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		_eventBus = EventBus.Instance;
		_cameraObj = GetViewport().GetCamera3D();
		_inputHandler = InputHandler.Instance;

		_eventBus.OnGameStateChanged += HandleGameStateChanged;

		SetLevelTreeCam();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_inCombat)
		{
			// Move camera only if user didn't click on interactable object
			// The boolean here prevents the camera movement until left mouse button is released
			if (_inputHandler.HoldingLeftMouse && !_inputHandler.ClickedOnCollider)
			{
				MoveCamGimbal(delta);
			}

			if (Position.Y >= 2.5f)
			{
				RotateCamGimbal(delta, _maxCamAngle);
			}
			else
			{
				RotateCamGimbal(delta, 0);
			}
		}
	}

	// Drag camera up and down
	public void MoveCamGimbal(double delta)
	{
		if(_mouseMotionY != 0)
		{
			Vector2 moveDirV = GetMouseMoveDir();

			_targetPositionV += moveDirV[1] * 1f;
			_targetPositionV = Math.Clamp(_targetPositionV, _cameraMinY, _cameraMaxY);
			Vector3 targetVector = new Vector3(Position.X, _targetPositionV, Position.Z);
			GlobalPosition = GlobalPosition.Lerp(targetVector, _cameraMoveSpeed * (float)delta);

			_mouseMotionY = 0;
		}
	}

	private Vector2 GetMouseMoveDir()
	{
		Vector2 moveDirV = new Vector2(0, 0);

		// Removes magnitude from mouse movement
		if (_mouseMotionY > 0)
		{
			moveDirV[1] = 1;
		}
		else
		{
			moveDirV[1] = -1;
		}

		return moveDirV;
	}

	// Rotate camera angle when camera reaches Y pos -0.45
	private void RotateCamGimbal(double delta, float targetAngle)
	{
		float currentAngle = Rotation.X;
		Rotation = new Vector3(Mathf.Lerp(currentAngle, targetAngle, 1f*(float)delta), 0, 0);
	}

	// Shoot raycast on mouse click
	// If no collision - allow camera movement
	// If collision - block cam movement until left button is released
	private void CheckClickedObject(InputEvent @event)
	{

		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				if (Utils.ShootRayCast(_cameraObj) != null)
				{
					_clickedOnCollider = true;
				}
				else
				{
					_clickedOnCollider = false;
				}
			}

			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased())
			{
				_clickedOnCollider = false;
			}
		}
	}

	private async Task HandleGameStateChanged(GameState state)
	{
		switch(state)
		{
			case GameState.LevelTree:
				SetLevelTreeCam();
				_inCombat = false;
				break;
			case GameState.Combat:
				SetCombatCam();
				_inCombat = true;
				break;
		}

		await Task.Yield();
	}


	private void SetLevelTreeCam()
	{
		GlobalPosition = _levelTreePos;
		Rotation = _levelTreeRot;
	}

	private void SetCombatCam()
	{
		GlobalPosition = _combatPos;
		//Rotation = _combatRot;
	}
}