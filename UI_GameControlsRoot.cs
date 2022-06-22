using UnityEngine;

public class UI_GameControlsRoot
{
	protected const int TabWidth = 200;

	public const int TabHeight = 35;

	public const int TabSpacing = 0;

	public GameControls_TabInspect tabInspect = new GameControls_TabInspect();

	public GameControls_TabArchitect tabArchitect = new GameControls_TabArchitect();

	public UIPanel_Tab openTab;

	protected int tabButsDrawnThisFrame;

	protected UIPanel_Tab lastFrameTabButtonMouseOver;

	public bool clickPassThrough;

	protected readonly Texture2D TabIcon_Inspect = Res.LoadTexture("UI/Widgets/TabIcon_Inspect");

	protected readonly Texture2D TabIcon_Architect = Res.LoadTexture("UI/Widgets/TabIcon_Architect");

	public UI_GameControlsRoot()
	{
		openTab = tabInspect;
	}

	public void GameControlsOnGUI()
	{
		tabButsDrawnThisFrame = 0;
		GenUI.SetFontSmall();
		Rect rect = new Rect(0f, Screen.height - 35, 200f, 35f);
		if (UIWidgetsSpecial.IconButton(rect, "Architect", TabIcon_Architect))
		{
			ToggleTab(tabArchitect);
		}
		rect.x += rect.width;
		bool flag = false;
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
		{
			flag = true;
		}
		if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.MouseDown && Event.current.button == 1)
		{
			flag = true;
		}
		if (flag && openTab != tabArchitect)
		{
			tabInspect.selector.ClearSelection();
			ToggleTab(tabArchitect);
			Event.current.Use();
		}
		openTab.PanelOnGUI();
		if (clickPassThrough)
		{
			openTab.PanelOnGUI();
			clickPassThrough = false;
		}
	}

	public void TabsUpdate()
	{
		openTab.PanelUpdate();
	}

	public void EscapeCurrentTab()
	{
		if (openTab != tabInspect)
		{
			ToggleTab(tabInspect);
		}
	}

	public void ToggleTab(UIPanel_Tab ClickedTab)
	{
		if (openTab == ClickedTab)
		{
			if (openTab != tabInspect)
			{
				GenSound.PlaySoundOnCamera(UISounds.TabClose, 0.2f);
				openTab.PanelClosing();
				openTab = tabInspect;
			}
		}
		else
		{
			openTab = ClickedTab;
			openTab.PanelOpened();
			GenSound.PlaySoundOnCamera(UISounds.TabOpen, 0.2f);
		}
	}

	public bool PausedTab()
	{
		return false;
	}
}
