using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_DialogBoxHandler
{
	private Stack<DialogBox> dialogStack = new Stack<DialogBox>();

	public DialogBox TopDialog
	{
		get
		{
			if (dialogStack.Count > 0)
			{
				return dialogStack.Peek();
			}
			return null;
		}
	}

	public void DialogBoxesOnGUI()
	{
		if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
		{
			foreach (DialogBox item in dialogStack.ToList())
			{
				item.DoDialogBoxGUI();
			}
			return;
		}
		List<DialogBox> list = dialogStack.ToList();
		list.Reverse();
		foreach (DialogBox item2 in list)
		{
			item2.DoDialogBoxGUI();
		}
	}

	public void AddDialogBox(DialogBox newDialogBox)
	{
		if (newDialogBox.clearDialogStack)
		{
			dialogStack.Clear();
		}
		dialogStack.Push(newDialogBox);
	}

	public void PopBox()
	{
		if (dialogStack.Count == 0)
		{
			Debug.LogWarning("Cleared dialog box without having one.");
			return;
		}
		dialogStack.Peek().PreClose();
		dialogStack.Pop();
	}
}
