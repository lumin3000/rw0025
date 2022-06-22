using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ITab_Pawn_Thoughts : ITab
{
	private const float TopAreaHeight = 70f;

	private const float MainBarsHeight = 80f;

	private const float InTitleAreaHeight = 75f;

	private const float InTitleLabelHeight = 28f;

	private const float ThoughtHeight = 20f;

	private const float ThoughtSpacing = 4f;

	private const float ThoughtInterval = 24f;

	private const float HappinessX = 235f;

	private const float FearX = 305f;

	private const float ThoughtSymbolSize = 32f;

	private const float BarAverageMarkerSize = 16f;

	private Vector2 thoughtScrollPosition = default(Vector2);

	private static readonly Vector2 LoyaltyBarSize = new Vector2(150f, 30f);

	private static readonly Vector2 TitleBarSize = new Vector2(120f, 23f);

	private static readonly Texture2D LoyaltyThresholdMarkerTex = Res.LoadTexture("UI/Widgets/LoyaltyThresholdMarker");

	private static readonly Texture2D BarAverageMarkerTex = Res.LoadTexture("UI/Widgets/BarAverageMarker");

	private static readonly Color HappinessColor = new Color(0.1f, 1f, 0.1f);

	private static readonly Color HappinessColorNegative = new Color(0f, 0.8f, 0f);

	private static readonly Color FearColor = new Color(1f, 0.1f, 0.1f);

	private static readonly Color FearColorNegative = new Color(0.5f, 0.05f, 0.05f);

	private static readonly Color NoHappinessFearColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

	public override bool IsVisible => base.SelPawn.psychology != null;

	public ITab_Pawn_Thoughts()
	{
		Size = new Vector2(400f, 440f);
		Label = "Thoughts";
	}

	public override void Opening()
	{
		thoughtScrollPosition = default(Vector2);
	}

	protected override void FillTab()
	{
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		Rect position = new Rect(0f, 0f, Size.x, 40f);
		GUI.Label(position, "Loyalty");
		float num = Size.x / 2f;
		Vector2 loyaltyBarSize = LoyaltyBarSize;
		float left = num - loyaltyBarSize.x / 2f;
		Vector2 loyaltyBarSize2 = LoyaltyBarSize;
		float x = loyaltyBarSize2.x;
		Vector2 loyaltyBarSize3 = LoyaltyBarSize;
		Rect rect = new Rect(left, 40f, x, loyaltyBarSize3.y);
		UIWidgets.FillableBar(rect, base.SelPawn.psychology.LoyaltyPercent);
		UIWidgets.FillableBarChangeArrows(rect, base.SelPawn.psychology.Loyalty.RateOfChange);
		float num2 = ((base.SelPawn.Team != TeamType.Prisoner) ? 0.1f : (base.SelPawn.prisoner.RecruitmentLoyaltyThreshold / 100f));
		Rect position2 = new Rect(rect.x - 2f + rect.width * num2, rect.y - 4f, 4f, rect.height + 8f);
		GUI.DrawTexture(position2, LoyaltyThresholdMarkerTex);
		Rect rect2 = new Rect(0f, 0f, Size.x, 70f);
		TooltipHandler.TipRegion(rect2, base.SelPawn.psychology.Loyalty.GetTooltipDef());
		Rect baseRect = new Rect(0f, 70f, Size.x, 80f);
		baseRect = baseRect.GetInnerRect(10f);
		GUI.BeginGroup(baseRect);
		Rect position3 = new Rect(0f, 0f, baseRect.width / 2f - 5f, baseRect.height);
		Rect position4 = new Rect(baseRect.width / 2f + 5f, 0f, baseRect.width / 2f - 5f, baseRect.height);
		GUI.BeginGroup(position3);
		Rect position5 = new Rect(0f, 0f, position3.width, 28f);
		GUI.Label(position5, "Happiness");
		float num3 = position3.width / 2f;
		Vector2 titleBarSize = TitleBarSize;
		float left2 = num3 - titleBarSize.x / 2f;
		Vector2 titleBarSize2 = TitleBarSize;
		float x2 = titleBarSize2.x;
		Vector2 titleBarSize3 = TitleBarSize;
		Rect rect3 = new Rect(left2, 28f, x2, titleBarSize3.y);
		UIWidgets.FillableBar(rect3, base.SelPawn.psychology.Happiness.PercentFull);
		UIWidgets.FillableBarChangeArrows(rect3, base.SelPawn.psychology.Happiness.RateOfChange);
		DrawBarAverageMarkerAt(new Vector2(rect3.x + rect3.width * base.SelPawn.psychology.Happiness.ThoughtsTotal / 100f, rect3.y + rect3.height));
		Rect rect4 = new Rect(0f, 0f, position3.width, 75f);
		TooltipHandler.TipRegion(rect4, base.SelPawn.psychology.Happiness.GetTooltipDef());
		GUI.EndGroup();
		GUI.BeginGroup(position4);
		Rect position6 = new Rect(0f, 0f, position4.width, 28f);
		GUI.Label(position6, "Fear");
		float num4 = position4.width / 2f;
		Vector2 titleBarSize4 = TitleBarSize;
		float left3 = num4 - titleBarSize4.x / 2f;
		Vector2 titleBarSize5 = TitleBarSize;
		float x3 = titleBarSize5.x;
		Vector2 titleBarSize6 = TitleBarSize;
		Rect rect5 = new Rect(left3, 28f, x3, titleBarSize6.y);
		UIWidgets.FillableBar(rect5, base.SelPawn.psychology.Fear.PercentFull);
		UIWidgets.FillableBarChangeArrows(rect5, base.SelPawn.psychology.Fear.RateOfChange);
		DrawBarAverageMarkerAt(new Vector2(rect5.x + rect5.width * base.SelPawn.psychology.Fear.ThoughtsTotal / 100f, rect5.y + rect5.height));
		Rect rect6 = new Rect(0f, 0f, position4.width, 75f);
		TooltipHandler.TipRegion(rect6, base.SelPawn.psychology.Fear.GetTooltipDef());
		GUI.EndGroup();
		GUI.EndGroup();
		Rect baseRect2 = new Rect(0f, 150f, Size.x, Size.y - 70f - 80f - 10f);
		baseRect2 = baseRect2.GetInnerRect(10f);
		DrawThoughtListing(baseRect2);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	private void DrawThoughtListing(Rect listingRect)
	{
		List<ThoughtType> list = base.SelPawn.psychology.thoughts.ThoughtTypesPresent.ToList();
		float height = (float)list.Count * 24f;
		thoughtScrollPosition = GUI.BeginScrollView(listingRect, thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 24f, height));
		GenUI.SetFontTiny();
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		float num = 0f;
		foreach (ThoughtType item in list)
		{
			Rect thoughtRect = new Rect(0f, num, listingRect.width - 24f, 20f);
			DrawThoughtGroup(thoughtRect, item);
			num += 24f;
		}
		GUI.EndScrollView();
	}

	private void DrawThoughtGroup(Rect ThoughtRect, ThoughtType ThType)
	{
		List<Thought> list = base.SelPawn.psychology.thoughts.ThoughtGroupOfType(ThType);
		ThoughtDefinition definition = ThType.GetDefinition();
		if (ThoughtRect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(ThoughtRect, GenUI.HighlightTex);
		}
		TooltipDef tooltipDef = new TooltipDef(list[0].Def.description, 7291);
		if (definition.duration > 5)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			if (list.Count == 1)
			{
				stringBuilder.Append("Expires in: " + (definition.duration - list[0].age).TicksInDaysString());
			}
			else
			{
				stringBuilder.Append("Starts expiring in: " + (definition.duration - list[0].age).TicksInDaysString());
				stringBuilder.AppendLine();
				stringBuilder.Append("Finishes expiring in: " + (definition.duration - list[list.Count - 1].age).TicksInDaysString());
			}
			tooltipDef.tipText += stringBuilder.ToString();
		}
		TooltipHandler.TipRegion(ThoughtRect, tooltipDef);
		GUI.BeginGroup(ThoughtRect);
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		Rect position = new Rect(10f, 0f, 400f, 28f);
		string text = ThType.GetDefinition().label;
		if (list.Count > 1)
		{
			text = text + " x" + list.Count;
		}
		GUI.Label(position, text);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		float num = base.SelPawn.psychology.thoughts.EffectOfThoughtGroup(ThType, ThoughtEffectType.Happiness);
		float num2 = base.SelPawn.psychology.thoughts.EffectOfThoughtGroup(ThType, ThoughtEffectType.Fear);
		if (num == 0f)
		{
			GUI.color = NoHappinessFearColor;
		}
		else if (num > 0f)
		{
			GUI.color = HappinessColor;
		}
		else
		{
			GUI.color = HappinessColorNegative;
		}
		Rect position2 = new Rect(235f, 0f, 32f, 32f);
		GUI.Label(position2, num.ToString("##0"));
		if (num2 == 0f)
		{
			GUI.color = NoHappinessFearColor;
		}
		else if (num2 > 0f)
		{
			GUI.color = FearColor;
		}
		else
		{
			GUI.color = FearColorNegative;
		}
		Rect position3 = new Rect(305f, 0f, 32f, 32f);
		GUI.Label(position3, num2.ToString("##0"));
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		GUI.EndGroup();
	}

	private void DrawBarAverageMarkerAt(Vector2 PointLoc)
	{
		Rect position = new Rect(PointLoc.x - 8f, PointLoc.y, 16f, 16f);
		GUI.DrawTexture(position, BarAverageMarkerTex);
	}
}
