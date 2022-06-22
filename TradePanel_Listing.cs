using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TradePanel_Listing : TradePanel
{
	private const float MoneyAreaHeight = 70f;

	private const float MoneyDrawHeight = 35f;

	private const int SectTitleHeight = 30;

	private const float ItemBuyInterfaceHeight = 76f;

	private const float BuyButtonWidth = 90f;

	private const float InfoButLeftX = 130f;

	protected string buySectionTitle = "Buyables:";

	protected string sellSectionTitle = "Sellables:";

	private Vector2 scrollPos = Vector2.zero;

	private static readonly Vector2 TransactorLabelPos = new Vector2(34f, 0f);

	private static readonly Texture2D PawnBuyInterfaceBGTex = GenRender.SolidColorTexture(new Color(0f, 0f, 0f, 0.2f));

	protected abstract bool WillTrade { get; }

	protected abstract string NoTradeString { get; }

	protected abstract IEnumerable<Tradeable> Buyables { get; }

	protected abstract IEnumerable<Tradeable> Sellables { get; }

	public TradePanel_Listing()
	{
	}

	public override void PanelOnGUITrade(Vector2 fillSize)
	{
		if (!WillTrade)
		{
			Rect position = new Rect(0f, 0f, fillSize.x, 100f);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(position, NoTradeString);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			return;
		}
		Vector2 transactorLabelPos = TransactorLabelPos;
		Rect position2 = new Rect(0f, transactorLabelPos.y, 100f, 70f);
		Vector2 transactorLabelPos2 = TransactorLabelPos;
		position2.x = transactorLabelPos2.x;
		GUI.Label(position2, "Colony");
		float num = fillSize.x - position2.width;
		Vector2 transactorLabelPos3 = TransactorLabelPos;
		position2.x = num - transactorLabelPos3.x;
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label(position2, "Trader");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		Rect position3 = new Rect(0f, 35f, fillSize.x, 999f);
		GUI.BeginGroup(position3);
		TradeUI.DrawResourceForTrade(EntityType.Money, 0f, drawSlider: false);
		GUI.EndGroup();
		List<Tradeable> list = Buyables.ToList();
		List<Tradeable> list2 = Sellables.ToList();
		float height = 60f + (float)(list.Count + list2.Count) * 86f;
		Rect viewRect = new Rect(0f, 0f, fillSize.x - 24f, height);
		Rect position4 = new Rect(0f, 70f, fillSize.x, fillSize.y - 70f);
		scrollPos = GUI.BeginScrollView(position4, scrollPos, viewRect);
		float num2 = 0f;
		Rect position5 = new Rect(0f, 0f, viewRect.width, 30f);
		Rect buyRect = new Rect(0f, position5.height, viewRect.width, 76f);
		if (list.Count > 0)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			position5.y = num2;
			GUI.Label(position5, buySectionTitle);
			num2 += position5.height;
			foreach (Tradeable item in list)
			{
				buyRect.y = num2;
				DrawTradeInterfaceFor(item, buyRect, BuySellMode.Buying);
				num2 += 86f;
			}
		}
		if (list2.Count > 0)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			position5.y = num2;
			GUI.Label(position5, sellSectionTitle);
			num2 += position5.height;
			foreach (Tradeable item2 in list2)
			{
				buyRect.y = num2;
				DrawTradeInterfaceFor(item2, buyRect, BuySellMode.Selling);
				num2 += 86f;
			}
		}
		GUI.EndScrollView();
	}

	private static void DrawTradeInterfaceFor(Tradeable trad, Rect buyRect, BuySellMode tradeMode)
	{
		GUI.DrawTexture(buyRect, PawnBuyInterfaceBGTex);
		Rect innerRect = buyRect.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(0f, 0f, 150f, 30f), trad.Label);
		GUI.Label(text: "$" + ((tradeMode != 0) ? trad.SellPrice : trad.BuyPrice), position: new Rect(0f, 30f, 150f, 30f));
		Rect position = new Rect(130f, 0f, innerRect.width - 10f - 130f - 10f - 90f - 10f - 90f - 10f, innerRect.height);
		GenUI.SetFontTiny();
		GUI.Label(position, trad.InfoStringShort);
		GenUI.SetFontSmall();
		Rect butRect = new Rect(innerRect.width - 180f - 10f, 0f, 90f, innerRect.height);
		if (UIWidgets.TextButton(butRect, "Info"))
		{
			Find.Dialogs.AddDialogBox(trad.NewInfoDialog);
		}
		Rect butRect2 = new Rect(innerRect.width - 90f, 0f, 90f, innerRect.height);
		if (tradeMode == BuySellMode.Buying && UIWidgets.TextButton(butRect2, "Buy"))
		{
			trad.TryBuy();
		}
		if (tradeMode == BuySellMode.Selling && UIWidgets.TextButton(butRect2, "Sell"))
		{
			trad.TrySell();
		}
		GUI.EndGroup();
	}
}
