using System.Collections.Generic;
using System.Linq;

public class AIKing_FleeChecker : Saveable
{
	private const float FleeThreshold = 0.3f;

	private AIKing king;

	public int numPawnsGained;

	private bool fled;

	public AIKing_FleeChecker(AIKing king)
	{
		this.king = king;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref fled, "Fled");
		Scribe.LookField(ref numPawnsGained, "NumPawnsGained");
	}

	public void Notify_PawnIncapped(Pawn p)
	{
		TestForFlee();
	}

	public void TestForFlee()
	{
		if (king.ownedPawns.Count() != 0)
		{
			int num = king.ownedPawns.Where((Pawn p) => !p.Incapacitated).Count();
			if ((float)num < (float)numPawnsGained * 0.3f)
			{
				Flee();
			}
		}
	}

	public void Flee()
	{
		if (fled)
		{
			return;
		}
		fled = true;
		List<Pawn> list = king.ownedPawns.Where((Pawn p) => !p.Incapacitated).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		UI_Messages.Message("Raiders are fleeing.", UIMessageSound.Standard);
		foreach (Pawn item in list)
		{
			item.mind.mindState.brokenState = MindBrokenState.PanicFlee;
			item.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
		}
	}
}
