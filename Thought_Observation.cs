public class Thought_Observation : Thought
{
	public int targetHash;

	public Thought_Observation()
	{
	}

	public Thought_Observation(ThoughtType ThType, Thing target)
		: base(ThType)
	{
		targetHash = target.GetHashCode();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref targetHash, "TargetHash");
	}
}
