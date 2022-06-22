public static class PowerNetGrid
{
	private static PowerNet[,,] netGrid;

	public static void ResetStaticData()
	{
		netGrid = new PowerNet[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
	}

	public static PowerNet TransmittedPowerNetAt(IntVec3 loc)
	{
		return netGrid[loc.x, loc.y, loc.z];
	}

	public static void Notify_PowerNetCreated(PowerNet newNet)
	{
		foreach (Building transmitter in newNet.transmitters)
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(transmitter))
			{
				netGrid[item.x, item.y, item.z] = newNet;
			}
		}
	}

	public static void Notify_PowerNetDeleted(PowerNet deadNet)
	{
		foreach (Building transmitter in deadNet.transmitters)
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(transmitter))
			{
				netGrid[item.x, item.y, item.z] = null;
			}
		}
	}
}
