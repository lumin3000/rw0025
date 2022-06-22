using UnityEngine;

public class UI_EntryRoot : UI_Root
{
	public UI_MenuPage curPage { get; private set; }

	public UI_EntryRoot()
	{
		curPage = new UI_PageMainMenu();
		dialogs.AddDialogBox(new DialogBox_MainMenu());
	}

	public override void UIRootOnGUI()
	{
		if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
		{
			base.UIRootOnGUI();
			curPage.PageOnGUI();
		}
		else
		{
			curPage.PageOnGUI();
			base.UIRootOnGUI();
		}
	}

	public void SetPage(UI_MenuPage newPage)
	{
		GenSound.PlaySoundOnCamera(UISounds.PageChange, 0.25f);
		curPage = newPage;
	}
}
