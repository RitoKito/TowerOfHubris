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

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            _mouseMotionY = mouseMotion.Relative.Y;
            _mouseMotionX = mouseMotion.Relative.X;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            MoveCamera(delta);
        }

        if(Position.Y >= 0.45f)
        {
            RotateCamera(delta, _maxCamAngle);
        }
        else
        {
            RotateCamera(delta, 0);
        }
    }

    private void MoveCamera(double delta)
    {
        if(_mouseMotionY != 0)
        {
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

    private void RotateCamera(double delta, float targetAngle)
    {
        var currentAngle = Rotation.X;
        Rotation = new Vector3(Mathf.Lerp(currentAngle, targetAngle, 1f*(float)delta), 0, 0);
    }
}

//-2 | 2