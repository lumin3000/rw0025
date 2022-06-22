using UnityEngine;

public static class GenWorldUI
{
	public const float NameBGHeight = 12f;

	public const float NameBGExtraWidth = 4f;

	private const float LabelOffsetYStandard = -0.4f;

	public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
	{
		Vector3 drawPos = thing.DrawPos;
		drawPos.z += worldOffsetZ;
		Vector2 result = Find.CameraMap.camera.WorldToScreenPoint(drawPos);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	public static void DrawThingLabelFor(Thing thing, string text, Color textColor)
	{
		DrawThingLabel(LabelDrawPosFor(thing, -0.4f), text, textColor);
	}

	public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
	{
		GenUI.SetFontTiny();
		float x = GUI.skin.label.CalcSize(new GUIContent(text)).x;
		Rect position = new Rect(screenPos.x - x / 2f - 4f, screenPos.y, x + 8f, 12f);
		GUI.DrawTexture(position, GenUI.GrayTextBG);
		GUI.color = textColor;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(screenPos.x - x / 2f, screenPos.y - 2f, x, 999f), text);
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}
}
