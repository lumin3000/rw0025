using System;

public class Pawn_MindHuman : Pawn_Mind
{
	public Pawn_WorkSettings workSettings;

	public JobGiver_JobQueue workJobQueue;

	public bool drafted;

	public Pawn_MindHuman(Pawn pawn)
		: base(pawn)
	{
		workSettings = new Pawn_WorkSettings(pawn);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref drafted, "Drafted");
		Scribe.LookSaveable(ref workSettings, "WorkSettings", pawn);
	}

	public void TakeOrderedJob(Job newJob)
	{
		if (pawn.HasAttachment(EntityType.Fire))
		{
			return;
		}
		if (newJob.jType == JobType.Goto)
		{
			newJob.targetA.Loc = SpilloverDestination(newJob.targetA.Loc, pawn);
		}
		if (newJob.jType == JobType.Wait)
		{
			newJob.targetA = new TargetPack(SpilloverDestination(pawn.Position, pawn));
			if (newJob.targetA.Loc != pawn.Position)
			{
				newJob.jType = JobType.Goto;
			}
		}
		if (pawn.Team == TeamType.Colonist)
		{
			if (newJob.jType == JobType.AttackMelee || newJob.jType == JobType.AttackStatic)
			{
				MoteMaker.ThrowFeedback(newJob.targetA.Loc, "FeedbackAttack");
			}
			if (newJob.jType == JobType.Goto)
			{
				MoteMaker.ThrowFeedback(newJob.targetA.Loc, "FeedbackGoto");
			}
			if (newJob.jType == JobType.Equip)
			{
				MoteMaker.ThrowFeedback(newJob.targetA.Loc, "FeedbackEquip");
			}
		}
		if (pawn.jobs.CurJob == null || !pawn.jobs.CurJob.JobIsSameAs(newJob))
		{
			pawn.stances.CancelActionIfPossible();
			if (newJob.jType == JobType.Goto)
			{
				Find.PawnDestinationManager.ReserveDestinationFor(pawn, newJob.targetA.Loc);
			}
			thinkNodeRoot.GetThinkNode<JobGiver_Orders>().QueueJob(newJob);
			if (pawn.jobs.CurJob != null)
			{
				pawn.jobs.CurJobDriver.EndJobWith(JobCondition.ForcedInterrupt);
			}
		}
	}

	private IntVec3 SpilloverDestination(IntVec3 root, Pawn searcher)
	{
		Predicate<IntVec3> predicate = (IntVec3 sq) => !Find.PawnDestinationManager.DestinationIsReserved(sq, searcher) && sq.Standable() && searcher.CanReach(sq);
		if (predicate(root))
		{
			return root;
		}
		int num = 1;
		IntVec3 result = default(IntVec3);
		float num2 = -1000f;
		bool flag = false;
		while (true)
		{
			IntVec3 intVec = root + Gen.RadialPattern[num];
			if (predicate(intVec))
			{
				float num3 = CoverUtility.SimpleCoverScoreAt(intVec);
				if (num3 > num2)
				{
					num2 = num3;
					result = intVec;
					flag = true;
				}
			}
			if (num >= 8 && flag)
			{
				break;
			}
			num++;
		}
		return result;
	}
}
