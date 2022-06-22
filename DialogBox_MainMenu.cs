using System.Collections.Generic;
using UnityEngine;

public class DialogBox_MainMenu : DialogBox
{
	private const int ButCount = 3;

	private const float GameRectWidth = 200f;

	private const float NewsRectWidth = 350f;

	private const float TitleShift = 50f;

	private const float ColumnLabelHeight = 40f;

	private static readonly Vector2 PaneSize = new Vector2(800f, 500f);

	protected readonly Vector2 ButSize = new Vector2(180f, 60f);

	private static readonly Texture2D TitleTex = Res.LoadTexture("UI/HeroArt/GameTitle");

	private static readonly Vector2 TitleSize = new Vector2(560f, 142f);

	private static readonly Texture2D ImageBlog = Res.LoadTexture("UI/HeroArt/WebIcons/Blog");

	private static readonly Texture2D ImageForums = Res.LoadTexture("UI/HeroArt/WebIcons/Forums");

	private static readonly Texture2D ImageTwitter = Res.LoadTexture("UI/HeroArt/WebIcons/Twitter");

	private static readonly Texture2D ImageBook = Res.LoadTexture("UI/HeroArt/WebIcons/Book");

	private static readonly Texture2D ImageBug = Res.LoadTexture("UI/HeroArt/WebIcons/ReportABug");

	public override void DoDialogBoxGUI()
	{
		float num = Screen.width / 2;
		Vector2 paneSize = PaneSize;
		float left = num - paneSize.x / 2f;
		float num2 = Screen.height / 2;
		Vector2 paneSize2 = PaneSize;
		float top = num2 - paneSize2.y / 2f;
		Vector2 paneSize3 = PaneSize;
		float x = paneSize3.x;
		Vector2 paneSize4 = PaneSize;
		Rect rect = new Rect(left, top, x, paneSize4.y);
		if (Game.GMode == GameMode.Menus)
		{
			rect.y += 50f;
		}
		else
		{
			UIWidgets.DrawWindow(rect);
		}
		if (Game.GMode == GameMode.Menus)
		{
			float num3 = Screen.width / 2;
			Vector2 titleSize = TitleSize;
			float left2 = num3 - titleSize.x / 2f;
			float y = rect.y;
			Vector2 titleSize2 = TitleSize;
			float top2 = y - titleSize2.y - 10f;
			Vector2 titleSize3 = TitleSize;
			float x2 = titleSize3.x;
			Vector2 titleSize4 = TitleSize;
			Rect position = new Rect(left2, top2, x2, titleSize4.y);
			GUI.DrawTexture(position, TitleTex, ScaleMode.StretchToFill, alphaBlend: true);
		}
		Rect innerRect = rect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		Rect rect2 = new Rect(0f, 40f, 200f, innerRect.height - 40f);
		Rect rect3 = new Rect(rect2.xMax + 17f, 40f, 350f, innerRect.height - 40f);
		Rect rect4 = new Rect(rect3.xMax + 17f, 40f, -1f, innerRect.height - 40f);
		rect4.xMax = innerRect.width;
		rect4.xMax += 50f;
		Rect position2 = new Rect(rect2);
		position2.y -= 40f;
		GenUI.SetFontMedium();
		GUI.Label(position2, "Play");
		GenUI.SetFontSmall();
		List<ListableOption> list = new List<ListableOption>();
		ListableOption item;
		if (Game.GMode == GameMode.Menus)
		{
			item = new ListableOption("New Colony", delegate
			{
				MapInitParams.Reset();
				Find.Dialogs.AddDialogBox(new DialogBox_StorytellerChooser());
			});
			list.Add(item);
		}
		if (Game.GMode == GameMode.Gameplay)
		{
			item = new ListableOption("Back to Game", delegate
			{
				Find.Dialogs.PopBox();
			});
			list.Add(item);
			item = new ListableOption("Save", delegate
			{
				Find.UIRoot.dialogs.AddDialogBox(new DialogBox_MapList_Save());
			});
			list.Add(item);
		}
		item = new ListableOption("Load", delegate
		{
			Find.UIRoot.dialogs.AddDialogBox(new DialogBox_MapList_Load());
		});
		list.Add(item);
		item = new ListableOption("Options", delegate
		{
			Find.UIRoot.dialogs.AddDialogBox(new DialogBox_Options());
		});
		list.Add(item);
		if (Game.GMode == GameMode.Gameplay)
		{
			item = new ListableOption("Quit to Main Menu", delegate
			{
				Application.LoadLevel("Entry");
			});
			list.Add(item);
		}
		item = new ListableOption("Quit to OS", delegate
		{
			Application.Quit();
		});
		list.Add(item);
		OptionListingUtility.DrawOptionListing(rect2.GetInnerRect(17f), list);
		Rect position3 = new Rect(rect3);
		position3.y -= 40f;
		GenUI.SetFontMedium();
		GUI.Label(position3, "News");
		GenUI.SetFontSmall();
		if (Game.GMode == GameMode.Menus)
		{
			GUI.DrawTexture(rect3, GenUI.DarkTransparentTex);
		}
		Rect innerRect2 = rect3.GetInnerRect(17f);
		string text = "This is a pre-alpha build of RimWorld.\n\nFuture versions will also include art and audio done by professionals (instead of by me), as well as far more content and fewer bugs.\n\nIf you have any thoughts on the game, please join us via the forum link on the right.\n\nCheers,\n\n    -Ty";
		GUI.Label(innerRect2, text);
		Rect position4 = new Rect(rect4);
		position4.y -= 40f;
		GenUI.SetFontMedium();
		GUI.Label(position4, "Web");
		GenUI.SetFontSmall();
		List<ListableOption> list2 = new List<ListableOption>();
		ListableOption item2 = new ListableOption_WebLink("Ludeon blog", "http://ludeon.com/blog", ImageBlog);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Forums", "http://ludeon.com/forums", ImageForums);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Official wiki", "http://ludeon.com/rimworld/wiki", ImageBlog);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Tynan's twitter", "https://twitter.com/TynanSylvester", ImageTwitter);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Tynan's design book", "http://tynansylvester.com/book", ImageBook);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Report a bug", "http://ludeon.com/bugs/", ImageBug);
		list2.Add(item2);
		item2 = new ListableOption_WebLink("Credits", delegate
		{
			Find.UIRoot.dialogs.AddDialogBox(new DialogBox_Credits());
		}, ImageBlog);
		list2.Add(item2);
		OptionListingUtility.DrawOptionListing(rect4.GetInnerRect(17f), list2);
		GUI.EndGroup();
		DetectShouldClose(doButton: false);
	}
}
