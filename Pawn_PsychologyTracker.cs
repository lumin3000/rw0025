using UnityEngine;

public class Pawn_PsychologyTracker : Saveable
{
	public const float MentalBreakThreshold = 10f;

	private const float MentalBreakApproachingThreshold = 20f;

	protected const float MentalBreakChancePerTick = 5.0000002E-05f;

	private Pawn pawn;

	public ThoughtHandler thoughts;

	public PawnObserver observer;

	private StatusLevel_Loyalty pieceLoyalty;

	private StatusLevel_Happiness pieceHappiness;

	private StatusLevel_Fear pieceFear;

	private StatusLevel_Environment pieceEnvironment;

	private StatusLevel_Openness pieceOpenness;

	public bool MentalBreakImminent => Loyalty.curLevel < 10f;

	public bool MentalBreakApproaching => !MentalBreakImminent && Loyalty.curLevel < 20f;

	public StatusLevel_Loyalty Loyalty => pieceLoyalty;

	public float LoyaltyPercent => pieceLoyalty.PercentFull;

	public StatusLevel_Happiness Happiness => pieceHappiness;

	public StatusLevel_Fear Fear => pieceFear;

	public StatusLevel_Environment Environment => pieceEnvironment;

	public StatusLevel_Openness Openness => pieceOpenness;

	public Pawn_PsychologyTracker()
	{
	}

	public Pawn_PsychologyTracker(Pawn newPawn)
	{
		pawn = newPawn;
		thoughts = new ThoughtHandler(pawn);
		observer = new PawnObserver(pawn);
		pieceLoyalty = new StatusLevel_Loyalty(pawn);
		pieceHappiness = new StatusLevel_Happiness(pawn);
		pieceFear = new StatusLevel_Fear(pawn);
		pieceEnvironment = new StatusLevel_Environment(pawn);
		pieceOpenness = new StatusLevel_Openness(pawn);
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref pieceLoyalty, "LoyaltyBase", pawn);
		Scribe.LookSaveable(ref pieceHappiness, "PieceHappiness", pawn);
		Scribe.LookSaveable(ref pieceFear, "PieceFear", pawn);
		Scribe.LookSaveable(ref pieceEnvironment, "PieceEnvironment", pawn);
		Scribe.LookSaveable(ref pieceOpenness, "PieceOpenness", pawn);
		Scribe.LookSaveable(ref thoughts, "ThoughtHandler", pawn);
	}

	public void StatusTick()
	{
		thoughts.ThoughtRecordTick();
		observer.PawnObserverTick();
		pieceLoyalty.StatusLevelTick();
		pieceHappiness.StatusLevelTick();
		pieceFear.StatusLevelTick();
		pieceEnvironment.StatusLevelTick();
		pieceOpenness.StatusLevelTick();
		if (MentalBreakImminent && Random.value < 5.0000002E-05f && !pawn.Incapacitated && !pawn.IsInBed())
		{
			PsychologyUtility.DoMentalBreak(pawn);
		}
	}
}
