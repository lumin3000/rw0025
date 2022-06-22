using System.Linq;
using UnityEngine;

public class DialogBox_Codex : DialogBox
{
	private const float TabAreaHeight = 65f;

	private const float SectionButtonSpacing = 6f;

	private const float LeftAreaWidth = 220f;

	private const float ContentSpacing = 6f;

	private const float RightRectTopMargin = 40f;

	private Vector2 listingScrollPosition = Vector2.zero;

	private Vector2 articleScrollPosition = Vector2.zero;

	private static readonly Vector2 WinSize = new Vector2(800f, 750f);

	private static CodexSection CurSection
	{
		get
		{
			return CodexDatabase.curSection;
		}
		set
		{
			CodexDatabase.curSection = value;
		}
	}

	private static CodexArticle CurArticle
	{
		get
		{
			return CodexDatabase.curArticle;
		}
		set
		{
			CodexDatabase.curArticle = value;
		}
	}

	public DialogBox_Codex()
	{
		Vector2 winSize = WinSize;
		float x = winSize.x;
		Vector2 winSize2 = WinSize;
		SetWinCentered(x, winSize2.y);
	}

	public DialogBox_Codex(string codexPath)
		: this()
	{
		CodexDatabase.OpenPath(codexPath);
	}

	public override void DoDialogBoxGUI()
	{
		GenUI.SetFontSmall();
		UIWidgets.DrawWindow(winRect);
		if (UIWidgets.CloseButtonFor(winRect))
		{
			Find.Dialogs.PopBox();
		}
		Rect innerRect = winRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontMedium();
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(0f, 0f, innerRect.width, 300f), "Codex");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GenUI.SetFontSmall();
		Rect rect = new Rect(0f, 65f, innerRect.width, innerRect.height - 65f);
		UIWidgets.DrawMenuSection(rect);
		UIWidgets.DrawTabs(rect, CodexDatabase.sectionList.Select((CodexSection sect) => new TabDef(sect.title, delegate
		{
			CurSection = sect;
		}, CurSection == sect)));
		Rect innerRect2 = rect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect2);
		Rect inRect = new Rect(0f, 0f, 220f, innerRect2.height);
		Rect inRect2 = new Rect(inRect.width + 17f, 40f, innerRect2.width - inRect.width - 17f, innerRect2.height - 40f);
		FillCodexListing(inRect);
		FillCurArticleContent(inRect2);
		GUI.EndGroup();
		GUI.EndGroup();
		DetectShouldClose(doButton: false);
		GenUI.AbsorbClicksInRect(winRect);
	}

	private void FillCodexListing(Rect inRect)
	{
		float contentWidth = inRect.width - 24f;
		float height = CurSection.categoryList.Sum((CodexCategory c) => c.TotalHeight(contentWidth) + 6f);
		listingScrollPosition = GUI.BeginScrollView(inRect, listingScrollPosition, new Rect(0f, 0f, contentWidth, height));
		float num = 0f;
		foreach (CodexCategory category in CurSection.categoryList)
		{
			Rect position = new Rect(0f, num, inRect.width, 999f);
			GUI.BeginGroup(position);
			num += category.DrawOnGUI(contentWidth);
			GUI.EndGroup();
			num += 6f;
		}
		GUI.EndScrollView();
	}

	private void FillCurArticleContent(Rect inRect)
	{
		float contentWidth = inRect.width - 24f;
		float height = CurArticle.contentList.Sum((CodexContent c) => c.TotalHeight(contentWidth) + 6f);
		articleScrollPosition = GUI.BeginScrollView(inRect, articleScrollPosition, new Rect(0f, 0f, contentWidth, height));
		float num = 0f;
		foreach (CodexContent content in CurArticle.contentList)
		{
			Rect position = new Rect(0f, num, inRect.width, 999f);
			GUI.BeginGroup(position);
			num += content.DrawOnGUI(inRect.width);
			GUI.EndGroup();
			num += 6f;
		}
		GUI.EndScrollView();
	}
}
