using UnityEngine;

public static class GlobalInit
{
	private static bool globalInitDone;

	public static void GlobalInitIfNotAlreadyDone()
	{
		if (!globalInitDone)
		{
			VersionControl.LogVersionNumber();
			Application.targetFrameRate = 60;
			globalInitDone = true;
		}
	}
}
