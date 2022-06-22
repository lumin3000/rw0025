using UnityEngine;

public static class AirGrid
{
	private static int lastAirAddFrame = -99;

	private static float[,,] airGrid;

	private static bool DataIsExpired => Time.frameCount > lastAirAddFrame + 1;

	private static void ResetAirGrid()
	{
		airGrid = new float[Find.Map.Size.x, 1, Find.Map.Size.z];
	}

	public static void AddToAirGrid(Room enc)
	{
		if (DataIsExpired)
		{
			ResetAirGrid();
		}
		foreach (IntVec3 squares in enc.squaresList)
		{
			airGrid[squares.x, 0, squares.z] = enc.AirPressure;
		}
		lastAirAddFrame = Time.frameCount;
	}

	public static void FillFromAirGrid(Room enc)
	{
		if (DataIsExpired)
		{
			return;
		}
		foreach (IntVec3 squares in enc.squaresList)
		{
			enc.Air += airGrid[squares.x, 0, squares.z];
		}
	}
}
