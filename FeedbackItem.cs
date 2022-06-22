using UnityEngine;

public abstract class FeedbackItem
{
	protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

	public float TimeLeft = 2f;

	protected Vector2 CurScreenPos;

	public FeedbackItem(Vector2 ScreenPos)
	{
		CurScreenPos = ScreenPos;
		CurScreenPos.y -= 15f;
	}

	public void Update()
	{
		TimeLeft -= Time.deltaTime;
		CurScreenPos += FloatPerSecond * Time.deltaTime;
	}

	public abstract void FeedbackOnGUI();

	protected void DrawFloatingText(string Text, Color TextColor)
	{
		GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperCenter;
		GenUI.SetFontSmall();
		float x = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(Text)).x;
		Rect position = new Rect(CurScreenPos.x - x / 2f, CurScreenPos.y, x, 14f);
		GUI.DrawTexture(position, GenUI.GrayTextBG);
		position.y -= 2f;
		position.height += 100f;
		GUI.color = TextColor;
		GUI.Label(position, Text);
		GUI.color = Color.white;
		GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
	}
}
