using UnityEngine;

public static class PsychologyUtility
{
	public static void DoMentalBreak(Pawn pawn)
	{
		int num = Random.Range(0, 3);
		if (pawn.Team == TeamType.Prisoner)
		{
			num = 0;
		}
		switch (num)
		{
		case 0:
		{
			DoMentalBreak(pawn, MindBrokenState.Psychotic);
			string text3 = string.Concat(pawn, " has has a mental break and is going on a rampage.");
			Find.LetterStack.ReceiveLetter(new Letter(text3));
			break;
		}
		case 1:
		{
			DoMentalBreak(pawn, MindBrokenState.GiveUpExit);
			string text2 = string.Concat(pawn, " has given up and is leaving from the colony.");
			Find.LetterStack.ReceiveLetter(new Letter(text2));
			break;
		}
		case 2:
		{
			DoMentalBreak(pawn, MindBrokenState.DazedWander);
			string text = string.Concat(pawn, " has given up and is wandering around in a daze.");
			Find.LetterStack.ReceiveLetter(new Letter(text));
			break;
		}
		}
	}

	public static void DoMentalBreak(Pawn pawn, MindBrokenState newState)
	{
		switch (newState)
		{
		case MindBrokenState.Psychotic:
			pawn.ChangePawnTeamTo(TeamType.Psychotic);
			pawn.MindState.brokenState = MindBrokenState.Psychotic;
			break;
		case MindBrokenState.GiveUpExit:
			pawn.ChangePawnTeamTo(TeamType.Traveler);
			pawn.MindState.brokenState = MindBrokenState.GiveUpExit;
			break;
		case MindBrokenState.DazedWander:
			pawn.ChangePawnTeamTo(TeamType.Traveler);
			pawn.MindState.brokenState = MindBrokenState.DazedWander;
			break;
		default:
			Debug.LogError("Unknown mental break state on " + pawn);
			break;
		}
		pawn.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
	}
}
