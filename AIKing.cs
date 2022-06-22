using System.Collections.Generic;

public class AIKing : Saveable
{
	private const float FogClearRadius = 11.5f;

	public AIKing_Config config = new AIKing_Config();

	public AIKing_Cortex cortex;

	public AIKing_HitList hitList;

	public AIKing_FleeChecker fleeChecker;

	public List<Pawn> ownedPawns = new List<Pawn>();

	public IntVec3 basePoint = Find.Map.Center;

	public TeamType Team => config.team;

	public AIKing()
	{
		cortex = new AIKing_Cortex(this);
		hitList = new AIKing_HitList(this);
		fleeChecker = new AIKing_FleeChecker(this);
		Find.Map.aiKingManager.AddKing(this);
	}

	public AIKing(AIKing_Config InitData, IEnumerable<Pawn> InitialPawns)
		: this()
	{
		config = InitData;
		foreach (Pawn InitialPawn in InitialPawns)
		{
			AddPawn(InitialPawn);
		}
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref config, "Config");
		Scribe.LookSaveable(ref fleeChecker, "FleeChecker", this);
		Scribe.LookSaveable(ref cortex, "Cortex", this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			cortex.king = this;
		}
		Scribe.LookField(ref basePoint, "BaseCenter");
		List<Thing> valueList = new List<Thing>();
		foreach (Pawn ownedPawn in ownedPawns)
		{
			valueList.Add(ownedPawn);
		}
		Scribe.LookListThingRef(ref valueList, "OwnedPawns", this);
		ownedPawns = new List<Pawn>();
		foreach (Thing item in valueList)
		{
			ownedPawns.Add((Pawn)item);
		}
	}

	public void AddPawn(Pawn p)
	{
		ownedPawns.Add(p);
		fleeChecker.numPawnsGained++;
		UpdateAllPawnDuties();
	}

	public void DropInitialPawns()
	{
		basePoint = AIKingUtility.GoodDropSpot();
		string text = "Raiders have landed nearby.\n\nThey will prepare for a while, then attack.\n\nPrepare a defense or attack them pre-emptively.";
		Find.LetterStack.ReceiveLetter(new Letter(text, basePoint));
		foreach (Pawn ownedPawn in ownedPawns)
		{
			IntVec3 pos = DropPodUtility.DropPodSpotNear(basePoint);
			DropPodContentsInfo contents = new DropPodContentsInfo(ownedPawn);
			DropPodUtility.MakeDropPodAt(pos, contents);
		}
		Find.FogGrid.ClearFogCircle(basePoint, 11.5f);
		cortex.InitStagingCountdown();
	}

	public void AIKingTick()
	{
		cortex.AIKingCortexTick();
	}

	private void UpdateAllPawnDuties()
	{
		foreach (Pawn ownedPawn in ownedPawns)
		{
			if (cortex.cortexIntent == AIKingIntent.Staging)
			{
				ownedPawn.MindState.duty = AIDuty.Stage;
				ownedPawn.MindState.dutyLocation = basePoint;
			}
			if (cortex.cortexIntent == AIKingIntent.Assaulting)
			{
				ownedPawn.MindState.duty = AIDuty.Assault;
			}
			if (cortex.cortexIntent == AIKingIntent.Exiting)
			{
				ownedPawn.MindState.duty = AIDuty.Exit;
			}
		}
	}

	private void RemovePawn(Pawn p)
	{
		ownedPawns.Remove(p);
		if (ownedPawns.Count == 0)
		{
			Find.AIKingManager.RemoveKing(this);
		}
	}

	public void Notify_PawnDestroyed(Pawn p)
	{
		RemovePawn(p);
		fleeChecker.TestForFlee();
	}

	public void Notify_PawnChangingTeam(Pawn p)
	{
		fleeChecker.numPawnsGained--;
		RemovePawn(p);
	}

	public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
	{
		cortex.Notify_PawnDetectedEnemy(detector, newTarg);
	}

	public void Notify_PawnSpawned(Pawn p)
	{
		UpdateAllPawnDuties();
	}

	public void CortexSay_StartAttack()
	{
		UpdateAllPawnDuties();
	}

	public void CortexSay_Exit()
	{
		UpdateAllPawnDuties();
	}
}
