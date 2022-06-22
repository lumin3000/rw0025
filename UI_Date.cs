using System.Text;
using UnityEngine;

public static class UI_Date
{
	private const float SeasonIconSize = 22f;

	private const float LabelHeight = 18f;

	private static readonly Texture2D LightSeasonIconTex = Res.LoadTexture("UI/Widgets/SeasonLightIcon");

	private static readonly Texture2D DarkSeasonIconTex = Res.LoadTexture("UI/Widgets/SeasonDarkIcon");

	private static float DayPassedPercent => (float)(Find.TickManager.tickCount % 20000) / 20000f;

	private static string DayPassedPercentString => (100f * DayPassedPercent).ToString("##0") + "%";

	private static int Hour => Mathf.FloorToInt(DayPassedPercent * 24f);

	public static void DateOnGUI(Rect dateRect)
	{
		if (dateRect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(dateRect, GenUI.HighlightTex);
		}
		GUI.BeginGroup(dateRect);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		float num = dateRect.height - 18f;
		float top = num;
		float num2 = dateRect.width * 1f / 6f;
		GenUI.SetFontSmall();
		Rect position = new Rect(num2 - 50f, 0f, 100f, num);
		GUI.Label(position, Hour + "h");
		GenUI.SetFontTiny();
		Rect position2 = new Rect(num2 - 50f, top, 100f, 18f);
		GUI.Label(position2, "Time");
		num2 = dateRect.width * 3f / 6f;
		GenUI.SetFontSmall();
		Rect position3 = new Rect(num2 - 50f, 0f, 100f, num);
		GUI.Label(position3, DateHandler.DayOfCurrentCycle.ToString());
		GenUI.SetFontTiny();
		Rect position4 = new Rect(num2 - 50f, top, 100f, 18f);
		GUI.Label(position4, "Day");
		num2 = dateRect.width * 5f / 6f;
		GenUI.SetFontSmall();
		Rect position5 = new Rect(num2 - 50f, 0f, 100f, num);
		GUI.Label(position5, (DateHandler.CyclesPassed + 1).ToString());
		GenUI.SetFontTiny();
		Rect position6 = new Rect(num2 - 50f, top, 100f, 18f);
		GUI.Label(position6, "Cycle");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.EndGroup();
		int tickCount = Find.TickManager.tickCount;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Current day is " + DayPassedPercentString + " over.");
		stringBuilder.AppendLine("Total days passed: " + DateHandler.DaysPassed);
		stringBuilder.Append("This moon completed " + DateHandler.CyclesPassed + " orbits of the gas giant since your arrival.");
		TooltipDef tip = new TooltipDef(stringBuilder.ToString(), 86423);
		TooltipHandler.TipRegion(dateRect, tip);
	}
}
