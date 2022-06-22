public class DropPodContentsInfo : Saveable
{
	public Thing containedThing;

	public DropPodContentsInfo()
	{
	}

	public DropPodContentsInfo(Thing t)
	{
		containedThing = t;
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref containedThing, "ContainedThing");
	}
}
