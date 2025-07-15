using Godot;
using System;
using System.Drawing;

public partial class TargetArrow : Path3D
{
    // Called when the node enters the scene tree for the first time.

    [Export]
    public float destinationX = 2f;
    [Export]
    public float destinationY = 3.3f;
    Camera3D _camera;
    Vector3 _viewPortToWorldMouse;
	public override void _Ready()
	{
        _camera = GetViewport().GetCamera3D();
		Curve = (Curve3D)Curve.Duplicate();
		//Curve.AddPoint(GlobalPosition);
		var mousePos = GetViewport().GetMousePosition();
        //Curve.AddPoint(new Vector3(mousePos.X, mousePos.Y, 0));
        //Curve.SetPointIn(1, new Vector3(-200, -25, 0));

        //Curve.AddPoint(new Vector3(GlobalPosition.X, 0, 0));

        /* var vertexLift = -0.5f;

         Vector2 pointA = new Vector2(GlobalPosition.X, GlobalPosition.Y);
         Vector2 pointB = new Vector2(0.473f, 0.213f);

         float h = (pointA.X + pointB.X) / 2f;
         float baseY = Mathf.Min(pointA.Y, pointB.Y); // Higher up on screen (lower value)
         float k = baseY - vertexLift;               // Lift vertex up by subtracting

         // Solve for parabola coefficient 'a'
         float a = (pointA.Y - k) / Mathf.Pow(pointA.X - h, 2);

         // Generate points between A.x and B.x
         float startX = Mathf.Min(pointA.X, pointB.X);
         float endX = Mathf.Max(pointA.X, pointB.X);


         for (float i = 0; i < 20; i += 1)
         {
             float t = i / (float)20;
             float x = Mathf.Lerp(startX, endX, t);
             float y = a * Mathf.Pow(x - h, 2) + k;

             Vector3 point = new Vector3(x, y, 0);

             GD.Print($"X:{x} Y:{y}");

             Curve.AddPoint(point);
             Curve.ClearPoints();
         }*/
        //Curve.AddPoint(new Vector3(Position.X, Position.Y, 0));
        //Curve.AddPoint(new Vector3(destinationX, destinationY, 0));

        //Curve.AddPoint(origin);
        Vector3 origin = new Vector3(GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z);
        //GD.Print(GlobalPosition.Y);
        Vector3 destination = new Vector3(3.8f, 0f, 2.5f);
        Vector3 vertex = origin + destination / 2 + new Vector3(0, 3, 0);
        float a = (origin.Y - vertex.Y) / Mathf.Pow(origin.X - vertex.X, 2);

        var numPoints = 20;
        var height = 4;
  
        for (float i = 0; i < numPoints; i += 1)
        {
            var x = Mathf.Lerp(origin.X, destination.X, i / numPoints);
            var y = a * Mathf.Pow(x - vertex.X, 2) + vertex.Y;
            GD.Print(y);
            Curve.AddPoint(ToLocal(new Vector3(x, y, 2.5f)));
        }
        //Curve.AddPoint(ToLocal(mid));
        //Curve.AddPoint(ToLocal(destination));

        //foreach (Vector3 point in Curve.GetBakedPoints())
        //{
        //    Curve.AddPoint(point);
        //}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

        /*Curve.ClearPoints();
        Curve.AddPoint(origin);
        Vector3 destination = ToLocal(new Vector3(_viewPortToWorldMouse.X, _viewPortToWorldMouse.Y, _viewPortToWorldMouse.Z));
        Vector3 mid = origin + destination / 2;
        Curve.AddPoint(ToLocal(mid + new Vector3(0, 10, 0)));
        Curve.AddPoint(ToLocal(destination));*/
    }

    /*public override void _PhysicsProcess(double delta)
    {
        var mousePos = GetViewport().GetMousePosition();
        var rayOrigin = _camera.ProjectRayOrigin(mousePos);
        var dir = _camera.ProjectRayNormal(mousePos);

        var spaceState = GetWorld3D().DirectSpaceState;
        var p = PhysicsRayQueryParameters3D.Create(rayOrigin, rayOrigin + dir * 1000f, 2);
        var result = spaceState.IntersectRay(p);

        if (result != null)
        {
            _viewPortToWorldMouse = (Vector3) result["position"];
        }
    }*/
}
