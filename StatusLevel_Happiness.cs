using System.Text;

public class StatusLevel_Happiness : StatusLevel_HappinessFear
{
	public StatusLevel_Happiness(Pawn pawn)
		: base(pawn)
	{
		EfType = ThoughtEffectType.Happiness;
		TotalEffectBase = 50f;
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Happiness is sustained by fulfilling physical and psychological needs.");
		return new TooltipDef(stringBuilder.ToString(), 17203);
	}
}
