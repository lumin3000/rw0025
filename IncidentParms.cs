public class IncidentParms : Saveable
{
	public Thing target;

	public int points = -1;

	public bool forceIncap;

	public void ExposeData()
	{
		Scribe.LookThingRef(ref target, "Target", this);
		Scribe.LookField(ref points, "Points");
		Scribe.LookField(ref forceIncap, "ForceIncap", defaultValue: false, forceSave: false);
	}

	public override string ToString()
	{
		if (target == null && points < 0)
		{
			return string.Empty;
		}
		string text = "[";
		if (target != null)
		{
			string text2 = text;
			text = string.Concat(text2, "target = ", target, " ");
		}
		if (points >= 0)
		{
			text = text + "points = " + points;
		}
		return text + "]";
	}
}
