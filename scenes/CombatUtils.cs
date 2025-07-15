

using Godot;
using Godot.Collections;

public static partial class CombatUtils
{
    public static Dictionary ShootRayCast(Camera3D cameraObj, float rayDistance = 1000)
    {
        var spaceState = cameraObj.GetWorld3D().DirectSpaceState;
        var mousePos = cameraObj.GetViewport().GetMousePosition();
        var origin = cameraObj.ProjectRayOrigin(mousePos);
        var distance = rayDistance;
        var end = origin + cameraObj.ProjectLocalRayNormal(mousePos) * rayDistance;

        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithAreas = true;
        var result = spaceState.IntersectRay(query);

        if (result.ContainsKey("collider"))
        {
            return result;
        }

        return null;
    }
}
