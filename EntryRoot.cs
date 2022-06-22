using UnityEngine;

public class EntryRoot : MonoBehaviour
{
	public UI_EntryRoot UIEntryRoot;

	private void Start()
	{
		GlobalInit.GlobalInitIfNotAlreadyDone();
		UIEntryRoot = new UI_EntryRoot();
	}

	private void Update()
	{
		if (Time.frameCount == 3)
		{
			Prefs.ApplyPrefs();
		}
		LongEventHandler.LongEventsUpdate();
	}

	private void OnGUI()
	{
		UIEntryRoot.UIRootOnGUI();
	}
}
