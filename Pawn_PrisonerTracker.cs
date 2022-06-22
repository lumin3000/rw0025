using UnityEngine;

public class Pawn_PrisonerTracker : Saveable
{
	private Pawn pawn;

	public bool getsFood = true;

	public bool tryRecruit;

	public PrisonerInteractionMode interactionMode;

	public int lastWardenVisitTime = -9999;

	public int MinInteractionInterval = 5000;

	public bool ScheduledForInteraction
	{
		get
		{
			if (interactionMode == PrisonerInteractionMode.NoInteraction)
			{
				return false;
			}
			return lastWardenVisitTime < Find.TickManager.tickCount - MinInteractionInterval;
		}
	}

	public float RecruitmentLoyaltyThreshold => pawn.kindDef.recruitmentLoyaltyThreshold;

	public bool Secure
	{
		get
		{
			if (pawn.MindState.brokenState != 0)
			{
				return false;
			}
			if (pawn.jobs.CurJob != null && pawn.jobs.CurJob.exitMapOnArrival)
			{
				return false;
			}
			return true;
		}
	}

	public Pawn_PrisonerTracker()
	{
	}

	public Pawn_PrisonerTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref interactionMode, "InteractionMode");
		Scribe.LookField(ref getsFood, "GetsFood");
		Scribe.LookField(ref tryRecruit, "attemptRecruit");
		Scribe.LookField(ref lastWardenVisitTime, "LastInteractionTime");
	}

	public void PrisonerTrackerTick()
	{
	}

	public bool TryRecruitBy(Pawn recruiter)
	{
		if (pawn.psychology.Loyalty.curLevel < RecruitmentLoyaltyThreshold)
		{
			UI_Messages.Message(string.Concat(recruiter.characterName, " failed to recruit ", pawn, " (prisoner below loyalty threshold)."), UIMessageSound.Silent);
			return false;
		}
		float num = 1f - pawn.kindDef.recruitmentLoyaltyThreshold / 100f;
		float num2 = (float)recruiter.skills.LevelOf(SkillType.Social) * 0.05f;
		float num3 = 1f - pawn.psychology.Loyalty.curLevel / 100f;
		float num4 = num * num2;
		string text = (num4 * 100f).ToString("##0") + "%";
		if (Random.value < num4)
		{
			UI_Messages.Message(string.Concat(recruiter.characterName, " failed to recruit ", pawn, " (", text, " chance)."), UIMessageSound.Negative);
			return false;
		}
		Find.LetterStack.ReceiveLetter(new Letter(string.Concat(recruiter, " successfully recruited ", pawn, " (", text, " chance)."), pawn));
		pawn.ChangePawnTeamTo(TeamType.Colonist);
		return true;
	}
}
