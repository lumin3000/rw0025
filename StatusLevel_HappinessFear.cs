using System.Text;

public class StatusLevel_HappinessFear : StatusLevel
{
	public const float ApproachAverageTightness = 0.0005f;

	private const float RateOfChangeDisplayMultiplier = 600f;

	protected ThoughtEffectType EfType;

	protected float TotalEffectBase;

	public override string Label => EfType.ToString();

	public override int RateOfChange => (int)(ChangeThisTick * 600f);

	public float ThoughtsTotal
	{
		get
		{
			float num = TotalEffectBase;
			foreach (ThoughtType item in pawn.psychology.thoughts.ThoughtTypesPresent)
			{
				num += pawn.psychology.thoughts.EffectOfThoughtGroup(item, EfType);
			}
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > 100f)
			{
				num = 100f;
			}
			return num;
		}
	}

	private float ChangeThisTick
	{
		get
		{
			float num = ThoughtsTotal - base.curLevel;
			return num * 0.0005f;
		}
	}

	public StatusLevel_HappinessFear(Pawn pawn)
		: base(pawn)
	{
	}

	public override void StatusLevelTick()
	{
		base.curLevel += ChangeThisTick;
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Fear represents how intimidated someone is by their environment.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("It rises witnesses threats, brutality, and displays of power.");
		return new TooltipDef(stringBuilder.ToString(), 11121);
	}
}
