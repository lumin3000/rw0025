public class DialogBox_DialogTree : DialogBox
{
	private DiaNode CurNode;

	public DialogBox_DialogTree(DiaNode NodeRoot)
	{
		GotoNode(NodeRoot);
		SetWinCentered(600f, 400f);
	}

	public override void PreClose()
	{
		base.PreClose();
		CurNode.PreClose();
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		CurNode.NodeOnGUI(winRect.GetInnerRect(20f));
		GenUI.AbsorbAllInput();
	}

	public void GotoNode(DiaNode newNode)
	{
		CurNode = newNode;
		newNode.Opened();
	}
}
