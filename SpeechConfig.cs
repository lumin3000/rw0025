using UnityEngine;

public class SpeechConfig
{
	public const float MaxTalkRange = 5f;

	public ThoughtType thoughtToGive;

	public SpeechEffect specialEffect;

	public bool TrySendFromTo(Pawn speaker, Pawn talkee)
	{
		if (speaker.talker.MouthBusyTalking)
		{
			Debug.LogError(string.Concat(speaker, " started talking to ", talkee, " while busy talking."));
		}
		if ((speaker.Position - talkee.Position).LengthHorizontalSquared > 25f)
		{
			return false;
		}
		if (!GenGrid.LineOfSight(speaker.Position, talkee.Position))
		{
			return false;
		}
		if (talkee.IsInBed() || speaker.IsInBed())
		{
			return false;
		}
		if (talkee.Incapacitated)
		{
			return false;
		}
		speaker.talker.LastTalkTime = Find.TickManager.tickCount;
		if (thoughtToGive != 0 && talkee.psychology != null)
		{
			Thought thought = new Thought(thoughtToGive);
			thought.effectMultiplier = 1f + (float)speaker.skills.LevelOf(SkillType.Social) * 0.1f;
			talkee.psychology.thoughts.GainThought(thought);
		}
		if (specialEffect == SpeechEffect.TryRecruit && talkee.Team == TeamType.Prisoner)
		{
			talkee.prisoner.TryRecruitBy(speaker);
		}
		MoteMaker.MakeSpeechOverlay(speaker);
		talkee.mind.Notify_SpeechReceived(this);
		return true;
	}
}
