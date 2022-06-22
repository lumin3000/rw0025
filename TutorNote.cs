using System;
using UnityEngine;

public class TutorNote : TutorItem, Saveable
{
	private const float DrawWidth = 600f;

	private const float DistFromScreenBottom = 90f;

	private const float DistFromScreenBottomArch = 260f;

	private const float FadeInDuration = 0.25f;

	private const float DoneButPad = 8f;

	protected string baseText;

	public Type nextItemType;

	protected string codexPath = string.Empty;

	public bool doFadeIn = true;

	private static readonly Texture2D WhiteTex = GenRender.SolidColorTexture(Color.white);

	private static readonly Vector2 DoneButSize = new Vector2(32f, 32f);

	private static readonly Vector2 CodexButSize = new Vector2(100f, 32f);

	public override bool Completed => false;

	private Vector2 CurTopLeft
	{
		get
		{
			float num = 90f;
			if (Find.UIMapRoot.modeControls.openTab == Find.UIMapRoot.modeControls.tabArchitect)
			{
				num = 260f;
			}
			return new Vector2((float)(Screen.width / 2) - 300f, (float)Screen.height - num - WindowHeight);
		}
	}

	private float WindowHeight
	{
		get
		{
			GUIStyle label = GUI.skin.label;
			GUIContent content = new GUIContent(baseText);
			Vector2 doneButSize = DoneButSize;
			float num = label.CalcHeight(content, 600f - doneButSize.x - 16f - 20f);
			num += 20f;
			Vector2 doneButSize2 = DoneButSize;
			float y = doneButSize2.y;
			Vector2 codexButSize = CodexButSize;
			float val = y + codexButSize.y + 24f;
			return Math.Max(num, val) + 15f;
		}
	}

	public void ExposeData()
	{
	}

	protected virtual string GetFullText()
	{
		return baseText;
	}

	public override void TutorItemOnGUI()
	{
		if (Time.timeSinceLevelLoad < 0.01f)
		{
			return;
		}
		Texture2D tex = ((nextItemType != null) ? UIWidgets.NextButTexBig : UIWidgets.CloseButTexBig);
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		Rect rect = new Rect(CurTopLeft.x, CurTopLeft.y, 600f, WindowHeight);
		UIWidgets.DrawWindow(rect);
		Rect innerRect = rect.GetInnerRect(10f);
		float width = innerRect.width;
		Vector2 codexButSize = CodexButSize;
		innerRect.width = width - (codexButSize.x + 16f);
		GUI.Label(innerRect, GetFullText());
		float num = rect.x + rect.width;
		Vector2 doneButSize = DoneButSize;
		float left = num - doneButSize.x - 8f;
		float top = rect.y + 8f;
		Vector2 doneButSize2 = DoneButSize;
		float x = doneButSize2.x;
		Vector2 doneButSize3 = DoneButSize;
		Rect butRect = new Rect(left, top, x, doneButSize3.y);
		if (UIWidgets.ImageButton(butRect, tex))
		{
			NoteClose();
		}
		if (codexPath != string.Empty)
		{
			float num2 = rect.x + rect.width;
			Vector2 codexButSize2 = CodexButSize;
			float left2 = num2 - codexButSize2.x - 8f;
			float num3 = rect.y + rect.height - 8f;
			Vector2 codexButSize3 = CodexButSize;
			float top2 = num3 - codexButSize3.y;
			Vector2 codexButSize4 = CodexButSize;
			float x2 = codexButSize4.x;
			Vector2 codexButSize5 = CodexButSize;
			Rect butRect2 = new Rect(left2, top2, x2, codexButSize5.y);
			if (UIWidgets.TextButton(butRect2, "Open Codex"))
			{
				Find.UIRoot.dialogs.AddDialogBox(new DialogBox_Codex(codexPath));
			}
		}
		if (doFadeIn)
		{
			float num4 = (Time.time - Find.Tutor.activeNoteStartRealTime) / 0.25f;
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			GUI.color = new Color(1f, 1f, 1f, 1f - num4);
			GUI.DrawTexture(rect, WhiteTex);
			GUI.color = Color.white;
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
	}

	private void NoteClose()
	{
		if (nextItemType != null)
		{
			TutorItem item = Find.Tutor.ItemOfType(nextItemType);
			Find.Tutor.StartShow(item);
		}
		else
		{
			GenSound.PlaySoundOnCamera(UISounds.Click, 0.1f);
			Find.Tutor.activeNote = null;
		}
	}
}
