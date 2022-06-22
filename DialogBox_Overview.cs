using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogBox_Overview : DialogBox
{
	private const float TopAreaHeight = 60f;

	private const float TitleRectHeight = 40f;

	private const float SectionButtonSpacing = 6f;

	private List<UIPanel_Tab> overviewTabs = new List<UIPanel_Tab>();

	private UIPanel_Tab curTab;

	private static readonly Vector2 WinSize = new Vector2(900f, 750f);

	public DialogBox_Overview()
	{
		Vector2 winSize = WinSize;
		float x = winSize.x;
		Vector2 winSize2 = WinSize;
		SetWinCentered(x, winSize2.y);
		overviewTabs.Add(new Tab_Overview_Work());
		curTab = overviewTabs[0];
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		if (UIWidgets.CloseButtonFor(winRect))
		{
			Find.Dialogs.PopBox();
		}
		GenUI.SetFontSmall();
		Rect innerRect = winRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		Rect position = new Rect(0f, 0f, innerRect.width, 40f);
		GenUI.SetFontMedium();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(position, "Overview of " + Find.ColonyInfo.ColonyName);
		float num = 70f;
		Rect rect = new Rect(0f, num, innerRect.width, innerRect.height - num);
		UIWidgets.DrawMenuSection(rect);
		curTab.PanelOnGUI(rect);
		List<TabDef> tabsEnum = overviewTabs.Select((UIPanel_Tab panel) => new TabDef(panel.title, delegate
		{
			curTab = panel;
		}, panel == curTab)).ToList();
		UIWidgets.DrawTabs(rect, tabsEnum);
		GUI.EndGroup();
		DetectShouldClose(doButton: false);
		GenUI.AbsorbAllInput();
	}
}
