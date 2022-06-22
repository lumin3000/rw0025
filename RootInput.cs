using UnityEngine;

public static class RootInput
{
	public static void RootInputOnGUI()
	{
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			Event.current.Use();
			Find.UIMapRoot.dialogs.AddDialogBox(new DialogBox_MainMenu());
		}
	}
}
