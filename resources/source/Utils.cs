using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System;

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

	public static List<int> GetThreeUniqueRandomNumbers(int min, int max)
	{
		List<int> numbers = new();
		for (int i = min; i < max; i++)
			numbers.Add(i);

		if (numbers.Count < 3)
			throw new ArgumentException("Range must contain at least 3 unique numbers.");

		Random rng = new();
		for (int i = numbers.Count - 1; i > 0; i--)
		{
			int j = rng.Next(i + 1);
			(numbers[i], numbers[j]) = (numbers[j], numbers[i]);
		}

		return numbers.GetRange(0, 3);
	}
}
