using System;

public class DialogBox_DebugCameraConfigChooser : DialogBox_DebugLister
{
	protected override void DoList()
	{
		foreach (Type item in typeof(CameraMapConfig).AllSubclasses())
		{
			Type localType = item;
			AddOption(localType.Name, delegate
			{
				Find.CameraMap.config = (CameraMapConfig)Activator.CreateInstance(localType);
			});
		}
	}
}
