using UnityEngine;

public abstract class Pawn_Mind : Saveable
{
	public Pawn pawn;

	public ThinkNode thinkNodeRoot;

	public MindState mindState;

	private int jobsGivenThisFrame;

	private int lastFrameWhenJobWasGiven;

	public Pawn_Mind(Pawn pawn)
	{
		this.pawn = pawn;
		mindState = new MindState(pawn);
		thinkNodeRoot = ThinkNodeConfigContent.GetNewNodesForConfigFor(pawn.kindDef.thinkConfig, pawn, this);
	}

	public virtual void ExposeData()
	{
		Scribe.LookSaveable(ref mindState, "MindState", pawn);
	}

	public void MindTick()
	{
		if (pawn.jobs.CurJob == null)
		{
			TryStartNextJob();
		}
	}

	public virtual void Notify_JobEnded(Job endingJob, JobCondition condition)
	{
		if (condition == JobCondition.Succeeded && endingJob != null && endingJob.jType != JobType.Wait && !pawn.pather.moving)
		{
			pawn.jobs.StartJob(new Job(JobType.Wait, 1));
		}
		else
		{
			TryStartNextJob();
		}
	}

	private void TryStartNextJob()
	{
		if (pawn.jobs.CurJob != null)
		{
			Debug.LogWarning(string.Concat(pawn, " doing TryStartNextJob while still having job ", pawn.jobs.CurJob));
		}
		if (!pawn.Incapacitated && pawn.carrier == null)
		{
			StartNextJob();
		}
	}

	protected virtual void StartNextJob()
	{
		mindState.lastJobTag = JobTag.NoTag;
		JobPackage jobPackage = thinkNodeRoot.TryIssueJobPackage();
		if (jobPackage == null)
		{
			Debug.LogWarning(string.Concat(pawn, " did a StartNextJob but came up with no jobs."));
			pawn.jobs.StartJob(new Job(JobType.Wait, 60));
			return;
		}
		mindState.lastJobGiver = jobPackage.finalNode;
		Job job = jobPackage.job;
		if (Time.frameCount != lastFrameWhenJobWasGiven)
		{
			lastFrameWhenJobWasGiven = Time.frameCount;
			jobsGivenThisFrame = 0;
		}
		jobsGivenThisFrame++;
		if (jobsGivenThisFrame > 10)
		{
			string text = "null";
			if (job != null)
			{
				text = job.ToString();
			}
			Debug.LogWarning(string.Concat(pawn, " entered job-giving loop with Job=", text));
			pawn.jobs.StartJob(new Job(JobType.Wait, 100));
		}
		else
		{
			pawn.jobs.StartJob(job);
		}
	}

	public void Notify_SpeechReceived(SpeechConfig speech)
	{
	}
}
