using Godot;
using System;

public partial class CamGimbal : Node3D
{
	[Export] private float _rotationSpeed = 0.05f;
	private float _mouseMotionY = 0;
	private float _mouseMotionX = 0;
	private float _currentRotationX = 0;

	private float _currentPosX = 0.3f;

	private float _targetPositionH = 0;
	private float _targetPositionV = 0;

	private float _maxCamAngle = Mathf.DegToRad(-20f);

    RayCast3D rayCast;
	Camera3D cam;
    Vector2 mousePos;
	bool rayCollision = false;


    public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion)
		{
			_mouseMotionY = mouseMotion.Relative.Y;
			_mouseMotionX = mouseMotion.Relative.X;
		}

		// Shoot raycast on mouse click
		// If no collision - allow movement
		// If collision - block cam movement until left button is released
		if(@event is InputEventMouseButton mouseButton)
		{
			if(mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				if (isRayCollision()){
					rayCollision = true;
				}
				else
				{
					rayCollision = false;
				}
			}

			if(mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased())
			{
				rayCollision = false;
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = GetNode<Camera3D>("Camera3D");
		rayCast = GetNode<RayCast3D>("Camera3D/RayCast3D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// Move camera only if user didn't click on interactable object
        if (Input.IsMouseButtonPressed(MouseButton.Left) && !rayCollision)
		{
			MoveCamera(delta);
        }

        if (Position.Y >= 0.45f)
        {
            RotateCamera(delta, _maxCamAngle);
        }

        else
        {
            RotateCamera(delta, 0);
        }
    }

	// Drag camera up and down
	private void MoveCamera(double delta)
	{
		if(_mouseMotionY != 0)
		{
			// Removes magnitude of mouse motion
			// Only moveDirV decides speed of camera movement
			var moveDirV = 0;

			if(_mouseMotionY > 0)
			{
				moveDirV = 1;
			}
			else
			{
				moveDirV = -1;
			}

			_targetPositionH += moveDirV * 1f;
			_targetPositionH = Math.Clamp(_targetPositionH, 0.3f, 0.5f);
			Vector3 targetVector = new Vector3(Position.X, _targetPositionH, Position.Z);
			GlobalPosition = GlobalPosition.Lerp(targetVector, 3f * (float)delta);

			_mouseMotionY = 0;
		}
    }

	// Rotate camera angle when camera reaches Y pos -0.45
	private void RotateCamera(double delta, float targetAngle)
	{
		var currentAngle = Rotation.X;
		Rotation = new Vector3(Mathf.Lerp(currentAngle, targetAngle, 1f*(float)delta), 0, 0);
	}

	// Shoot raycast from cam to check for interactable objects on left mouse click
	private bool isRayCollision()
	{
        mousePos = GetViewport().GetMousePosition();
        rayCast.TargetPosition = cam.ProjectLocalRayNormal(mousePos) * 100.0f;
        rayCast.ForceRaycastUpdate();

        if (rayCast.IsColliding())
        {
            rayCollision = true;
        }

		return rayCast.IsColliding();
    }
}