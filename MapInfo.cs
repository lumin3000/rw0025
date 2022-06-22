using System;

public class MapInfo : Saveable
{
	public string fileName = "Unnamed Map";

	public IntVec3 size = default(IntVec3);

	public int PowerOfTwoOverMapSize
	{
		get
		{
			int num = Math.Max(size.x, size.z);
			int num2;
			for (num2 = 1; num2 <= num; num2 *= 2)
			{
			}
			return num2;
		}
	}

	public int NumSquaresOnMap => size.x * size.y * size.z;

	public void ExposeData()
	{
		Scribe.LookField(ref fileName, "Name");
		Scribe.LookField(ref size, "Size");
		Scribe.LookField(ref ThingIDCounter.maxThingIDIndex, "MaxThingIDIndex");
	}
}
