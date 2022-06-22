using UnityEngine;

public class Prefs
{
	public static float Volume
	{
		get
		{
			return PlayerPrefs.GetFloat("Volume", 1f);
		}
		set
		{
			AudioListener.volume = value;
			PlayerPrefs.SetFloat("Volume", value);
		}
	}

	public static bool TutorialEnabled
	{
		get
		{
			if (PlayerPrefs.GetInt("TutorialEnabled", 1) == 0)
			{
				return false;
			}
			return true;
		}
		set
		{
			int value2 = (value ? 1 : 0);
			PlayerPrefs.SetInt("TutorialEnabled", value2);
		}
	}

	public static bool CustomCursorEnabled
	{
		get
		{
			if (PlayerPrefs.GetInt("CustomCursorEnabled", 1) == 0)
			{
				return false;
			}
			return true;
		}
		set
		{
			int value2 = (value ? 1 : 0);
			PlayerPrefs.SetInt("CustomCursorEnabled", value2);
			if (value)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
		}
	}

	public static int QualityIndex
	{
		get
		{
			return PlayerPrefs.GetInt("QualityIndex");
		}
		set
		{
			if (QualitySettings.GetQualityLevel() != value)
			{
				QualitySettings.SetQualityLevel(value, applyExpensiveChanges: true);
			}
			PlayerPrefs.SetInt("QualityIndex", value);
			ApplyPrefs();
		}
	}

	public static void ApplyPrefs()
	{
		if (CustomCursorEnabled)
		{
			CustomCursor.Activate();
		}
		else
		{
			CustomCursor.Deactivate();
		}
		AudioListener.volume = Volume;
		if (QualitySettings.GetQualityLevel() != QualityIndex)
		{
			QualitySettings.SetQualityLevel(QualityIndex, applyExpensiveChanges: true);
		}
	}
}
