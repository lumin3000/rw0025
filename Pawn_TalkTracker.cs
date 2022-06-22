using System.Collections.Generic;
using UnityEngine;

public class Pawn_TalkTracker : Saveable
{
	protected const int MinTalkInterval = 350;

	protected const float IncidentalSpeechChancePerTick = 0.0025f;

	private Pawn pawn;

	public int LastTalkTime = -9999;

	public bool MouthBusyTalking => LastTalkTime + 350 > Find.TickManager.tickCount;

	public Pawn_TalkTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref LastTalkTime, "LastTalkTime", -9999);
	}

	public void TalkTrackerTick()
	{
		if (Random.value < 0.0025f)
		{
			TryIncidentalSocialSpeech();
		}
	}

	public bool TryIncidentalSocialSpeech()
	{
		if (MouthBusyTalking || pawn.Incapacitated || !pawn.raceDef.humanoid)
		{
			return false;
		}
		List<Pawn> list = Find.PawnManager.PawnsOnTeam[pawn.Team].ListFullCopy();
		list.Shuffle();
		foreach (Pawn item in list)
		{
			if (item != pawn)
			{
				SpeechConfig speechConfig = new SpeechConfig();
				speechConfig.thoughtToGive = ThoughtType.SocialTalk;
				if (speechConfig.TrySendFromTo(pawn, item))
				{
					pawn.skills.Learn(SkillType.Social, 4f);
					return true;
				}
			}
		}
		return false;
	}
}
