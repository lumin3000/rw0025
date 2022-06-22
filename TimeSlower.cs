public class TimeSlower
{
	private const int ForceTicksStandard = 790;

	private const int ForceTicksShort = 250;

	private int forceNormalSpeedUntil;

	public bool ForcedNormalSpeed => Find.TickManager.tickCount < forceNormalSpeedUntil;

	public void SignalForceNormalSpeed()
	{
		forceNormalSpeedUntil = Find.TickManager.tickCount + 790;
	}

	public void SignalForceNormalShort()
	{
		forceNormalSpeedUntil = Find.TickManager.tickCount + 250;
	}
}
