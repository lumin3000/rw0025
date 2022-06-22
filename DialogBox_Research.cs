using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogBox_Research : DialogBox
{
	private const float LeftAreaWidth = 230f;

	private const int ModeSelectButHeight = 40;

	private const float ProjectTitleHeight = 50f;

	private const float ProjectTitleLeftMargin = 20f;

	private const int ProjectIntervalY = 34;

	protected ResearchProject selectedProject;

	private bool showResearchedProjects;

	private Vector2 projectListScrollPosition = default(Vector2);

	private static readonly Texture2D BarFillTex = GenRender.SolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

	private static readonly Texture2D BarBGTex = GenRender.SolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

	public DialogBox_Research()
	{
		SetWinCentered(800f, 700f);
		selectedProject = Find.ResearchManager.CurrentProj;
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		if (UIWidgets.CloseButtonFor(winRect))
		{
			Find.Dialogs.PopBox();
		}
		Rect innerRect = winRect.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontMedium();
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(0f, 0f, innerRect.width, 300f), "Research");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GenUI.SetFontSmall();
		Rect rect = new Rect(0f, 75f, 230f, innerRect.height - 75f);
		UIWidgets.DrawMenuSection(rect, drawTop: false);
		Rect innerRect2 = rect.GetInnerRect(10f);
		IEnumerable<ResearchProject> enumerable = ((!showResearchedProjects) ? Find.ResearchManager.projectList.Where((ResearchProject p) => !p.IsFinished && p.PrereqsFulfilled()) : Find.ResearchManager.projectList.Where((ResearchProject p) => p.IsFinished && p.PrereqsFulfilled()));
		float height = 34 * enumerable.Count();
		Rect rect2 = new Rect(0f, 0f, innerRect2.width - 24f, height);
		projectListScrollPosition = GUI.BeginScrollView(innerRect2, projectListScrollPosition, rect2);
		Rect innerRect3 = rect2.GetInnerRect(10f);
		innerRect3.height += 20f;
		GUI.BeginGroup(innerRect3);
		int num = 0;
		foreach (ResearchProject item3 in enumerable)
		{
			Rect rect3 = new Rect(0f, num, innerRect3.width, 28f);
			if (selectedProject == item3)
			{
				GUI.DrawTexture(rect3, GenUI.HighlightTex);
			}
			Rect rect4 = new Rect(rect3);
			rect4.x += 6f;
			rect4.width -= 6f;
			if (UIWidgets.TextButton(rect4, item3.label, drawBackground: false, doMouseoverSound: true))
			{
				GenSound.PlaySoundOnCamera(UISounds.Click, 0.1f);
				selectedProject = item3;
			}
			num += 34;
		}
		GUI.EndGroup();
		GUI.EndScrollView();
		List<TabDef> list = new List<TabDef>();
		TabDef item = new TabDef("Researched", delegate
		{
			showResearchedProjects = true;
		}, showResearchedProjects);
		list.Add(item);
		TabDef item2 = new TabDef("Unresearched", delegate
		{
			showResearchedProjects = false;
		}, !showResearchedProjects);
		list.Add(item2);
		UIWidgets.DrawTabs(rect, list);
		Rect rect5 = new Rect(rect.x + rect.width + 10f, 45f, winRect.width - rect.width - 30f, innerRect.height - 45f);
		UIWidgets.DrawMenuSection(rect5);
		Rect innerRect4 = rect5.GetInnerRect(20f);
		GUI.BeginGroup(innerRect4);
		if (selectedProject != null)
		{
			GenUI.SetFontMedium();
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Rect position = new Rect(20f, 0f, innerRect4.width - 20f, 50f);
			GUI.Label(position, selectedProject.label);
			GenUI.ResetLabelAlign();
			GenUI.SetFontSmall();
			Rect position2 = new Rect(0f, 50f, innerRect4.width, innerRect4.height - 50f);
			GUI.Label(position2, selectedProject.descriptionBefore);
			Rect rect6 = new Rect(innerRect4.width / 2f - 50f, 300f, 100f, 50f);
			if (selectedProject.IsFinished)
			{
				UIWidgets.DrawMenuSection(rect6);
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label(rect6, "Finished");
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
			}
			else if (selectedProject == Find.ResearchManager.CurrentProj)
			{
				UIWidgets.DrawMenuSection(rect6);
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label(rect6, "In Progress");
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
			}
			else if (UIWidgets.TextButton(rect6, "Research"))
			{
				GenSound.PlaySoundOnCamera("Interface/ResearchStart", 0.25f);
				Find.ResearchManager.StartWorkOn(selectedProject.rType);
			}
			Rect rect7 = new Rect(15f, 450f, innerRect4.width - 30f, 35f);
			UIWidgets.FillableBar(rect7, selectedProject.PercentComplete, BarFillTex, doBlackBorder: true, BarBGTex);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(rect7, selectedProject.ProgressNumbersString());
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
		}
		GUI.EndGroup();
		GUI.EndGroup();
		DetectShouldClose(doButton: false);
		GenUI.AbsorbAllInput();
	}
}
