using Godot;
using Godot.Collections;

public static partial class Utils
{
	public static Dictionary ShootRayCast(Camera3D cameraObj, uint layer = 1, float rayDistance = 1000)
	{
		PhysicsDirectSpaceState3D spaceState = cameraObj.GetWorld3D().DirectSpaceState;
		
		Vector2 mousePos = cameraObj.GetViewport().GetMousePosition();
		Vector3 origin = cameraObj.ProjectRayOrigin(mousePos);
		Vector3 end = origin + cameraObj.ProjectRayNormal(mousePos) * rayDistance;


		
		
		
		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end, layer);
		query.CollideWithAreas = true;
		Dictionary result = spaceState.IntersectRay(query);

		if (result.ContainsKey("collider"))
		{
			return result;
		}

		return null;
	}
}
