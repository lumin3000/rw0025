public abstract class StatusLevel : Saveable
{
	public const float MaxLevel = 100f;

	protected Pawn pawn;

	private float CurLevelInt = 50f;

	public float curLevel
	{
		get
		{
			return CurLevelInt;
		}
		set
		{
			CurLevelInt = value;
			if (CurLevelInt < 0f)
			{
				CurLevelInt = 0f;
			}
			if (CurLevelInt > 100f)
			{
				CurLevelInt = 100f;
			}
		}
	}

	public float PercentFull => curLevel / 100f;

	public abstract string Label { get; }

	public abstract int RateOfChange { get; }

	protected string TooltipBase => Label + ": " + curLevel.ToString("##0") + "%";

	public virtual bool ShouldTrySatisfy => false;

	public StatusLevel()
	{
	}

	public StatusLevel(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public virtual void ExposeData()
	{
		Scribe.LookField(ref CurLevelInt, "CurLevel");
	}

	public abstract void StatusLevelTick();

	public abstract TooltipDef GetTooltipDef();
}
