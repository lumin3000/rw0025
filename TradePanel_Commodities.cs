using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TradePanel_Commodities : TradePanel
{
	private const float TopAreaHeight = 70f;

	private const float BottomAreaHeight = 150f;

	private const float ColumnWidth = 120f;

	private const float FirstCommodityY = 16f;

	private float lastMoneyFlashTime = -100f;

	private Vector2 commoditiesScrollPosition = Vector2.zero;

	private static readonly Vector2 BottomButtonsSize = new Vector2(100f, 35f);

	private static readonly Vector2 TransactorLabelPos = new Vector2(34f, 10f);

	private static readonly Texture2D CommodityColumnBGTex = GenUI.HighlightTex;

	private static readonly Texture2D FlashTex = GenRender.SolidColorTexture(new Color(1f, 0f, 0f, 0.4f));

	public TradePanel_Commodities()
	{
		title = "Commodities";
	}

	public void FlashMoney()
	{
		lastMoneyFlashTime = Time.time;
	}

	public override void PanelOnGUITrade(Vector2 fillSize)
	{
		Rect position = new Rect(0f, 0f, 120f, fillSize.y - 150f);
		GUI.DrawTexture(position, CommodityColumnBGTex);
		Rect position2 = new Rect(fillSize.x - 120f, 0f, 120f, fillSize.y - 150f);
		GUI.DrawTexture(position2, CommodityColumnBGTex);
		Vector2 transactorLabelPos = TransactorLabelPos;
		Rect position3 = new Rect(0f, transactorLabelPos.y, 100f, 70f);
		Vector2 transactorLabelPos2 = TransactorLabelPos;
		position3.x = transactorLabelPos2.x;
		GUI.Label(position3, "Colony");
		float num = fillSize.x - position3.width;
		Vector2 transactorLabelPos3 = TransactorLabelPos;
		position3.x = num - transactorLabelPos3.x;
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label(position3, "Trader");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		Vector2 transactorLabelPos4 = TransactorLabelPos;
		Rect position4 = new Rect(180f, transactorLabelPos4.y, 200f, 200f);
		GUI.Label(position4, "Buy at");
		position4.x = 335f;
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label(position4, "Sell at");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = new Color(1f, 1f, 1f, 0.6f);
		GenUI.SetFontTiny();
		Rect position5 = new Rect(fillSize.x / 2f - 100f, 55f, 200f, 50f);
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(position5, "Drag to trade");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		GenUI.SetFontSmall();
		List<EntityType> list = Find.ActiveTrader.TradingCommodityTypes.ToList();
		float height = 16f + (float)list.Count * 45f;
		commoditiesScrollPosition = GUI.BeginScrollView(new Rect(0f, 70f, fillSize.x, fillSize.y - 70f - 150f), viewRect: new Rect(0f, 0f, fillSize.x - 24f, height), scrollPosition: commoditiesScrollPosition);
		float num2 = 16f;
		foreach (EntityType item in list)
		{
			if (item != EntityType.Money)
			{
				TradeUI.DrawResourceForTrade(item, num2);
				num2 += 45f;
			}
		}
		GUI.EndScrollView();
		float top = fillSize.y - 150f + 10f;
		Rect position7 = new Rect(0f, top, fillSize.x, 50f);
		GUI.BeginGroup(position7);
		TradeUI.DrawResourceForTrade(EntityType.Money, 0f);
		GUI.EndGroup();
		Rect position8 = new Rect(fillSize.x / 2f - 100f, top, 200f, 30f);
		if (Time.time - lastMoneyFlashTime < 0.5f)
		{
			GUI.DrawTexture(position8, FlashTex);
		}
		float num3 = fillSize.x / 2f;
		Vector2 bottomButtonsSize = BottomButtonsSize;
		float left = num3 - bottomButtonsSize.x - 5f;
		float y = fillSize.y;
		Vector2 bottomButtonsSize2 = BottomButtonsSize;
		float top2 = y - bottomButtonsSize2.y - 10f;
		Vector2 bottomButtonsSize3 = BottomButtonsSize;
		float x = bottomButtonsSize3.x;
		Vector2 bottomButtonsSize4 = BottomButtonsSize;
		Rect butRect = new Rect(left, top2, x, bottomButtonsSize4.y);
		if (UIWidgets.TextButton(butRect, "Trade"))
		{
			TradeSession.TryExecuteDeal();
		}
		butRect.x += butRect.width + 10f;
		if (UIWidgets.TextButton(butRect, "Reset"))
		{
			TradeSession.curDeal = new Deal();
		}
	}
}
