using UnityEngine;

public class TooltipDef
{
	public string tipText;

	public TooltipPriority priority;

	public int uniqueId;

	public double firstTriggerTime;

	public int lastTriggerFrame;

	public double triggerTime => (double)Time.realtimeSinceStartup - firstTriggerTime;

	public TooltipDef(string newText, int newDefCode)
	{
		tipText = newText;
		uniqueId = newDefCode;
	}

	public TooltipDef(string newText, string DefCodeText)
	{
		tipText = newText;
		uniqueId = QuickAndDirtyHash(DefCodeText);
	}

	public TooltipDef(string newText)
	{
		tipText = newText;
		uniqueId = QuickAndDirtyHash(newText);
	}

	public TooltipDef(TooltipDef Other)
	{
		tipText = Other.tipText;
		priority = Other.priority;
		uniqueId = Other.uniqueId;
		firstTriggerTime = Other.firstTriggerTime;
	}

	private int QuickAndDirtyHash(string str)
	{
		return str.GetHashCode();
	}

	public float DrawTooltip(float OffsetVerticalAwayFromMouse)
	{
		GenUI.SetFontTiny();
		Vector2 vector = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(tipText));
		if (vector.x > 200f)
		{
			vector.x = 200f;
			vector.y = GUI.skin.GetStyle("Label").CalcHeight(new GUIContent(tipText), vector.x);
		}
		Rect baseRect = new Rect(0f, 0f, vector.x, vector.y);
		baseRect = baseRect.GetInnerRect(-4f);
		Vector3 vector2 = Event.current.mousePosition;
		if (vector2.y + 14f + baseRect.height < (float)Screen.height)
		{
			baseRect.y = vector2.y + 14f + OffsetVerticalAwayFromMouse;
		}
		else
		{
			baseRect.y = vector2.y - 5f - baseRect.height - OffsetVerticalAwayFromMouse;
		}
		if (vector2.x + 16f + baseRect.width < (float)Screen.width)
		{
			baseRect.x = vector2.x + 16f;
		}
		else
		{
			baseRect.x = vector2.x - 4f - baseRect.width;
		}
		UIWidgets.DrawShadowAround(baseRect);
		GUI.DrawTexture(baseRect, GenUI.BlackTex);
		GUI.Label(baseRect.GetInnerRect(4f), tipText);
		GenUI.DrawBox(baseRect, 1);
		return baseRect.height;
	}

	public override string ToString()
	{
		return "TIP[" + tipText + ", " + uniqueId + "]";
	}

	public static implicit operator TooltipDef(string str)
	{
		return new TooltipDef(str);
	}
}
