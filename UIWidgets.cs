using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UIWidgets
{
	public const int CheckboxSize = 24;

	private const float RadioButtonSize = 32f;

	private const int FillableBarBorderWidth = 3;

	private const int MaxFillChangeArrowSize = 16;

	private const float CloseButtonSize = 18f;

	private const float CloseButtonMargin = 4f;

	private const float MaxTabWidth = 200f;

	private const float TabHeight = 32f;

	private const float TabEndWidth = 30f;

	private const float TabMiddleGraphicWidth = 4f;

	private const float TabHoriztonalOverlap = 10f;

	private static readonly Texture2D DefaultBarBgTex = GenUI.BlackTex;

	private static readonly Texture2D BarFullTexHor = GenRender.SolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

	public static readonly Texture2D CheckboxOnTex = Res.LoadTexture("UI/Widgets/Check");

	public static readonly Texture2D CheckboxOffTex = Res.LoadTexture("UI/Widgets/Uncheck");

	private static readonly Texture2D RadioButOnTex = Res.LoadTexture("UI/Widgets/RadioButOn");

	private static readonly Texture2D RadioButOffTex = Res.LoadTexture("UI/Widgets/RadioButOff");

	private static readonly Texture2D FillArrowTexRight = Res.LoadTexture("UI/Widgets/FillChangeArrowRight");

	private static readonly Texture2D FillArrowTexLeft = Res.LoadTexture("UI/Widgets/FillChangeArrowLeft");

	private static readonly Texture2D MenuBGAtlas = Res.LoadTexture("UI/Widgets/MenuBG");

	private static readonly Texture2D ShadowAtlas = Res.LoadTexture("UI/Widgets/DropShadow");

	private static readonly Texture2D SubmenuBGAtlas = Res.LoadTexture("UI/Widgets/MenuSectionBG");

	private static readonly Texture2D ButtonBGAtlas = Res.LoadTexture("UI/Widgets/ButtonBG");

	private static readonly Texture2D ButtonBGAtlasMouseover = Res.LoadTexture("UI/Widgets/ButtonBGMouseover");

	private static readonly Texture2D ButtonBGAtlasClick = Res.LoadTexture("UI/Widgets/ButtonBGClick");

	public static readonly Texture2D CloseButTexBig = Res.LoadTexture("UI/Widgets/CloseX");

	public static readonly Texture2D CloseButTexSmall = Res.LoadTexture("UI/Widgets/CloseXSmall");

	public static readonly Texture2D NextButTexBig = Res.LoadTexture("UI/Widgets/NextArrow");

	private static float FillableBarChangeRateDisplayRatio = 100000000f;

	public static int MaxFillableBarChangeRate = 3;

	private static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

	private static readonly Color MouseoverOptionColor = Color.yellow;

	private static readonly Texture2D TabAtlasTex = Res.LoadTexture("UI/Widgets/TabAtlas");

	public static void DrawTextureFitted(Rect outerRect, Texture2D tex, float scale)
	{
		DrawTextureFitted(outerRect, tex, scale, new Vector2(tex.width, tex.height));
	}

	public static void DrawTextureFitted(Rect outerRect, Texture2D tex, float scale, Vector2 texProportions)
	{
		Rect position = new Rect(0f, 0f, texProportions.x, texProportions.y);
		float num = ((!(position.width / position.height < outerRect.width / outerRect.height)) ? (outerRect.width / position.width) : (outerRect.height / position.height));
		num *= scale;
		position.width *= num;
		position.height *= num;
		position.x = outerRect.x + outerRect.width / 2f - position.width / 2f;
		position.y = outerRect.y + outerRect.height / 2f - position.height / 2f;
		GUI.DrawTexture(position, tex);
	}

	public static void LabelCheckbox(Rect rect, string label, ref bool checkboxValue)
	{
		GUI.Label(rect, label);
		Vector2 topLeft = new Vector2(rect.x + rect.width - 24f, rect.y);
		Checkbox(topLeft, ref checkboxValue);
	}

	public static void Checkbox(Vector2 topLeft, ref bool checkboxValue)
	{
		Texture2D tex = ((!checkboxValue) ? CheckboxOffTex : CheckboxOnTex);
		Rect rect = new Rect(topLeft.x, topLeft.y, 24f, 24f);
		MouseoverSounds.DoRegion(rect);
		if (ImageButton(rect, tex))
		{
			if (checkboxValue)
			{
				GenSound.PlaySoundOnCamera(UISounds.CheckboxTurnedOff, 0.1f);
			}
			else
			{
				GenSound.PlaySoundOnCamera(UISounds.CheckboxTurnedOn, 0.1f);
			}
			checkboxValue = !checkboxValue;
		}
	}

	public static bool LabelRadioButton(Rect screenRect, string labelText, bool chosen)
	{
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(screenRect, labelText);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		Vector2 topLeft = new Vector2(screenRect.x + screenRect.width - 32f, screenRect.y + screenRect.height / 2f - 16f);
		return RadioButton(topLeft, chosen);
	}

	public static bool RadioButton(Vector2 topLeft, bool chosen)
	{
		Texture2D tex = ((!chosen) ? RadioButOffTex : RadioButOnTex);
		Rect rect = new Rect(topLeft.x, topLeft.y, 24f, 24f);
		MouseoverSounds.DoRegion(rect);
		bool flag = ImageButton(rect, tex);
		if (flag && !chosen)
		{
			GenSound.PlaySoundOnCamera(UISounds.RadioButtonClicked, 0.1f);
		}
		return flag;
	}

	public static void FillableBar(Rect rect, float fillPercent)
	{
		FillableBar(rect, fillPercent, BarFullTexHor);
	}

	public static void FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
	{
		bool doBlackBorder = false;
		if (rect.height > 15f && rect.width > 20f)
		{
			doBlackBorder = true;
		}
		FillableBar(rect, fillPercent, fillTex, doBlackBorder, DefaultBarBgTex);
	}

	public static void FillableBar(Rect screenRect, float fillPercent, Texture2D fillTex, bool doBlackBorder, Texture2D innerBGTex)
	{
		if (doBlackBorder)
		{
			GUI.DrawTexture(screenRect, GenUI.BlackTex);
			screenRect = screenRect.GetInnerRect(3f);
		}
		if (innerBGTex != null)
		{
			GUI.DrawTexture(screenRect, innerBGTex);
		}
		screenRect.width *= fillPercent;
		GUI.DrawTexture(screenRect, fillTex);
	}

	public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
	{
		if (fillPercent < 0f)
		{
			fillPercent = 0f;
		}
		if (fillPercent > 1f)
		{
			fillPercent = 1f;
		}
		Rect position = rect;
		position.width = labelWidth;
		GUI.Label(position, label);
		Rect rect2 = rect;
		rect2.x += labelWidth;
		rect2.width -= labelWidth;
		FillableBar(rect2, fillPercent);
	}

	public static void FillableBarChangeArrows(Rect barRect, float changeRate)
	{
		int changeRate2 = (int)(changeRate * FillableBarChangeRateDisplayRatio);
		FillableBarChangeArrows(barRect, changeRate2);
	}

	public static void FillableBarChangeArrows(Rect barRect, int changeRate)
	{
		if (changeRate != 0)
		{
			if (changeRate > MaxFillableBarChangeRate)
			{
				changeRate = MaxFillableBarChangeRate;
			}
			if (changeRate < -MaxFillableBarChangeRate)
			{
				changeRate = -MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Math.Abs(changeRate);
			float top = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = num;
				image = FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - num - 2f;
				num4 = 0f - num;
				image = FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				Rect position = new Rect(num3, top, num, num);
				GUI.DrawTexture(position, image);
				num3 += num4;
			}
		}
	}

	public static bool CloseButtonFor(Rect rectToClose)
	{
		Rect butRect = new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f);
		return ImageButton(butRect, CloseButTexSmall);
	}

	public static bool InvisibleButton(Rect ButRect)
	{
		GUIStyle style = new GUIStyle();
		return GUI.Button(ButRect, string.Empty, style);
	}

	public static bool TextButton(Rect butRect, string Label)
	{
		return TextButton(butRect, Label, drawBackground: true, doMouseoverSound: false, NormalOptionColor);
	}

	public static bool TextButton(Rect butRect, string Label, bool doMouseoverSound)
	{
		return TextButton(butRect, Label, drawBackground: true, doMouseoverSound, NormalOptionColor);
	}

	public static bool TextButtonNaked(Rect butRect, string Label)
	{
		return TextButton(butRect, Label, drawBackground: false, doMouseoverSound: false, NormalOptionColor);
	}

	public static bool TextButton(Rect rect, string label, bool drawBackground, bool doMouseoverSound)
	{
		return TextButton(rect, label, drawBackground, doMouseoverSound, NormalOptionColor);
	}

	public static bool TextButton(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color optionColor)
	{
		if (drawBackground)
		{
			Texture2D atlas = ButtonBGAtlas;
			if (rect.Contains(Event.current.mousePosition))
			{
				atlas = ButtonBGAtlasMouseover;
				if (Input.GetMouseButton(0))
				{
					atlas = ButtonBGAtlasClick;
				}
			}
			DrawAtlas(rect, atlas);
		}
		if (doMouseoverSound)
		{
			MouseoverSounds.DoRegion(rect);
		}
		if (!drawBackground)
		{
			GUI.color = optionColor;
			if (rect.Contains(Event.current.mousePosition))
			{
				GUI.color = MouseoverOptionColor;
			}
		}
		if (drawBackground)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		}
		else
		{
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
		GUI.Label(rect, label);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		return InvisibleButton(rect);
	}

	public static bool ImageButton(Rect ButRect, Texture2D Tex)
	{
		if (ButRect.Contains(Event.current.mousePosition))
		{
			GUI.color = GenUI.MouseoverColor;
		}
		GUI.DrawTexture(ButRect, Tex);
		GUI.color = Color.white;
		return InvisibleButton(ButRect);
	}

	public static void DrawHighlight(Rect rect)
	{
		GUI.DrawTexture(rect, GenUI.HighlightTex);
	}

	public static void DrawWindow(Rect rect)
	{
		TooltipHandler.ClearTooltipsFrom(rect);
		DrawShadowAround(rect);
		DrawAtlas(rect, MenuBGAtlas);
	}

	public static void DrawShadowAround(Rect rect)
	{
		Rect innerRect = rect.GetInnerRect(-9f);
		innerRect.x += 2f;
		innerRect.y += 2f;
		DrawAtlas(innerRect, ShadowAtlas);
	}

	public static void DrawMenuSection(Rect rect)
	{
		DrawMenuSection(rect, drawTop: true);
	}

	public static void DrawMenuSection(Rect rect, bool drawTop)
	{
		DrawAtlas(rect, SubmenuBGAtlas, drawTop);
	}

	public static void DrawAtlas(Rect rect, Texture2D atlas)
	{
		DrawAtlas(rect, atlas, drawTop: true);
	}

	public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
	{
		float num = (float)atlas.width * 0.25f;
		GUI.BeginGroup(rect);
		Rect texRect;
		Rect drawRect;
		if (drawTop)
		{
			drawRect = new Rect(0f, 0f, num, num);
			texRect = new Rect(0f, 0f, 0.25f, 0.25f);
			DrawTexturePart(drawRect, texRect, atlas);
			drawRect = new Rect(rect.width - num, 0f, num, num);
			texRect = new Rect(0.75f, 0f, 0.25f, 0.25f);
			DrawTexturePart(drawRect, texRect, atlas);
		}
		drawRect = new Rect(0f, rect.height - num, num, num);
		texRect = new Rect(0f, 0.75f, 0.25f, 0.25f);
		DrawTexturePart(drawRect, texRect, atlas);
		drawRect = new Rect(rect.width - num, rect.height - num, num, num);
		texRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
		DrawTexturePart(drawRect, texRect, atlas);
		drawRect = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
		if (!drawTop)
		{
			drawRect.height += num;
			drawRect.y -= num;
		}
		DrawTexturePart(texRect: new Rect(0.25f, 0.25f, 0.5f, 0.5f), drawRect: drawRect, Tex: atlas);
		if (drawTop)
		{
			drawRect = new Rect(num, 0f, rect.width - num * 2f, num);
			texRect = new Rect(0.25f, 0f, 0.5f, 0.25f);
			DrawTexturePart(drawRect, texRect, atlas);
		}
		drawRect = new Rect(num, rect.height - num, rect.width - num * 2f, num);
		texRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
		DrawTexturePart(drawRect, texRect, atlas);
		drawRect = new Rect(0f, num, num, rect.height - num * 2f);
		if (!drawTop)
		{
			drawRect.height += num;
			drawRect.y -= num;
		}
		DrawTexturePart(texRect: new Rect(0f, 0.25f, 0.25f, 0.5f), drawRect: drawRect, Tex: atlas);
		drawRect = new Rect(rect.width - num, num, num, rect.height - num * 2f);
		if (!drawTop)
		{
			drawRect.height += num;
			drawRect.y -= num;
		}
		DrawTexturePart(texRect: new Rect(0.75f, 0.25f, 0.25f, 0.5f), drawRect: drawRect, Tex: atlas);
		GUI.EndGroup();
	}

	public static Rect ToUVRect(this Rect r, Vector2 texSize)
	{
		return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
	}

	public static void DrawTexturePart(Rect drawRect, Rect texRect, Texture2D Tex)
	{
		GUI.BeginGroup(drawRect);
		Rect position = new Rect((0f - texRect.x) * (drawRect.width / texRect.width), (0f - texRect.y) * (drawRect.height / texRect.height), drawRect.width / texRect.width, drawRect.height / texRect.height);
		GUI.DrawTexture(position, Tex);
		GUI.EndGroup();
	}

	public static TabDef DrawTabs(Rect baseRect, IEnumerable<TabDef> tabsEnum)
	{
		List<TabDef> tabList = tabsEnum.ToList();
		TabDef tabDef = null;
		TabDef tabDef2 = tabList.Where((TabDef t) => t.selected).FirstOrDefault();
		if (tabDef2 == null)
		{
			Debug.LogWarning("Drew tabs without any being selected.");
			return tabList[0];
		}
		float num = baseRect.width + (float)(tabList.Count - 1) * 10f;
		float tabWidth = num / (float)tabList.Count;
		if (tabWidth > 200f)
		{
			tabWidth = 200f;
		}
		Rect position = new Rect(baseRect);
		position.y -= 32f;
		position.height = 9999f;
		GUI.BeginGroup(position);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GenUI.SetFontSmall();
		Func<TabDef, Rect> func = delegate(TabDef tab)
		{
			int num2 = tabList.IndexOf(tab);
			float left = (float)num2 * (tabWidth - 10f);
			return new Rect(left, 1f, tabWidth, 32f);
		};
		List<TabDef> list = tabList.ListFullCopy();
		list.Remove(tabDef2);
		list.Add(tabDef2);
		TabDef tabDef3 = null;
		List<TabDef> list2 = list.ListFullCopy();
		list2.Reverse();
		foreach (TabDef item in list2)
		{
			Rect rect = func(item);
			if (tabDef3 == null && rect.Contains(Event.current.mousePosition))
			{
				tabDef3 = item;
			}
			MouseoverSounds.DoRegion(rect);
			if (InvisibleButton(rect))
			{
				tabDef = item;
			}
		}
		foreach (TabDef item2 in list)
		{
			Rect rect2 = func(item2);
			Rect drawRect = new Rect(rect2);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect2);
			drawRect2.width = 30f;
			drawRect2.x = rect2.x + rect2.width - 30f;
			Rect texRect = new Rect(17f / 32f, 0f, 15f / 32f, 1f);
			Rect drawRect3 = new Rect(rect2);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			Rect texRect2 = new Rect(30f, 0f, 4f, TabAtlasTex.height).ToUVRect(new Vector2(TabAtlasTex.width, TabAtlasTex.height));
			DrawTexturePart(drawRect, new Rect(0f, 0f, 15f / 32f, 1f), TabAtlasTex);
			DrawTexturePart(drawRect3, texRect2, TabAtlasTex);
			DrawTexturePart(drawRect2, texRect, TabAtlasTex);
			Rect position2 = rect2;
			if (tabDef3 == item2)
			{
				GUI.color = Color.yellow;
				position2.x += 2f;
				position2.y -= 2f;
			}
			GUI.Label(position2, item2.label);
			GUI.color = Color.white;
			if (item2 != tabDef2)
			{
				Rect drawRect4 = new Rect(rect2);
				drawRect4.y += rect2.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				DrawTexturePart(texRect: new Rect(0.5f, 0.01f, 0.01f, 0.01f), drawRect: drawRect4, Tex: TabAtlasTex);
			}
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.EndGroup();
		if (tabDef != null)
		{
			GenSound.PlaySoundOnCamera(UISounds.SubmenuSelect, 0.1f);
			if (tabDef.clickedAction != null)
			{
				tabDef.clickedAction();
			}
		}
		return tabDef;
	}
}
