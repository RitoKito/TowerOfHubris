using Godot;
using System;
using System.Drawing;

public partial class TargetArrow : Path3D
{
    // Called when the node enters the scene tree for the first time.

    private Camera3D _camera;
    private Curve3D _curveDuplicate;
    private Vector3 _targetCurvePos;
    public Vector3 TargetCurvePos {  get { return _targetCurvePos; } }
    private Node3D _arrowSprite;

    private Vector2 _mousePos;
    private float _curveStrenght = 1;
    private uint _collisionLayer = 2;
    private float _vertexHeight = 1;

	public override void _Ready()
	{
        _camera = GetViewport().GetCamera3D();

        // Naturally all CSGPolygon objects in the scene would follow the original curve
        // Duplicating the curve makes polygons follow the duplicate of the parent
        // TODO make the curve global
        _curveDuplicate = (Curve3D)Curve.Duplicate();
        Curve = _curveDuplicate;
        _targetCurvePos = GetNode<Node3D>("target_curve").GlobalPosition;
        _arrowSprite = GetNode<Node3D>("arrow_spr");
        _arrowSprite.Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

    }

    public void DrawTargetCurve(Vector3 destination)
    {
        Curve.ClearPoints();
        var origin = _targetCurvePos;
        var midpoint = (origin + destination) / 2;


        Curve.AddPoint(ToLocal(origin));
        Curve.AddPoint(ToLocal(destination));
        Curve.SetPointIn(1, ToLocal(new Vector3(origin.X - destination.X, midpoint.Y + _vertexHeight, midpoint.Z) * _curveStrenght));
    }

    public void DrawTargetArrow()
    {
        _arrowSprite.Show();

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


            var lastPoint = _curveDuplicate.GetPointPosition(_curveDuplicate.PointCount - 1);
           // var arrowPos = ToGlobal(new Vector3(lastPoint.X, lastPoint.Y, lastPoint.Z));
            _arrowSprite.GlobalPosition = destination;
            var dir = (destination - origin);
            var angle = Mathf.Atan2(dir.Y, dir.X);
            _arrowSprite.Rotation = new Vector3(0, 0, angle);
        }
    }

    public void HideTargettingUI()
    {
       _curveDuplicate.ClearPoints();
        _arrowSprite.Hide();
    }
}
