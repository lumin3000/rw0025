using UnityEngine;

public class CodexContent_Image : CodexContent
{
	public Texture2D tex;

	public CodexContent_Image(string imgPath)
	{
		tex = Res.LoadTexture(imgPath);
	}

	public override float DrawOnGUI(float width)
	{
		float height = 200f;
		Rect position = new Rect(0f, 0f, width, height);
		Texture texture = tex;
		if (texture == null)
		{
			texture = GenUI.MissingContentTex;
		}
		GUI.DrawTexture(position, tex);
		return 200f;
	}
}
