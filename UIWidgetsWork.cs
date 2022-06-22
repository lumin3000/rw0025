using System;
using System.Text;
using UnityEngine;

public static class UIWidgetsWork
{
	public const float WorkBoxSize = 25f;

	private const int MidAptCutoff = 7;

	private const int MaxAptitude = 10;

	private static readonly Texture2D WorkBoxBGTex_Bad = Res.LoadTexture("UI/Widgets/WorkBoxBG_Bad");

	private static readonly Texture2D WorkBoxBGTex_Mid = Res.LoadTexture("UI/Widgets/WorkBoxBG_Mid");

	private static readonly Texture2D WorkBoxBGTex_Excellent = Res.LoadTexture("UI/Widgets/WorkBoxBG_Excellent");

	private static readonly Texture2D WorkBoxCheckTex = Res.LoadTexture("UI/Widgets/WorkBoxCheck");

	private static Color ColorOfPriority(int prio)
	{
		return prio switch
		{
			1 => Color.green, 
			2 => new Color(1f, 0.9f, 0.6f), 
			3 => new Color(0.8f, 0.7f, 0.5f), 
			4 => new Color(0.6f, 0.6f, 0.6f), 
			_ => Color.grey, 
		};
	}

	public static void DrawWorkBoxFor(Vector2 topLeft, Pawn p, WorkType wType)
	{
		if (p.story.WorkIsDisabled(wType))
		{
			return;
		}
		Rect rect = new Rect(topLeft.x, topLeft.y, 25f, 25f);
		DrawAdjustedBackground(rect, p, wType);
		if (Find.PlaySettings.useWorkPriorities)
		{
			int priorityOf = p.WorkSettings.GetPriorityOf(wType);
			string text = ((priorityOf <= 0) ? string.Empty : priorityOf.ToString());
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.color = ColorOfPriority(priorityOf);
			Rect innerRect = rect.GetInnerRect(-3f);
			innerRect.y += 4f;
			innerRect.x -= 2f;
			GUI.Label(innerRect, text);
			GUI.color = Color.white;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			if (Event.current.type != 0 || !rect.Contains(Event.current.mousePosition))
			{
				return;
			}
			if (Event.current.button == 0)
			{
				int num = p.WorkSettings.GetPriorityOf(wType) - 1;
				if (num < 0)
				{
					num = 4;
				}
				p.WorkSettings.SetWorkToPriority(wType, num);
				GenSound.PlaySoundOnCamera(UISounds.TickHigh, 0.1f);
			}
			if (Event.current.button == 1)
			{
				int num2 = p.WorkSettings.GetPriorityOf(wType) + 1;
				if (num2 > 4)
				{
					num2 = 0;
				}
				p.WorkSettings.SetWorkToPriority(wType, num2);
				GenSound.PlaySoundOnCamera(UISounds.TickLow, 0.1f);
			}
			Event.current.Use();
			return;
		}
		int priorityOf2 = p.WorkSettings.GetPriorityOf(wType);
		if (priorityOf2 > 0)
		{
			GUI.DrawTexture(rect, WorkBoxCheckTex);
		}
		if (UIWidgets.InvisibleButton(rect))
		{
			if (p.WorkSettings.GetPriorityOf(wType) > 0)
			{
				p.WorkSettings.SetWorkToPriority(wType, 0);
				GenSound.PlaySoundOnCamera(UISounds.TickLow, 0.1f);
			}
			else
			{
				p.WorkSettings.SetWorkToPriority(wType, 4);
				GenSound.PlaySoundOnCamera(UISounds.TickHigh, 0.1f);
			}
		}
	}

	public static TooltipDef TipForPawnWorker(Pawn p, WorkType wType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		WorkDefinition definition = wType.GetDefinition();
		stringBuilder.AppendLine(definition.gerundLabel);
		if (p.story.WorkIsDisabled(wType))
		{
			stringBuilder.Append(p.characterName + " cannot do this kind of work.");
		}
		else
		{
			string text = string.Empty;
			if (definition.relevantSkills.Count == 0)
			{
				text = "(none)";
			}
			else
			{
				foreach (SkillType relevantSkill in definition.relevantSkills)
				{
					text = text + relevantSkill.GetDefinition().label + ", ";
				}
				text = text.Substring(0, text.Length - 2);
			}
			stringBuilder.AppendLine("Relevant skills: " + text);
			stringBuilder.AppendLine(p.characterName + "'s overall aptitude: " + AptitudeEstimateFor(p, wType).ToString() + "/" + 10);
			stringBuilder.AppendLine();
			stringBuilder.Append(wType.GetDefinition().tooltipDesc);
		}
		return new TooltipDef(stringBuilder.ToString());
	}

	private static void DrawAdjustedBackground(Rect boxRect, Pawn p, WorkType wType)
	{
		int num = AptitudeEstimateFor(p, wType);
		Texture2D image;
		Texture2D image2;
		float a;
		if (num <= 7)
		{
			image = WorkBoxBGTex_Bad;
			image2 = WorkBoxBGTex_Mid;
			a = (float)num / 7f;
		}
		else
		{
			image = WorkBoxBGTex_Mid;
			image2 = WorkBoxBGTex_Excellent;
			a = (float)(num - 7) / 3f;
		}
		GUI.DrawTexture(boxRect, image);
		GUI.color = new Color(1f, 1f, 1f, a);
		GUI.DrawTexture(boxRect, image2);
		GUI.color = Color.white;
	}

	private static int AptitudeEstimateFor(Pawn p, WorkType wType)
	{
		WorkDefinition definition = wType.GetDefinition();
		if (definition.relevantSkills.Count == 0)
		{
			return 3;
		}
		float num = 0f;
		foreach (SkillType relevantSkill in definition.relevantSkills)
		{
			num += (float)p.skills.LevelOf(relevantSkill);
		}
		num /= (float)definition.relevantSkills.Count;
		num *= 0.5f;
		return (int)Math.Round(num);
	}
}
