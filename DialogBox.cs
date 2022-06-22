using UnityEngine;

public abstract class DialogBox
{
	public bool clearDialogStack = true;

	protected Rect winRect = default(Rect);

	private readonly Vector2 DoneButSize = new Vector2(120f, 40f);

	private Texture2D ScreenGreyTex = GenRender.SolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 0.6f));

	public DialogBox()
	{
		GenSound.PlaySoundOnCamera("Interface/FlickWhoosh", 0.5f);
		if (Game.GMode == GameMode.Gameplay)
		{
			if (Find.FloatMenu.active)
			{
				Find.FloatMenu.Close();
			}
			UIPanel selectedPanel = Find.UIMapRoot.modeControls.tabArchitect.selectedPanel;
			if (selectedPanel != null)
			{
				((UIPanel_Designation)selectedPanel).dragger.CancelDrag();
			}
		}
	}

	public abstract void DoDialogBoxGUI();

	public virtual void PreClose()
	{
		GenSound.PlaySoundOnCamera(UISounds.Click, 0.1f);
	}

	protected void SetWinCentered(Vector2 screenSize)
	{
		SetWinCentered(screenSize.x, screenSize.y);
	}

	protected void SetWinCentered(float width, float height)
	{
		winRect = new Rect((float)(Screen.width / 2) - width / 2f, (float)(Screen.height / 2) - height / 2f, width, height);
	}

	protected bool DetectShouldClose(bool doButton)
	{
		bool flag = false;
		if (doButton)
		{
			GenUI.SetFontSmall();
			float num = winRect.x + winRect.width / 2f;
			Vector2 doneButSize = DoneButSize;
			float left = num - doneButSize.x / 2f;
			float top = winRect.y + winRect.height - 55f;
			Vector2 doneButSize2 = DoneButSize;
			float x = doneButSize2.x;
			Vector2 doneButSize3 = DoneButSize;
			if (UIWidgets.TextButton(new Rect(left, top, x, doneButSize3.y), "Done"))
			{
				flag = true;
				Event.current.Use();
			}
		}
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.Return))
		{
			flag = true;
			Event.current.Use();
		}
		if (flag)
		{
			Find.Dialogs.PopBox();
			if (Game.GMode == GameMode.Menus && Find.Dialogs.TopDialog == null)
			{
				Find.Dialogs.AddDialogBox(new DialogBox_MainMenu());
			}
		}
		return flag;
	}
}
