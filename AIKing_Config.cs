public class AIKing_Config : Saveable
{
	private const int DefaultStagingTime = 10000;

	private const int DefaultAssaultingTime = 14000;

	public int stagingTime = 10000;

	public int assaultingTime = 14000;

	public TeamType team;

	public void ExposeData()
	{
		Scribe.LookField(ref team, "Team");
		Scribe.LookField(ref stagingTime, "StagingTime");
		Scribe.LookField(ref assaultingTime, "AssaultingTime");
	}
}
