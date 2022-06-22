using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogBox_Trade : DialogBox
{
	private const float Pad = 12f;

	private const float TopAreaHeight = 100f;

	private const float TitleAreaHeight = 45f;

	private const float TabWidth = 100f;

	private const float TabAreaHeight = 40f;

	private const float TabSpacing = 10f;

	private const float BottomAreaHeight = 60f;

	private TradePanel currentPanel;

	public TradePanel_Commodities panelCommodities = new TradePanel_Commodities();

	public TradePanel_People panelPeople = new TradePanel_People();

	public TradePanel_Weapons panelWeapons = new TradePanel_Weapons();

	private static readonly Vector2 MoneyLabelLoc = new Vector2(30f, 40f);

	private IEnumerable<TradePanel> AllTabs
	{
		get
		{
			yield return panelCommodities;
			yield return panelPeople;
			yield return panelWeapons;
		}
	}

	public DialogBox_Trade(Trader TargetTrader)
	{
		Find.VisitorManager.ActiveTrader = TargetTrader;
		currentPanel = panelCommodities;
		SetWinCentered(800f, 800f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		Rect position = new Rect(0f, 0f, innerRect.width, 100f);
		GUI.BeginGroup(position);
		Rect position2 = new Rect(0f, 0f, position.width, 45f);
		GenUI.SetFontMedium();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(position2, Find.ActiveTrader.FullTitle);
		Vector2 moneyLabelLoc = MoneyLabelLoc;
		float x = moneyLabelLoc.x;
		Vector2 moneyLabelLoc2 = MoneyLabelLoc;
		Rect position3 = new Rect(x, moneyLabelLoc2.y, 999f, 999f);
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(position3, "Your money: $" + Find.ResourceManager.Money);
		GUI.EndGroup();
		Rect rect = new Rect(0f, 100f, innerRect.width, innerRect.height - 100f - 60f);
		UIWidgets.DrawMenuSection(rect);
		UIWidgets.DrawTabs(rect, AllTabs.Select((TradePanel tab) => new TabDef(tab.title, delegate
		{
			SelectPanel(tab);
		}, currentPanel == tab)));
		Rect innerRect2 = rect.GetInnerRect(12f);
		GUI.BeginGroup(innerRect2);
		Vector2 fillSize = new Vector2(innerRect2.width, innerRect2.height);
		currentPanel.PanelOnGUITrade(fillSize);
		GUI.EndGroup();
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbClicksInRect(winRect);
	}

	public override void PreClose()
	{
		base.PreClose();
		Find.ActiveTrader.CommsClosing();
	}

	private void SelectPanel(TradePanel newPanel)
	{
		if (currentPanel != newPanel)
		{
			currentPanel = newPanel;
			GenSound.PlaySoundOnCamera(UISounds.PageChange, 0.1f);
		}
	}
}
