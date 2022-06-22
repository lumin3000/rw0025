using System;
using System.Reflection;
using UnityEngine;

public static class VersionControl
{
	public static Version version;

	public static string versionStringFull;

	public static string versionStringShort;

	public static DateTime buildDate;

	static VersionControl()
	{
		Version version = Assembly.GetExecutingAssembly().GetName().Version;
		buildDate = new DateTime(2000, 1, 1).AddDays(version.Build);
		int build = version.Build - 4805;
		VersionControl.version = new Version(version.Major, version.Minor, build, version.Revision);
		versionStringFull = VersionControl.version.Major + "." + VersionControl.version.Minor + "." + VersionControl.version.Build + " rev" + VersionControl.version.Revision;
		versionStringShort = VersionControl.version.Major + "." + VersionControl.version.Minor + "." + VersionControl.version.Build;
	}

	public static void DrawVersionInCorner()
	{
		GenUI.SetFontSmall();
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		string text = "Version " + versionStringShort;
		if (Debug.isDebugBuild)
		{
			text += " (development build)";
		}
		text = text + "\nCompiled " + buildDate.ToString("MMM d yyyy");
		GUI.Label(new Rect(10f, 10f, 999f, 999f), text);
		GUI.color = Color.white;
	}

	public static void LogVersionNumber()
	{
		Debug.Log("RimWorld " + versionStringFull);
	}
}
