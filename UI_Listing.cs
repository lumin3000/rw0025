using UnityEngine;

public class UI_Listing
{
	private const float ColumnWidth = 200f;

	private Rect rect;

	private float curX;

	private float curY;

	public UI_Listing(Rect rect)
	{
		this.rect = rect;
		GUI.BeginGroup(rect);
		GenUI.SetFontSmall();
	}

	public void End()
	{
		GUI.EndGroup();
	}

	public void DoLabel(string lab)
	{
		float num = GUI.skin.label.CalcHeight(new GUIContent(lab), 200f);
		num += 10f;
		Rect position = new Rect(curX, curY, 200f, num);
		GUI.Label(position, lab);
		curY += num;
		CheckLoop();
	}

	public void DoHeading(string text)
	{
		GenUI.SetFontMedium();
		DoLabel(text);
		GenUI.SetFontSmall();
	}

	public float DoSlider(float val)
	{
		Rect position = new Rect(curX, curY, 200f, 50f);
		float result = GUI.HorizontalSlider(position, val, 0f, 1f);
		curY += 50f;
		CheckLoop();
		return result;
	}

	public void DoCheckbox(string label, ref bool val)
	{
		UIWidgets.LabelCheckbox(new Rect(curX, curY, 200f, 30f), label, ref val);
		curY += 34f;
		CheckLoop();
	}

	public bool DoRadioButton(string label, bool active)
	{
		bool result = UIWidgets.LabelRadioButton(new Rect(curX, curY, 200f, 30f), label, active);
		curY += 34f;
		CheckLoop();
		return result;
	}

	public void DoGap()
	{
		curY += 12f;
		CheckLoop();
	}

	public bool DoButton(string label)
	{
		bool result = UIWidgets.TextButton(new Rect(curX, curY, 200f, 30f), label);
		curY += 34f;
		CheckLoop();
		return result;
	}

	public void NewColumn()
	{
		curY = 0f;
		curX += 217f;
	}

	private void CheckLoop()
	{
		if (curY > rect.height - 70f)
		{
			NewColumn();
		}
	}
}
