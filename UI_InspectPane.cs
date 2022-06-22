using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_InspectPane
{
	private class CommandGridEntry
	{
		public Thing thing;

		public Command opt;

		public CommandGridEntry(Thing thing, Command opt)
		{
			this.thing = thing;
			this.opt = opt;
		}
	}

	private const float PaneInnerMargin = 12f;

	protected const float TabWidth = 72f;

	public const float TabHeight = 30f;

	public ITab openTab;

	public static readonly Vector2 PaneSize = new Vector2(360f, 160f);

	public static readonly Vector2 PaneInnerSize = new Vector2(PaneSize.x - 24f, PaneSize.y - 24f);

	private IEnumerable<Thing> SelectedThings => Find.Selector.SelectedThings;

	private Thing SelThing => Find.Selector.SingleSelectedThing;

	private int NumSelected => Find.Selector.NumSelected;

	public float PaneTopY
	{
		get
		{
			float num = Screen.height;
			Vector2 paneSize = PaneSize;
			return num - paneSize.y - 35f;
		}
	}

	public void InspectPaneOnGUI()
	{
		float paneTopY = PaneTopY;
		Vector2 paneSize = PaneSize;
		float x = paneSize.x;
		Vector2 paneSize2 = PaneSize;
		Rect rect = new Rect(0f, paneTopY, x, paneSize2.y);
		if (NumSelected > 0)
		{
			UIWidgets.DrawWindow(rect);
			GUI.BeginGroup(rect.GetInnerRect(12f));
			bool flag = true;
			if (NumSelected > 1)
			{
				flag = !SelectedThings.Where((Thing t) => !InspectionUtility.CanInspectTogether(SelectedThings.First(), t)).Any();
			}
			InspectionUtility.DrawLabelFor(SelectedThings);
			if (flag && NumSelected == 1)
			{
				InspectionUtility.DrawHealthFor(SelThing);
				InspectionUtility.DrawInspectStringFor(SelThing);
			}
			InspectionUtility.DrawCommandGridFor(SelectedThings);
			GUI.EndGroup();
			if (NumSelected == 1)
			{
				float top = PaneTopY - 30f;
				float num = 0f;
				foreach (ITab inspectorTab in SelThing.def.inspectorTabs)
				{
					if (inspectorTab.IsVisible)
					{
						Rect butRect = new Rect(num, top, 72f, 30f);
						GenUI.SetFontSmall();
						if (UIWidgets.TextButton(butRect, inspectorTab.Label))
						{
							ToggleTab(inspectorTab);
						}
						if (inspectorTab == openTab)
						{
							inspectorTab.DoGUI();
						}
						num += 72f;
					}
				}
			}
		}
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && openTab != null)
		{
			CloseOpenTab();
			Event.current.Use();
		}
		GenUI.AbsorbClicksInRect(rect);
	}

	public void CloseOpenTab()
	{
		if (openTab != null)
		{
			ToggleTab(openTab);
		}
	}

	private void ToggleTab(ITab tab)
	{
		if (tab == openTab)
		{
			openTab = null;
			GenSound.PlaySoundOnCamera(UISounds.TabClose, 0.3f);
		}
		else
		{
			tab.Opening();
			openTab = tab;
			GenSound.PlaySoundOnCamera(UISounds.TabOpen, 0.3f);
		}
	}

	public void Reset()
	{
		openTab = null;
	}
}
