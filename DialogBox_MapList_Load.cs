using UnityEngine;

public class DialogBox_MapList_Load : DialogBox_MapList
{
	public DialogBox_MapList_Load()
	{
		interactButLabel = "Load";
	}

	protected override void DoMapEntryInteraction(string MapName)
	{
		MapInitParams.Reset();
		MapInitParams.mapToLoad = MapName;
		LongEventHandler.QueueLongEvent(delegate
		{
			Application.LoadLevel("Gameplay");
		}, "Loading...");
	}
}
