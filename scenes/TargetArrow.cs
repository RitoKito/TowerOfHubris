using Godot;
using System;
using System.Drawing;

public partial class TargetArrow : Path3D
{
    // Called when the node enters the scene tree for the first time.

    private Camera3D _camera;
    private Vector2 _mousePos;
    private float _curveStrenght = 1;
    private uint _collisionLayer = 2;
    private float _vertexHeight = 1;
    private Curve3D _curveDuplicate;
    private Node3D _targetCurvePos;
    public Vector3 GetTargetCurvePos {  get { return _targetCurvePos.GlobalPosition; } }
	public override void _Ready()
	{
        _camera = GetViewport().GetCamera3D();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        // Naturally all polygon objects in the scene would follow the original curve
        // Duplicating the curve makes polygons follow the duplicate of the parent
        _curveDuplicate = (Curve3D)Curve.Duplicate();
        Curve = _curveDuplicate;
        _targetCurvePos = GetNode<Node3D>("target_curve");
    }

    public void DrawTargetCurve(Vector3 destination)
    {
        Curve.ClearPoints();
        var origin = _targetCurvePos.GlobalPosition;
        var midpoint = (origin + destination) / 2;


        Curve.AddPoint(ToLocal(origin));
        Curve.AddPoint(ToLocal(destination));
        Curve.SetPointIn(1, ToLocal(new Vector3(origin.X - destination.X, midpoint.Y + _vertexHeight, midpoint.Z) * _curveStrenght));
    }

    public void DrawTargetArrow()
    {
        _mousePos = GetViewport().GetMousePosition();

        var rayCastResult = CombatUtils.ShootRayCast(_camera, _collisionLayer);
        if(rayCastResult != null)
        {
            var mouseCollision = (Vector3)rayCastResult["position"];

            _curveDuplicate.ClearPoints();
            var origin = new Vector3(GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z);
            var destination = new Vector3(mouseCollision.X, mouseCollision.Y, mouseCollision.Z);

            _curveDuplicate.AddPoint(ToLocal(origin));
            _curveDuplicate.AddPoint(ToLocal(destination));
        }
    }

    public void ClearTargetGizmos()
    {
       _curveDuplicate.ClearPoints();
    }
}
