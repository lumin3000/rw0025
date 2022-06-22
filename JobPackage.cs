public class JobPackage
{
	public Job job;

	public ThinkNode finalNode;

	public JobPackage(Job job, ThinkNode givingNode)
	{
		this.job = job;
		finalNode = givingNode;
	}
}
