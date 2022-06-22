using UnityEngine;

public class CodexContent_Text : CodexContent
{
	public string text;

	public CodexContent_Text(string text)
	{
		this.text = text;
	}

	public override float DrawOnGUI(float width)
	{
		float num = GUI.skin.label.CalcHeight(new GUIContent(text), width);
		Rect position = new Rect(0f, 0f, width, num);
		GUI.Label(position, text);
		return num;
	}
}
