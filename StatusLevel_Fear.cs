using System.Text;

public class StatusLevel_Fear : StatusLevel_HappinessFear
{
	public StatusLevel_Fear(Pawn pawn)
		: base(pawn)
	{
		EfType = ThoughtEffectType.Fear;
		TotalEffectBase = 0f;
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
		stringBuilder.Append("It rises when someone witnesses threats, brutality, and displays of power.");
		return new TooltipDef(stringBuilder.ToString(), 11121);
	}
}
