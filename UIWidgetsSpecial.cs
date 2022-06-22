using UnityEngine;

public static class UIWidgetsSpecial
{
	private const int IconSize = 22;

	private const int MouseoverContentOffset = 2;

	private static readonly Texture2D IconButBG = Res.LoadTexture("UI/Widgets/TabButBG");

	private static readonly Texture2D IconButBorder = Res.LoadTexture("UI/Widgets/TabButBorder");

	private static readonly Texture2D IconBarTex = Res.LoadTexture("UI/Widgets/TabBar");

	public static bool IconButton(Rect rect, string label, Texture2D icon)
	{
		return IconButton(rect, label, icon, 0f);
	}

	public static bool IconButton(Rect butRect, string label, Texture2D icon, float barPercent)
	{
		bool flag = false;
		if (butRect.Contains(Event.current.mousePosition))
		{
			flag = true;
			GUI.color = GenUI.MouseoverColor;
		}
		MouseoverSounds.DoRegion(butRect, MouseoverSoundType.Thump);
		UIWidgets.DrawShadowAround(butRect);
		UIWidgets.DrawAtlas(butRect, IconButBG);
		GUI.color = Color.white;
		if (barPercent > 0.001f)
		{
			UIWidgets.FillableBar(butRect, barPercent, IconBarTex, doBlackBorder: false, null);
		}
		UIWidgets.DrawAtlas(butRect, IconButBorder);
		float num = 0f;
		if (icon != null)
		{
			num = butRect.height / 2f - 11f;
			Rect position = new Rect(butRect.x + num, butRect.y + num, 22f, 22f);
			if (flag)
			{
				position.x += 2f;
				position.y -= 2f;
			}
			GUI.DrawTexture(position, icon);
		}
		Rect position2 = new Rect(butRect);
		position2.x += num * 2f + 22f;
		if (flag)
		{
			position2.x += 2f;
			position2.y -= 2f;
		}
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GenUI.SetFontSmall();
		GUI.Label(position2, label);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		return UIWidgets.InvisibleButton(butRect);
	}
}
