using UnityEngine;

public static class SkillUI
{
	public enum SkillDrawMode
	{
		Gameplay,
		Menu
	}

	private const float SkillHeight = 24f;

	private const float SkillYSpacing = 3f;

	private const float LevelNumberX = 120f;

	private const float ProgBarX = 160f;

	private const float ProgBarHeight = 15f;

	private const float SideMargin = 15f;

	private const float TopMargin = 7f;

	private const float IncButX = 205f;

	private const float IncButSpacing = 10f;

	private static readonly Texture2D HighlightColTex = GenRender.SolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

	private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

	public static void DrawSkillsOf(Pawn p, Vector2 Offset, SkillDrawMode DrawMode)
	{
		GenUI.SetFontSmall();
		int num = 0;
		foreach (Skill allSkill in p.skills.AllSkills)
		{
			float y = (float)num * 27f + Offset.y;
			allSkill.DrawAt(new Vector2(Offset.x, y), DrawMode);
			num++;
		}
	}

	private static void DrawAt(this Skill skill, Vector2 topLeft, SkillDrawMode drawMode)
	{
		float num = 0f;
		if (drawMode == SkillDrawMode.Gameplay)
		{
			Vector2 displayOffset = ITab_Pawn_Skills.DisplayOffset;
			num = 300f - displayOffset.x * 2f;
		}
		if (drawMode == SkillDrawMode.Menu)
		{
			num = 150f;
		}
		float width = num - 160f - 15f;
		Rect rect = new Rect(topLeft.x, topLeft.y, num, 24f);
		if (rect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(rect, HighlightColTex);
		}
		GUI.BeginGroup(rect);
		Rect position = new Rect(15f, 7f, 999f, 999f);
		GUI.Label(position, skill.Label);
		Rect position2 = new Rect(120f, 0f, 24f, 35f);
		if (skill.Disabled)
		{
			GUI.color = DisabledSkillColor;
		}
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(position2, skill.LevelString);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		if (drawMode == SkillDrawMode.Gameplay)
		{
			Rect rect2 = new Rect(160f, 8f, width, 15f);
			if (skill.level < 20)
			{
				UIWidgets.FillableBar(rect2, skill.XpProgressPercent);
			}
			else
			{
				GenUI.SetFontTiny();
				GUI.Label(rect2, "MAX");
				GenUI.SetFontSmall();
			}
		}
		GUI.EndGroup();
		TooltipHandler.TipRegion(rect, skill.GetTooltip());
	}
}
