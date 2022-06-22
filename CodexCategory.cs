using System.Collections.Generic;
using UnityEngine;

public class CodexCategory
{
	private const float TitleHeight = 35f;

	private const float CategoryHeight = 28f;

	private const float CategoryIndent = 8f;

	public string title;

	public List<CodexArticle> articleList = new List<CodexArticle>();

	public bool isOpen;

	private float lastCalculatedHeight;

	public CodexCategory(string title)
	{
		this.title = title;
	}

	public float TotalHeight(float width)
	{
		if (Event.current.type == EventType.Layout)
		{
			lastCalculatedHeight = DrawOnGUI(width);
		}
		return lastCalculatedHeight;
	}

	public float DrawOnGUI(float width)
	{
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		float num = 0f;
		Rect butRect = new Rect(0f, 0f, width, 35f);
		if (UIWidgets.TextButtonNaked(butRect, title))
		{
			if (!isOpen)
			{
				GenSound.PlaySoundOnCamera(UISounds.TickHigh, 0.1f);
			}
			else
			{
				GenSound.PlaySoundOnCamera(UISounds.TickLow, 0.1f);
			}
			isOpen = !isOpen;
		}
		num += butRect.height;
		if (isOpen)
		{
			foreach (CodexArticle article in articleList)
			{
				Rect butRect2 = new Rect(8f, num, width - 8f, 28f);
				if (UIWidgets.TextButtonNaked(butRect2, article.title))
				{
					GenSound.PlaySoundOnCamera(UISounds.TickTiny, 0.1f);
					CodexDatabase.curArticle = article;
				}
				num += 28f;
			}
			return num;
		}
		return num;
	}
}
