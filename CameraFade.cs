using UnityEngine;

public class CameraFade : MonoBehaviour
{
	private GUIStyle backgroundStyle = new GUIStyle();

	private Texture2D fadeTexture;

	private Color currentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	private Color targetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	private Color deltaColor = new Color(0f, 0f, 0f, 0f);

	private int fadeGUIDepth = -1000;

	private void Awake()
	{
		fadeTexture = new Texture2D(1, 1);
		backgroundStyle.normal.background = fadeTexture;
		SetScreenOverlayColor(currentScreenOverlayColor);
	}

	private void Update()
	{
		if (currentScreenOverlayColor != targetScreenOverlayColor)
		{
			if (Mathf.Abs(currentScreenOverlayColor.a - targetScreenOverlayColor.a) < Mathf.Abs(deltaColor.a) * Time.deltaTime)
			{
				currentScreenOverlayColor = targetScreenOverlayColor;
				SetScreenOverlayColor(currentScreenOverlayColor);
				deltaColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				SetScreenOverlayColor(currentScreenOverlayColor + deltaColor * Time.deltaTime);
			}
		}
	}

	private void OnGUI()
	{
		if (currentScreenOverlayColor.a > 0f)
		{
			GUI.depth = fadeGUIDepth;
			GUI.Label(new Rect(-10f, -10f, Screen.width + 10, Screen.height + 10), fadeTexture, backgroundStyle);
		}
	}

	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		currentScreenOverlayColor = newScreenOverlayColor;
		fadeTexture.SetPixel(0, 0, currentScreenOverlayColor);
		fadeTexture.Apply();
	}

	public void StartFade(Color newScreenOverlayColor, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			SetScreenOverlayColor(newScreenOverlayColor);
			return;
		}
		targetScreenOverlayColor = newScreenOverlayColor;
		deltaColor = (targetScreenOverlayColor - currentScreenOverlayColor) / fadeDuration;
	}
}
