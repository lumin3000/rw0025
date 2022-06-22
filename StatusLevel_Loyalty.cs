using System.Text;

public class StatusLevel_Loyalty : StatusLevel
{
	public override string Label => "Loyalty";

	public override int RateOfChange => SourceLevel.RateOfChange;

	private StatusLevel SourceLevel
	{
		get
		{
			if (pawn.psychology.Happiness.curLevel > pawn.psychology.Fear.curLevel)
			{
				return pawn.psychology.Happiness;
			}
			return pawn.psychology.Fear;
		}
	}

	public StatusLevel_Loyalty(Pawn pawn)
		: base(pawn)
	{
	}

	public override void StatusLevelTick()
	{
		base.curLevel = SourceLevel.curLevel;
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		if (pawn.Team == TeamType.Colonist)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("Mental break threshold: " + 10f + "%");
		}
		else if (pawn.Team == TeamType.Prisoner)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("Recruitment threshold: " + pawn.prisoner.RecruitmentLoyaltyThreshold + "%");
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Loyalty represents a person's belief in the colony. If it falls too low, a person may give up, become irrational, or go insane.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Loyalty is equal to Happiness or Fear - whichever is greatest.");
		return new TooltipDef(stringBuilder.ToString(), 722713);
	}
}
