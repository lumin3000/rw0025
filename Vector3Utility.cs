using UnityEngine;

public static class Vector3Utility
{
	public static Vector2 ToScreenPosition(this Vector3 v)
	{
		Vector3 vector = Find.CameraMap.camera.WorldToScreenPoint(v);
		return new Vector2(vector.x, (float)Screen.height - vector.y);
	}
}
