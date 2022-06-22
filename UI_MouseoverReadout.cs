using UnityEngine;

internal class UI_MouseoverReadout
{
	private const float YInterval = 22f;

	private static readonly Vector2 BotLeft = new Vector2(15f, 65f);

	public void MouseoverReadoutOnGUI()
	{
		if (Find.UIMapRoot.modeControls.openTab != Find.UIMapRoot.modeControls.tabInspect || Find.Selector.NumSelected > 0 || Find.Dialogs.TopDialog != null)
		{
			return;
		}
		GenUI.SetFontSmall();
		GUI.color = new Color(1f, 1f, 1f, 0.8f);
		IntVec3 intVec = Gen.MouseWorldSquare();
		if (!intVec.InBounds())
		{
			return;
		}
		float num = 0f;
		Rect position;
		if (intVec.IsFogged())
		{
			Vector2 botLeft = BotLeft;
			float x = botLeft.x;
			float num2 = Screen.height;
			Vector2 botLeft2 = BotLeft;
			position = new Rect(x, num2 - botLeft2.y - num, 999f, 999f);
			GUI.Label(position, "Undiscovered");
			return;
		}
		Vector2 botLeft3 = BotLeft;
		float x2 = botLeft3.x;
		float num3 = Screen.height;
		Vector2 botLeft4 = BotLeft;
		position = new Rect(x2, num3 - botLeft4.y - num, 999f, 999f);
		GUI.Label(position, Find.GlowGrid.PsychGlowAt(intVec).HumanName());
		num += 22f;
		Vector2 botLeft5 = BotLeft;
		float x3 = botLeft5.x;
		float num4 = Screen.height;
		Vector2 botLeft6 = BotLeft;
		position = new Rect(x3, num4 - botLeft6.y - num, 999f, 999f);
		TerrainDefinition terrainDefinition = Find.TerrainGrid.TerrainAt(intVec);
		string label = terrainDefinition.label;
		label = label + " (walk speed " + SpeedPercentStringOn(terrainDefinition) + ")";
		GUI.Label(position, label);
		num += 22f;
		foreach (Thing item in Find.Grids.ThingsAt(intVec))
		{
			if (item.def.ListOnReadout)
			{
				Vector2 botLeft7 = BotLeft;
				float x4 = botLeft7.x;
				float num5 = Screen.height;
				Vector2 botLeft8 = BotLeft;
				position = new Rect(x4, num5 - botLeft8.y - num, 999f, 999f);
				string labelMouseover = item.LabelMouseover;
				GUI.Label(position, labelMouseover);
				num += 22f;
			}
		}
		RoofDefinition roofDefinition = Find.RoofGrid.RoofDefAt(intVec);
		if (roofDefinition != null)
		{
			Vector2 botLeft9 = BotLeft;
			float x5 = botLeft9.x;
			float num6 = Screen.height;
			Vector2 botLeft10 = BotLeft;
			position = new Rect(x5, num6 - botLeft10.y - num, 999f, 999f);
			GUI.Label(position, roofDefinition.label);
			num += 22f;
		}
		GUI.color = Color.white;
	}

	private string SpeedPercentStringOn(EntityDefinition def)
	{
		float f = 14f / ((float)def.pathCost + 14f) * 100f;
		return Mathf.RoundToInt(f).ToString("##0") + "%";
	}
}
