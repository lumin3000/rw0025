using UnityEngine;

public class Tab_Overview_Work : UIPanel_Tab
{
	private const float TopAreaHeight = 40f;

	private const float LabelRowHeight = 50f;

	private const float PawnRowHeight = 30f;

	private const float WorkBoxMargin = 2.5f;

	private const float WorkBoxSpacing = 30f;

	private const float NameColumnWidth = 180f;

	private const float NameLeftMargin = 15f;

	private const float StrikethroughY = 15f;

	private Vector2 pawnsScrollPosition = Vector2.zero;

	public Tab_Overview_Work()
	{
		title = "Work";
	}

	public override void PanelOnGUI(Rect fillRect)
	{
		Rect innerRect = fillRect.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		Rect position = new Rect(0f, 0f, innerRect.width, 40f);
		GUI.BeginGroup(position);
		GenUI.SetFontSmall();
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		Rect rect = new Rect(5f, 5f, 140f, 30f);
		UIWidgets.LabelCheckbox(rect, "Manual priorities", ref Find.Map.playSettings.useWorkPriorities);
		float num = position.width / 3f;
		float num2 = position.width * 2f / 3f;
		Rect position2 = new Rect(num - 50f, 5f, 100f, 30f);
		Rect position3 = new Rect(num2 - 50f, 5f, 100f, 30f);
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GenUI.SetFontTiny();
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(position2, "<-- Higher priority");
		GUI.Label(position3, "Lower priority -->");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.EndGroup();
		Rect position4 = new Rect(0f, 40f, innerRect.width, innerRect.height - 40f);
		GUI.BeginGroup(position4);
		GenUI.SetFontSmall();
		GUI.color = Color.white;
		float num3 = 180f;
		int num4 = 0;
		foreach (WorkDefinition item in WorkDefDatabase.AutomaticWorksInPriorityOrder)
		{
			float num5 = num3 + 15f;
			Rect position5 = new Rect(num5 - 100f, 0f, 200f, 30f);
			if (num4 % 2 == 1)
			{
				position5.y += 20f;
			}
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(position5, item.gerundLabel);
			num3 += 60f;
			num4++;
		}
		float height = (float)Find.PawnManager.Colonists.Count * 30f;
		Rect position6 = new Rect(0f, 50f, position4.width, position4.height - 50f);
		Rect rect2 = new Rect(0f, 0f, position4.width - 24f, height);
		pawnsScrollPosition = GUI.BeginScrollView(position6, pawnsScrollPosition, rect2);
		float num6 = 0f;
		foreach (Pawn colonist in Find.PawnManager.Colonists)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.2f);
			GenUI.DrawLineHorizontal(new Vector2(0f, num6), rect2.width);
			GUI.color = Color.white;
			DrawPawnRow(colonist, num6, rect2);
			num6 += 30f;
		}
		GUI.EndScrollView();
		GUI.EndGroup();
		GUI.EndGroup();
	}

	private void DrawPawnRow(Pawn p, float rowY, Rect fillRect)
	{
		Rect position = new Rect(0f, rowY, fillRect.width, 30f);
		if (position.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(position, GenUI.HighlightTex);
		}
		Rect rect = new Rect(0f, rowY, 180f, 30f);
		Rect innerRect = rect.GetInnerRect(3f);
		if (p.healthTracker.Health < p.healthTracker.MaxHealth - 1)
		{
			Rect screenRect = new Rect(rect);
			screenRect.xMin -= 4f;
			screenRect.yMin += 4f;
			screenRect.yMax -= 6f;
			UIWidgets.FillableBar(screenRect, (float)p.healthTracker.Health / (float)p.healthTracker.MaxHealth, PawnUIOverlay.HealthTex, doBlackBorder: false, GenUI.ClearTex);
		}
		if (rect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(innerRect, GenUI.HighlightTex);
		}
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		Rect position2 = new Rect(rect);
		position2.xMin += 15f;
		GUI.Label(position2, p.Label);
		if (UIWidgets.InvisibleButton(rect))
		{
			Find.UIRoot.dialogs.PopBox();
			Find.CameraMap.JumpTo(p.Position);
			return;
		}
		TooltipDef tooltip = p.GetTooltip();
		tooltip.tipText = "Click to jump to:\n\n" + tooltip.tipText;
		TooltipHandler.TipRegion(rect, tooltip);
		float num = 180f;
		GenUI.SetFontMedium();
		foreach (WorkDefinition item in WorkDefDatabase.AutomaticWorksInPriorityOrder)
		{
			Vector2 topLeft = new Vector2(num + 2.5f, rowY + 2.5f);
			UIWidgetsWork.DrawWorkBoxFor(topLeft, p, item.wType);
			Rect rect2 = new Rect(topLeft.x, topLeft.y, 25f, 25f);
			TooltipHandler.TipRegion(rect2, UIWidgetsWork.TipForPawnWorker(p, item.wType));
			num += 60f;
		}
		if (p.Incapacitated)
		{
			GUI.color = Color.red;
			GenUI.DrawLineHorizontal(new Vector2(180f, rowY + 15f), num - 180f - 17f);
			GUI.color = Color.white;
		}
	}
}
