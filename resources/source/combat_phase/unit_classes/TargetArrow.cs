using Godot;
using Godot.Collections;

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
	private float _vertexHeight = 0.2f;

	public override void _Ready()
	{
		_camera = GetViewport().GetCamera3D();

		// Naturally all CSGPolygon objects in the scene would follow the original curve
		// Duplicating the curve makes polygons follow the duplicate of the parent
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
		Vector3 origin = _targetCurvePos;
		Vector3 midpoint = (origin + destination) / 2;


		Curve.AddPoint(ToLocal(origin));
		Curve.AddPoint(ToLocal(destination));

		float _vertexHeightPerDistance = _vertexHeight + GlobalPosition.DistanceTo(destination / 10);
        Curve.SetPointIn(1, ToLocal(new Vector3(origin.X - destination.X, _vertexHeightPerDistance, midpoint.Z) * _curveStrenght));
	}

	public void DrawTargetArrow()
	{
		_arrowSprite.Show();

		_mousePos = GetViewport().GetMousePosition();

		Dictionary rayCastResult = Utils.ShootRayCast(_camera, _collisionLayer);
		if(rayCastResult != null)
		{
			Vector3 mouseCollision = (Vector3)rayCastResult["position"];

			_curveDuplicate.ClearPoints();
			Vector3 origin = new Vector3(GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z);

			
			
			Vector3 destination = new Vector3(mouseCollision.X, mouseCollision.Y, mouseCollision.Z);

			_curveDuplicate.AddPoint(ToLocal(origin));
			_curveDuplicate.AddPoint(ToLocal(destination));

			_arrowSprite.GlobalPosition = destination;
			Vector3 dir = (destination - origin);
			float angle = Mathf.Atan2(dir.Y, dir.X);
			_arrowSprite.Rotation = new Vector3(0, 0, angle);
		}
	}

	public void HideTargetingUI()
	{
	   _curveDuplicate.ClearPoints();
		_arrowSprite.Hide();
	}
}
