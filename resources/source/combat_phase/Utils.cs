using Godot;
using Godot.Collections;

public static partial class Utils
{
    public static Dictionary ShootRayCast(Camera3D cameraObj, uint layer = 1, float rayDistance = 1000)
    {
        var spaceState = cameraObj.GetWorld3D().DirectSpaceState;
        var mousePos = cameraObj.GetViewport().GetMousePosition();
        var origin = cameraObj.ProjectRayOrigin(mousePos);
        var end = origin + cameraObj.ProjectRayNormal(mousePos) * rayDistance;

        var query = PhysicsRayQueryParameters3D.Create(origin, end, layer);
        query.CollideWithAreas = true;
        var result = spaceState.IntersectRay(query);

        if (result.ContainsKey("collider"))
        {
            return result;
        }

        return null;
    }
}
