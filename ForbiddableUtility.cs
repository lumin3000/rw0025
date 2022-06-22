using UnityEngine;

public static class ForbiddableUtility
{
	public static void SetForbidden(this Thing t, bool value)
	{
		ThingWithComponents thingWithComponents = t as ThingWithComponents;
		if (thingWithComponents == null)
		{
			Debug.LogError("Tried to SetForbidden on non-ThingWithComponents Thing " + t);
			return;
		}
		CompForbiddable comp = thingWithComponents.GetComp<CompForbiddable>();
		if (comp == null)
		{
			Debug.LogError("Tried to SetForbidden on non-Forbiddable Thing " + t);
		}
		else
		{
			comp.forbidden = value;
		}
	}

	public static bool IsForbidden(this Thing t)
	{
		ThingWithComponents thingWithComponents = t as ThingWithComponents;
		if (thingWithComponents == null)
		{
			return false;
		}
		CompForbiddable comp = thingWithComponents.GetComp<CompForbiddable>();
		if (comp == null)
		{
			return false;
		}
		if (comp.forbidden)
		{
			return true;
		}
		return false;
	}
}
