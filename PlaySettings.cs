public class PlaySettings : Saveable
{
	public bool useWorkPriorities;

	public void ExposeData()
	{
		Scribe.LookField(ref useWorkPriorities, "UseWorkPriorities");
	}
}
