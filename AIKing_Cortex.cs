public class AIKing_Cortex : Saveable
{
	public AIKing king;

	public AIKingIntent cortexIntent;

	private int ticksSpentStaging;

	private int ticksSpentAssaulting;

	public AIKing_Cortex()
	{
	}

	public AIKing_Cortex(AIKing King)
	{
		king = King;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref cortexIntent, "CortexState");
		Scribe.LookField(ref ticksSpentStaging, "TicksSpentStaging");
		Scribe.LookField(ref ticksSpentStaging, "TicksSpentAssaulting");
	}

	public void InitStagingCountdown()
	{
		cortexIntent = AIKingIntent.Staging;
	}

	public void AIKingCortexTick()
	{
		if (cortexIntent == AIKingIntent.Staging)
		{
			ticksSpentStaging++;
			if (ticksSpentStaging >= king.config.stagingTime)
			{
				StartAssault();
			}
		}
		if (cortexIntent == AIKingIntent.Assaulting)
		{
			ticksSpentAssaulting++;
			if (ticksSpentAssaulting >= king.config.assaultingTime)
			{
				UI_Messages.Message("Raiders have given up and are leaving.");
				cortexIntent = AIKingIntent.Exiting;
				king.CortexSay_Exit();
			}
		}
	}

	public void StartAssault()
	{
		if (cortexIntent != AIKingIntent.Assaulting)
		{
			UI_Messages.Message("Raiders have finished staging and are beginning their assault.", UIMessageSound.SeriousAlert);
			StartAttack();
		}
	}

	public void Notify_PawnDetectedEnemy(Pawn detector, Thing newTarg)
	{
		if (newTarg.Team == TeamType.Colonist && cortexIntent == AIKingIntent.Staging)
		{
			UI_Messages.Message("Raiders have detected you and begun their assault early.", UIMessageSound.SeriousAlert);
			StartAttack();
		}
	}

	private void StartAttack()
	{
		cortexIntent = AIKingIntent.Assaulting;
		king.CortexSay_StartAttack();
		Find.Tutor.Signal(TutorSignal.EnemiesAttacking);
	}
}
