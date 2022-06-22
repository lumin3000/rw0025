using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class PawnCardUtility
{
	private const int MainRectsY = 100;

	private const float MainRectsHeight = 450f;

	private const int ConfigRectTitlesHeight = 40;

	public static Vector2 PawnCardSize = new Vector2(570f, 470f);

	public static void DrawPawnCard(Pawn pawn)
	{
		DrawPawnCard(pawn, null);
	}

	public static void DrawPawnCard(Pawn curPawn, Action randomizeCallback)
	{
		bool flag = randomizeCallback != null;
		Rect position = new Rect(0f, 0f, 175f, 30f);
		if (flag)
		{
			GUI.skin.settings.doubleClickSelectsWord = true;
			GUI.SetNextControlName("NameField");
			GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
			GUI.skin.textField.contentOffset = new Vector2(12f, 0f);
			string text = GUI.TextField(position, curPawn.characterName);
			Regex regex = new Regex("^[a-zA-Z '\\-]*$");
			if (text.Length <= 12 && regex.IsMatch(text))
			{
				curPawn.characterName = text;
			}
		}
		else
		{
			GUI.Label(position, curPawn.characterName);
		}
		if (randomizeCallback != null)
		{
			Rect butRect = new Rect(300f, 0f, 175f, 40f);
			if (UIWidgets.TextButton(butRect, "Randomize"))
			{
				randomizeCallback();
			}
		}
		Rect position2 = new Rect(0f, 45f, 300f, 30f);
		GUI.Label(position2, curPawn.gender.ToString() + " " + curPawn.raceDef.raceName + " " + curPawn.KindLabel + ", age " + curPawn.age);
		Rect baseRect = new Rect(0f, 100f, 260f, 450f);
		Rect baseRect2 = new Rect(baseRect.xMax + 17f, 100f, 280f, 450f);
		Rect innerRect = baseRect.GetInnerRect(10f);
		Rect innerRect2 = baseRect2.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		float num = 0f;
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, 0f, 200f, 30f), "Backstory");
		num += 30f;
		GenUI.SetFontSmall();
		foreach (int value in Enum.GetValues(typeof(CharHistorySlot)))
		{
			Rect rect = new Rect(0f, num, innerRect.width, 24f);
			if (rect.Contains(Event.current.mousePosition))
			{
				UIWidgets.DrawHighlight(rect);
			}
			TooltipHandler.TipRegion(rect, curPawn.story.GetItemInSlot((CharHistorySlot)value).FullDescriptionFor(curPawn));
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(rect, ((CharHistorySlot)value).ToString() + ":");
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			Rect position3 = new Rect(rect);
			position3.x += 90f;
			position3.width -= 90f;
			string title = curPawn.story.GetItemInSlot((CharHistorySlot)value).title;
			GUI.Label(position3, title);
			num += rect.height + 2f;
		}
		num += 25f;
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, num, 200f, 30f), "Incapable of");
		num += 30f;
		GenUI.SetFontSmall();
		StringBuilder stringBuilder = new StringBuilder();
		List<WorkTags> list = curPawn.story.DisabledWorkTags.ToList();
		if (list.Count == 0)
		{
			stringBuilder.Append("(none), ");
		}
		else
		{
			foreach (WorkTags item in list)
			{
				stringBuilder.Append(item.ToString());
				stringBuilder.Append(", ");
			}
		}
		string text2 = stringBuilder.ToString();
		text2 = text2.Substring(0, text2.Length - 2);
		Rect position4 = new Rect(0f, num, innerRect.width, 999f);
		GUI.Label(position4, text2);
		num += 100f;
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, num, 200f, 30f), "Traits");
		num += 30f;
		GenUI.SetFontSmall();
		foreach (Trait trait in curPawn.traits.traitList)
		{
			Rect rect2 = new Rect(0f, num, innerRect.width, 24f);
			if (rect2.Contains(Event.current.mousePosition))
			{
				UIWidgets.DrawHighlight(rect2);
			}
			GUI.Label(rect2, trait.def.label);
			num += rect2.height + 2f;
			TooltipHandler.TipRegion(rect2, "Traits are just fun story bits for now; most of them don't do anything. This will change in future versions.");
		}
		GUI.EndGroup();
		GUI.BeginGroup(innerRect2);
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, 0f, 200f, 30f), "Skills");
		SkillUI.DrawSkillsOf(DrawMode: (Game.GMode != GameMode.Gameplay) ? SkillUI.SkillDrawMode.Menu : SkillUI.SkillDrawMode.Gameplay, p: curPawn, Offset: new Vector2(0f, 35f));
		GUI.EndGroup();
	}
}
