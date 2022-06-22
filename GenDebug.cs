using System.Collections.Generic;
using UnityEngine;

public static class GenDebug
{
	public static void DebugPlaceSphere(Vector3 Loc, float Scale)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		gameObject.transform.position = Loc;
		gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
	}

	public static void LogList<T>(IEnumerable<T> list)
	{
		foreach (T item in list)
		{
			Debug.Log("    " + item.ToString());
		}
	}
}
