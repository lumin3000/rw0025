using System;
using UnityEngine;

public class ListableOption_WebLink : ListableOption
{
	public Texture2D image;

	public string url;

	private static readonly Vector2 Imagesize = new Vector2(24f, 18f);

	public ListableOption_WebLink(string label, Texture2D image)
		: base(label, null)
	{
		overSpace = 8f;
		height = 24f;
		this.image = image;
	}

	public ListableOption_WebLink(string label, string url, Texture2D image)
		: this(label, image)
	{
		this.url = url;
	}

	public ListableOption_WebLink(string label, Action action, Texture2D image)
		: this(label, image)
	{
		base.action = action;
	}

	public override void DrawOption(Rect rect)
	{
		if (image != null)
		{
			Rect position = new Rect(rect);
			Vector2 imagesize = Imagesize;
			position.width = imagesize.x;
			Vector2 imagesize2 = Imagesize;
			position.height = imagesize2.y;
			position.y = rect.y + rect.height / 2f - position.height / 2f;
			if (rect.Contains(Event.current.mousePosition))
			{
				GUI.color = Color.yellow;
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}
		Rect rect2 = new Rect(rect);
		float xMin = rect2.xMin;
		Vector2 imagesize3 = Imagesize;
		rect2.xMin = xMin + (imagesize3.x + 3f);
		if (UIWidgets.TextButton(rect2, label, drawBackground: false, doMouseoverSound: true, Color.white))
		{
			if (action != null)
			{
				action();
			}
			else
			{
				Application.OpenURL(url);
			}
		}
	}
}
