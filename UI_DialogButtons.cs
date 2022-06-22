using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_DialogButtons
{
	private class DialogButtonHook
	{
		public Type dialogType;

		public string label;

		public string tipString;

		public DialogButtonHook(Type dialogType, string label, string tipString)
		{
			this.dialogType = dialogType;
			this.label = label;
			this.tipString = tipString;
		}
	}

	private const float ButSpacing = 5f;

	private const float ButMargin = 8f;

	private readonly List<DialogButtonHook> HookList;

	public UI_DialogButtons()
	{
		HookList = new List<DialogButtonHook>();
		HookList.Add(new DialogButtonHook(typeof(DialogBox_Overview), "Overview", "Manage colony work priorities and statistics."));
		HookList.Add(new DialogButtonHook(typeof(DialogBox_Research), "Research", "Examine and decide on research projects."));
		HookList.Add(new DialogButtonHook(typeof(DialogBox_Codex), "Codex", "Find help playing."));
		HookList.Add(new DialogButtonHook(typeof(DialogBox_MainMenu), "Menu", "Save, load, or quit the game, or change options."));
	}

	public void DialogButtonsOnGUI(Rect ContentRect)
	{
		UIWidgets.DrawShadowAround(ContentRect);
		UIWidgets.DrawMenuSection(ContentRect);
		Rect innerRect = ContentRect.GetInnerRect(8f);
		GUI.BeginGroup(innerRect);
		float num = (innerRect.height + 5f) / (float)HookList.Count;
		num -= 5f;
		float num2 = 0f;
		foreach (DialogButtonHook hook in HookList)
		{
			Rect rect = new Rect(0f, num2, innerRect.width, num);
			if (UIWidgetsSpecial.IconButton(barPercent: (hook.dialogType != typeof(DialogBox_Research) || Find.ResearchManager.CurrentProj == null) ? 0f : Find.ResearchManager.CurrentProj.PercentComplete, butRect: rect, label: hook.label, icon: null))
			{
				Find.UIRoot.dialogs.AddDialogBox((DialogBox)Activator.CreateInstance(hook.dialogType));
			}
			TooltipHandler.TipRegion(rect, hook.tipString);
			num2 += num + 5f;
		}
		GUI.EndGroup();
	}
}
