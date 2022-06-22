using UnityEngine;

public abstract class CodexContent
{
	private float lastCalculatedHeight;

	public abstract float DrawOnGUI(float width);

	public float TotalHeight(float width)
	{
		if (Event.current.type == EventType.Layout)
		{
			lastCalculatedHeight = DrawOnGUI(width);
		}
		return lastCalculatedHeight;
	}
}
