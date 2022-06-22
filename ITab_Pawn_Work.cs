using UnityEngine;

public class ITab_Pawn_Work : ITab
{
	private const float LabelMargin = 30f;

	private const float DraftRowY = 20f;

	private const float DraftButHeight = 28f;

	private const float PrioritiesTopY = 70f;

	private const float WorkListStartY = 65f;

	private const float PriorityIntervalX = 50f;

	private const float PrioritiesLeftX = 130f;

	private const float WorkIntervalY = 4f;

	private const float WorkNameLeftMargin = 5f;

	private static readonly Texture2D WorkBGTex = GenRender.SolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 0.3f));

	private static readonly Texture2D DraftedCoverTex = GenRender.SolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 0.4f));

	private static readonly Vector2 WinSize = new Vector2(300f, 400f);

	private static readonly Vector2 LabelSize = new Vector2(WinSize.x - 20f - 60f, 28f);

	public override bool IsVisible => base.SelPawn.Team == TeamType.Colonist;

	public ITab_Pawn_Work()
	{
		Size = WinSize;
		Label = "Work";
	}

	protected override void FillTab()
	{
		GenUI.SetFontSmall();
		Vector2 winSize = WinSize;
		float x = winSize.x;
		Vector2 winSize2 = WinSize;
		Rect innerRect = new Rect(0f, 0f, x, winSize2.y).GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		Vector2 labelSize = LabelSize;
		float x2 = labelSize.x;
		Vector2 labelSize2 = LabelSize;
		Rect rect = new Rect(5f, 20f, x2, labelSize2.y);
		GUI.Label(rect, "Soldier");
		Rect butRect = new Rect(130f, 20f, innerRect.width - 130f - 15f, 28f);
		if (UIWidgets.TextButton(butRect, (!base.SelPawn.MindHuman.drafted) ? "Draft" : "Undraft"))
		{
			base.SelPawn.UIToggleDrafted();
		}
		Rect rect2 = new Rect(rect);
		rect2.width = innerRect.width - rect2.x;
		TooltipHandler.TipRegion(rect2, WorkType.Soldier.GetDefinition().tooltipDesc);
		int num = 0;
		foreach (WorkDefinition item in WorkDefDatabase.AutomaticWorksInPriorityOrder)
		{
			DrawWork(item.wType, num);
			num++;
		}
		if (base.SelPawn.MindHuman.drafted)
		{
			Rect position = new Rect(0f, 70f, innerRect.width, innerRect.height - 70f);
			GUI.DrawTexture(position, DraftedCoverTex);
		}
		GUI.EndGroup();
	}

	private void DrawWork(WorkType wt, int index)
	{
		float num = index;
		Vector2 labelSize = LabelSize;
		Vector2 vector = new Vector2(30f, 65f + num * (labelSize.y + 4f));
		float x = vector.x;
		float y = vector.y;
		Vector2 labelSize2 = LabelSize;
		float x2 = labelSize2.x;
		Vector2 labelSize3 = LabelSize;
		Rect rect = new Rect(x, y, x2, labelSize3.y);
		if (index % 2 == 0)
		{
			GUI.DrawTexture(rect, WorkBGTex);
		}
		TooltipHandler.TipRegion(rect, UIWidgetsWork.TipForPawnWorker(base.SelPawn, wt));
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		rect.x += 5f;
		GUI.Label(rect, wt.GetLabel());
		Vector2 topLeft = new Vector2(vector.x + 130f, vector.y);
		UIWidgetsWork.DrawWorkBoxFor(topLeft, base.SelPawn, wt);
	}
}
