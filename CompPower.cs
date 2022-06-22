public abstract class CompPower : ThingComp
{
	public static float WattsToWattDaysPerTick = 4.6296296E-05f;

	public override string CompInspectString()
	{
		Building building = (Building)parent;
		PowerNet powerNet;
		if (building.powerNet != null)
		{
			powerNet = building.powerNet;
		}
		else
		{
			if (building.connectedToTransmitter == null)
			{
				return "Not connected to power.";
			}
			powerNet = building.connectedToTransmitter.powerNet;
		}
		string empty = string.Empty;
		empty = empty + "Connected rate: " + (powerNet.CurrentEnergyGainRate() / WattsToWattDaysPerTick).ToString("#######0") + " W";
		return empty + "\nConnected stored: " + powerNet.CurrentStoredEnergy().ToString("######0.0") + " Wd";
	}
}
