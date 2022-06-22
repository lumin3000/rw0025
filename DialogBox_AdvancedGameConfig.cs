using UnityEngine;

public class DialogBox_AdvancedGameConfig : DialogBox
{
	private const float ColumnWidth = 200f;

	private static readonly Vector2 WinSize = new Vector2(700f, 500f);

	private static readonly int[] MapSizes = new int[3] { 200, 225, 250 };

	public DialogBox_AdvancedGameConfig()
	{
		clearDialogStack = false;
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		SetWinCentered(WinSize);
		Rect innerRect = winRect.GetInnerRect(17f);
		UI_Listing uI_Listing = new UI_Listing(innerRect);
		uI_Listing.DoLabel("Warning: The game isn't optimized yet, so performance will degrade fast at larger map sizes. The game plays well at 200x200, so please stick to that unless your machine is a beast and you're willing to encoutner performance problems.");
		uI_Listing.DoLabel("Map size");
		int[] mapSizes = MapSizes;
		foreach (int num in mapSizes)
		{
			string label = num + "x" + num + " (" + num * num + " squares)";
			if (uI_Listing.DoRadioButton(label, MapInitParams.mapSize == num))
			{
				MapInitParams.mapSize = num;
			}
		}
		uI_Listing.End();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}
}
