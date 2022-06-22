using UnityEngine;

public class DialogBox_Options : DialogBox
{
	private static readonly Vector2 WinSize = new Vector2(900f, 650f);

	public DialogBox_Options()
	{
		clearDialogStack = false;
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		SetWinCentered(WinSize);
		Rect innerRect = winRect.GetInnerRect(17f);
		UI_Listing uI_Listing = new UI_Listing(innerRect);
		uI_Listing.DoHeading("Audiovisuals");
		uI_Listing.DoGap();
		uI_Listing.DoGap();
		uI_Listing.DoLabel("Sound volume");
		Prefs.Volume = uI_Listing.DoSlider(Prefs.Volume);
		uI_Listing.DoLabel("Graphics quality");
		int num = 0;
		string[] names = QualitySettings.names;
		foreach (string label in names)
		{
			if (uI_Listing.DoRadioButton(label, Prefs.QualityIndex == num))
			{
				Prefs.QualityIndex = num;
			}
			num++;
		}
		bool val = Prefs.CustomCursorEnabled;
		uI_Listing.DoCheckbox("Custom cursor", ref val);
		Prefs.CustomCursorEnabled = val;
		uI_Listing.NewColumn();
		uI_Listing.DoHeading("Gameplay");
		uI_Listing.DoGap();
		uI_Listing.DoGap();
		bool val2 = Prefs.TutorialEnabled;
		uI_Listing.DoCheckbox("Tutorial enabled", ref val2);
		Prefs.TutorialEnabled = val2;
		uI_Listing.NewColumn();
		uI_Listing.DoHeading("Resolution");
		uI_Listing.DoGap();
		uI_Listing.DoGap();
		bool val3 = Screen.fullScreen;
		uI_Listing.DoCheckbox("Fullscreen", ref val3);
		Screen.fullScreen = val3;
		uI_Listing.DoLabel("Currently " + ResToString(Screen.currentResolution));
		Resolution[] resolutions = Screen.resolutions;
		for (int j = 0; j < resolutions.Length; j++)
		{
			Resolution res = resolutions[j];
			if (res.height >= 768 && res.width >= 1024 && uI_Listing.DoButton(ResToString(res)))
			{
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
			}
		}
		uI_Listing.End();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}

	private static string ResToString(Resolution res)
	{
		string text = res.width + "x" + res.height;
		if (res.width == 1280 && res.height == 720)
		{
			text += " (720p)";
		}
		if (res.width == 1920 && res.height == 1080)
		{
			text += " (1080p)";
		}
		return text;
	}
}
