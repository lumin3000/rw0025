using UnityEngine;

public static class ThingIDCounter
{
	public static int maxThingIDIndex;

	public static void Reset()
	{
		maxThingIDIndex = 0;
	}

	public static void GiveIDTo(Thing t)
	{
		if (t.def.HasThingIDNumber)
		{
			if (t.thingIDNumber != -1)
			{
				Debug.LogError(string.Concat("Giving ID to ", t, " which already has id ", t.thingIDNumber));
			}
			maxThingIDIndex++;
			t.thingIDNumber = maxThingIDIndex;
		}
	}
}
