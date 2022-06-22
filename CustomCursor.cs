using UnityEngine;

public static class CustomCursor
{
	private static readonly Texture2D CursorTex = Res.LoadTexture("UI/Cursors/CursorCustom");

	private static Vector2 CursorHotspot = new Vector2(3f, 3f);

	public static void Activate()
	{
		Cursor.SetCursor(CursorTex, CursorHotspot, CursorMode.Auto);
	}

	public static void Deactivate()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
