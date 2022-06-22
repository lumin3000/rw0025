using UnityEngine;

public static class TradeUI
{
	public const float IntervalYMain = 28f;

	public const float IntervalYTrading = 45f;

	public const float ResourceIconSize = 27f;

	private static EntityDefinition rDef;

	private static float curY;

	private static Trader trader;

	public static readonly Color NoTradeColor = new Color(0.5f, 0.5f, 0.5f);

	private static readonly Texture2D EmptyArrowRight = Res.LoadTexture("UI/Widgets/EmptyArrowRight");

	private static readonly Vector2 DragSliderSize = new Vector2(90f, 25f);

	public static void DrawResource(EntityType rType, float lineY)
	{
		curY = lineY;
		GenUI.SetFontSmall();
		rDef = rType.DefinitionOfType();
		DrawIcon(0f);
		DrawAmountHeld(34f, 100f, TextAnchor.MiddleLeft, TransactionType.PlayerBuys);
	}

	private static void DrawAmountHeld(float leftX, float width, TextAnchor alignment, TransactionType trans)
	{
		Rect position = new Rect(leftX, curY, width, 27f);
		int num;
		if (trans == TransactionType.PlayerBuys)
		{
			num = Find.ResourceManager.TotalAmountOf(rDef.eType);
			if (Find.ActiveTrader != null)
			{
				num += TradeSession.BoughtAmount(rDef.eType);
			}
		}
		else
		{
			num = trader.stockCommodities[rDef.eType];
			if (Find.ActiveTrader != null)
			{
				num -= TradeSession.BoughtAmount(rDef.eType);
			}
		}
		GUI.skin.label.alignment = alignment;
		GUI.Label(position, num.ToString());
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	public static void DrawResourceForTrade(EntityType rType, float lineY)
	{
		DrawResourceForTrade(rType, lineY, drawSlider: true);
	}

	public static void DrawResourceForTrade(EntityType rType, float lineY, bool drawSlider)
	{
		curY = lineY;
		trader = Find.ActiveTrader;
		rDef = rType.DefinitionOfType();
		DrawIcon(30f);
		DrawAmountHeld(64f, 80f, TextAnchor.MiddleLeft, TransactionType.PlayerBuys);
		DrawPriceType(110f, 100f, TextAnchor.MiddleRight, TransactionType.PlayerBuys);
		DrawPrice(220f, 100f, TextAnchor.MiddleLeft, TransactionType.PlayerBuys);
		if (drawSlider)
		{
			DrawDragSlider(375f);
		}
		DrawPrice(405f, 100f, TextAnchor.MiddleRight, TransactionType.TraderBuys);
		DrawPriceType(515f, 100f, TextAnchor.MiddleLeft, TransactionType.TraderBuys);
		DrawAmountHeld(605f, 80f, TextAnchor.MiddleRight, TransactionType.TraderBuys);
		DrawIcon(695f);
	}

	private static void DrawIcon(float leftX)
	{
		Rect rect = new Rect(leftX, curY, 27f, 27f);
		GUI.DrawTexture(rect, rDef.uiIcon);
		DoTooltip(rect);
	}

	private static void DoTooltip(Rect rect)
	{
		TooltipHandler.TipRegion(rect, rDef.label + ": " + rDef.desc);
	}

	private static void DrawPrice(float x, float width, TextAnchor alignment, TransactionType trans)
	{
		if (rDef.eType != EntityType.Money)
		{
			Rect position = new Rect(x, curY, width, 28f);
			GUI.skin.label.alignment = alignment;
			GUI.Label(position, "$" + trader.PriceOf(rDef.eType, trans));
		}
	}

	private static void DrawPriceType(float x, float width, TextAnchor alignment, TransactionType trans)
	{
		if (rDef.eType == EntityType.Money)
		{
			return;
		}
		PriceType priceType = trader.PriceTypeOf(rDef.eType, trans);
		if (priceType != 0)
		{
			string text = "ERROR in DrawPriceType";
			if (priceType == PriceType.VeryCheap)
			{
				GUI.color = new Color(0f, 1f, 0f);
				text = "Very cheap";
			}
			if (priceType == PriceType.Cheap)
			{
				GUI.color = new Color(0.5f, 1f, 0.5f);
				text = "Cheap";
			}
			if (priceType == PriceType.Normal)
			{
				text = string.Empty;
			}
			if (priceType == PriceType.Expensive)
			{
				GUI.color = new Color(1f, 0.5f, 0.5f);
				text = "Expensive";
			}
			if (priceType == PriceType.Exorbitant)
			{
				GUI.color = new Color(1f, 0f, 0f);
				text = "Exorbitant";
			}
			Rect position = new Rect(x, curY, width, 28f);
			GUI.skin.label.alignment = alignment;
			GUI.Label(position, text);
			GUI.color = Color.white;
		}
	}

	private static void DrawDragSlider(float centerX)
	{
		Vector2 dragSliderSize = DragSliderSize;
		float left = centerX - dragSliderSize.x / 2f;
		float top = curY + 4f;
		Vector2 dragSliderSize2 = DragSliderSize;
		float x = dragSliderSize2.x;
		Vector2 dragSliderSize3 = DragSliderSize;
		Rect rect = new Rect(left, top, x, dragSliderSize3.y);
		float height = rect.height;
		bool flag = TradeSession.curDeal.AmountPlayerBuying(rDef.eType) > 0;
		bool flag2 = TradeSession.curDeal.AmountPlayerBuying(rDef.eType) < 0;
		Rect slRect = new Rect(rect);
		slRect.x -= height;
		slRect.width += height * 2f;
		if (rDef.eType != EntityType.Money && DragSliderManager.DragSlider(slRect, TradeSliders.TradeSliderDraggingUpdate, TradeSliders.TradeSliderDraggingCompleted))
		{
			TradeSliders.dragEntityType = rDef.eType;
			TradeSliders.dragBaseAmount = TradeSession.curDeal.AmountPlayerBuying(rDef.eType);
			TradeSliders.dragLimitWarningGiven = false;
		}
		int num = TradeSession.curDeal.AmountPlayerBuying(rDef.eType);
		string empty = string.Empty;
		if (num > 0)
		{
			empty = num.ToString();
		}
		else if (num < 0)
		{
			empty = (-num).ToString();
		}
		else
		{
			GUI.color = NoTradeColor;
			empty = "0";
		}
		if (slRect.Contains(Event.current.mousePosition))
		{
			GUI.color = Color.yellow;
		}
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		rect.y += 2f;
		GUI.Label(rect, empty);
		rect.y -= 2f;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		if (flag || flag2)
		{
			Rect position = new Rect(rect.x + rect.width / 2f - (float)(EmptyArrowRight.width / 2), rect.y + rect.height / 2f - (float)(EmptyArrowRight.height / 2), EmptyArrowRight.width, EmptyArrowRight.height);
			if (flag)
			{
				position.x += position.width;
				position.width *= -1f;
			}
			GUI.DrawTexture(position, EmptyArrowRight);
		}
	}
}
