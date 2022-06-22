public static class DialogBoxHelper
{
	public static void InitDialogTree(DiaNode NodeRoot)
	{
		Find.UIMapRoot.dialogs.AddDialogBox(new DialogBox_DialogTree(NodeRoot));
	}
}
