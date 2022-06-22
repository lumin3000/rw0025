using System;
using UnityEngine;

public class GameControls_TabArchitect : UIPanel_Tab
{
	public const float WinHeight = 152f;

	public const float WinWidth = 200f;

	private UIPanel_Designation panelOrders = new UIPanel_Designation(DesignationListings.ListOrders());

	private UIPanel_Designation panelAreas = new UIPanel_Designation(DesignationListings.ListAreas());

	private UIPanel_Designation panelStructure = new UIPanel_Designation(DesignationListings.ListStructure());

	private UIPanel_Designation panelBuildings = new UIPanel_Designation(DesignationListings.ListBuildings());

	private UIPanel_Designation panelFurniture = new UIPanel_Designation(DesignationListings.ListFurniture());

	private UIPanel_Designation panelSecurity = new UIPanel_Designation(DesignationListings.ListSecurity());

	public UIPanel_Designation selectedPanel;

	public override void PanelUpdate()
	{
		if (selectedPanel != null)
		{
			selectedPanel.PanelUpdate();
		}
	}

	public override void PanelOnGUI(Rect fillRect)
	{
		Rect rect = new Rect(0f, (float)(Screen.height - 35) - 152f, 200f, 152f);
		UIWidgets.DrawWindow(rect);
		Rect position = new Rect(rect.GetInnerRect(8f));
		GUI.BeginGroup(position);
		float butHeight = (position.height - 16f) / 3f;
		float butWidth = (position.width - 8f) / 2f;
		float curY = 0f;
		float curX = 0f;
		Action<string, UIPanel_Designation> action = delegate(string label, UIPanel_Designation panel)
		{
			if (UIWidgets.TextButton(new Rect(curX, curY, butWidth, butHeight), label))
			{
				ClickedPanel(panel);
			}
		};
		action("Orders", panelOrders);
		curY += butHeight + 8f;
		action("Structure", panelStructure);
		curY += butHeight + 8f;
		action("Security", panelSecurity);
		curX += butWidth + 8f;
		curY = 0f;
		action("Areas", panelAreas);
		curY += butHeight + 8f;
		action("Buildings", panelBuildings);
		curY += butHeight + 8f;
		action("Furniture", panelFurniture);
		GUI.EndGroup();
		Rect innerRect = rect.GetInnerRect(-8f);
		GenUI.AbsorbClicksInRect(innerRect);
		if (selectedPanel != null)
		{
			selectedPanel.PanelOnGUI();
		}
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
		{
			Find.UIMapRoot.modeControls.clickPassThrough = true;
			Find.UIMapRoot.modeControls.EscapeCurrentTab();
		}
		if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape))
		{
			Find.UIMapRoot.modeControls.EscapeCurrentTab();
			Event.current.Use();
		}
	}

	protected void ClickedPanel(UIPanel_Designation Pan)
	{
		if (selectedPanel == Pan)
		{
			selectedPanel = null;
		}
		else
		{
			selectedPanel = Pan;
		}
		GenSound.PlaySoundOnCamera(UISounds.SubmenuSelect, 0.2f);
	}
}
