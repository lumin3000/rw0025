using System;
using System.IO;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
	private const string ScreenshotFolder = "Screenshots";

	private int screenshotCount;

	private int lastShotFrame = -999;

	private string lastShotFilePath;

	private void Update()
	{
		if (Time.frameCount == lastShotFrame + 1)
		{
			UI_Messages.Message("Screenshot saved as " + lastShotFilePath);
		}
		if (Input.GetKeyDown(KeyCode.F11))
		{
			TakeShot();
		}
	}

	private void TakeShot()
	{
		string text = Application.persistentDataPath + Path.DirectorySeparatorChar + "Screenshots";
		try
		{
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2;
			do
			{
				screenshotCount++;
				text2 = text + Path.DirectorySeparatorChar + "screenshot" + screenshotCount + ".png";
			}
			while (File.Exists(text2));
			Application.CaptureScreenshot(text2);
			lastShotFrame = Time.frameCount;
			lastShotFilePath = text2;
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to save screenshot: " + ex.ToString());
		}
	}
}
