using System;
using UnityEngine;

public static class EntryDialogUtility
{
	public const float BottomAreaHeight = 65f;

	private static readonly Vector2 BottomButSize = new Vector2(150f, 48f);

	public static void DoNextBackButtons(Rect mainRect, string nextLabel, Action nextAct, Action backAct)
	{
		Rect innerRect = mainRect.GetInnerRect(17f);
		GenUI.SetFontSmall();
		float top = innerRect.height - 65f + 17f;
		Vector2 bottomButSize = BottomButSize;
		float x = bottomButSize.x;
		Vector2 bottomButSize2 = BottomButSize;
		Rect butRect = new Rect(0f, top, x, bottomButSize2.y);
		if (UIWidgets.TextButton(butRect, "Back"))
		{
			backAct();
		}
		float width = innerRect.width;
		Vector2 bottomButSize3 = BottomButSize;
		float left = width - bottomButSize3.x;
		Vector2 bottomButSize4 = BottomButSize;
		float x2 = bottomButSize4.x;
		Vector2 bottomButSize5 = BottomButSize;
		Rect butRect2 = new Rect(left, top, x2, bottomButSize5.y);
		if (UIWidgets.TextButton(butRect2, nextLabel))
		{
			nextAct();
		}
	}
}
