using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InspectionUtility
{
	private const float LabelY = 0f;

	private const float HealthY = 33f;

	private const float InspectStringY = 53f;

	private static readonly Vector2 CommandGridRoot = new Vector2(204f, 30f);

	public static bool CanInspectTogether(Thing A, Thing B)
	{
		if (A.def.eType == EntityType.Pawn)
		{
			return false;
		}
		return A.def == B.def;
	}

	public static void DrawLabelFor(IEnumerable<Thing> selThingsEnum)
	{
		List<Thing> list = selThingsEnum.ToList();
		string text;
		if (list.Count == 1)
		{
			text = list[0].Label;
		}
		else
		{
			IEnumerable<IGrouping<string, Thing>> source = from th in list
				group th by th.Label.Split(' ')[0] into g
				select (g);
			text = ((source.Count() <= 1) ? (list[0].Label.Split(' ')[0] + " x" + list.Count) : "(various)");
		}
		GenUI.SetFontMedium();
		Rect position = new Rect(0f, 0f, 999f, 100f);
		GUI.Label(position, text);
	}

	public static void DrawHealthFor(Thing t)
	{
		Pawn pawn = t as Pawn;
		if (t.def.useStandardHealth || pawn != null)
		{
			GenUI.SetFontSmall();
			int num = pawn?.healthTracker.MaxHealth ?? t.def.maxHealth;
			Color color = Color.white;
			if (t.HealthState == HealthState.LightDamage)
			{
				color = Color.yellow;
			}
			if (t.HealthState == HealthState.HeavyDamage)
			{
				color = Color.red;
			}
			if (t.HealthState == HealthState.Dead)
			{
				color = Color.grey;
			}
			GUI.color = color;
			Rect position = new Rect(0f, 33f, 200f, 50f);
			GUI.Label(position, t.health + " / " + num);
			GUI.color = Color.white;
		}
	}

	public static void DrawInspectStringFor(Thing t)
	{
		GenUI.SetFontSmall();
		Vector2 paneInnerSize = UI_InspectPane.PaneInnerSize;
		Rect position = new Rect(0f, 53f, paneInnerSize.x, 100f);
		GUI.Label(position, t.GetInspectString());
	}

	public static void DrawCommandGridFor(IEnumerable<Thing> selThingsEnum)
	{
		List<Thing> list = selThingsEnum.ToList();
		IEnumerable<IGrouping<int, Command>> enumerable = from th in list
			from opt in th.GetCommandOptions()
			group opt by opt.GetHashCode() into g
			select (g);
		GenUI.SetFontSmall();
		Vector2 commandGridRoot = CommandGridRoot;
		float x = commandGridRoot.x;
		Vector2 commandGridRoot2 = CommandGridRoot;
		Rect position = new Rect(x, commandGridRoot2.y, 999f, 999f);
		GUI.BeginGroup(position);
		int count = list.Count;
		IntVec2 coords = new IntVec2(0, 0);
		foreach (IGrouping<int, Command> item in enumerable)
		{
			if (item.Count() < count)
			{
				continue;
			}
			Command command = item.Where((Command opt) => !opt.disabled).FirstOrDefault();
			if (command == null)
			{
				command = item.First();
			}
			if (command.DoButtonGUI(coords))
			{
				if (command.disabled)
				{
					UI_Messages.Message(command.disabledReason, UIMessageSound.Reject);
				}
				else
				{
					if (command.CurClickSound != null)
					{
						GenSound.PlaySoundOnCamera(command.CurClickSound, 0.1f);
					}
					foreach (Command item2 in item)
					{
						if (item2 != command && !item2.disabled && item2.ShareClicksFrom(command))
						{
							item2.action();
						}
					}
					command.action();
				}
				Event.current.Use();
			}
			coords.x++;
			if (coords.x >= 2)
			{
				coords.x = 0;
				coords.z++;
			}
		}
		GUI.EndGroup();
	}
}
