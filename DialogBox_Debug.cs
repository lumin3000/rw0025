using System;
using UnityEngine;

public abstract class DialogBox_Debug : DialogBox
{
	protected const float ButSpacing = 4f;

	protected Vector2 curOffset;

	protected static readonly Vector2 ButSize = new Vector2(230f, 30f);

	protected readonly float ColumnSpacing = 30f;

	protected readonly float SectSpacing = 10f;

	public DialogBox_Debug()
	{
		SetWinCentered(1050f, 800f);
	}

	protected void StartNextColumn()
	{
		curOffset.y = 0f;
		ref Vector2 reference = ref curOffset;
		float x = reference.x;
		Vector2 butSize = ButSize;
		reference.x = x + (butSize.x + ColumnSpacing);
	}

	protected void AddSectionSpace()
	{
		curOffset.y += SectSpacing;
	}

	protected void AffectedThingEffect(Thing t)
	{
		(t as Pawn)?.drawer.Notify_DebugAffected();
	}

	protected void AddOption(string label, Action action)
	{
		float x = curOffset.x;
		float y = curOffset.y;
		Vector2 butSize = ButSize;
		float x2 = butSize.x;
		Vector2 butSize2 = ButSize;
		if (GUI.Button(new Rect(x, y, x2, butSize2.y), label))
		{
			Find.Dialogs.PopBox();
			action();
		}
		curOffset.y += 32f;
	}

	protected void AddOption(string label, ref bool b)
	{
		float x = curOffset.x;
		float y = curOffset.y;
		Vector2 butSize = ButSize;
		float x2 = butSize.x;
		Vector2 butSize2 = ButSize;
		UIWidgets.LabelCheckbox(new Rect(x, y, x2, butSize2.y), label, ref b);
		curOffset.y += 32f;
	}

	protected void AddTool(string label, Action toolAction)
	{
		float x = curOffset.x;
		float y = curOffset.y;
		Vector2 butSize = ButSize;
		float x2 = butSize.x;
		Vector2 butSize2 = ButSize;
		if (GUI.Button(new Rect(x, y, x2, butSize2.y), label))
		{
			Find.Dialogs.PopBox();
			DebugTools.curTool = new DebugTool(label, toolAction);
		}
		curOffset.y += 32f;
	}
}
