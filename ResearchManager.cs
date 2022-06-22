using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchManager : Saveable
{
	private const float ResearchProgressScale = 1.2f;

	protected const int NoProjectWarningInterval = 1000;

	public List<ResearchProject> projectList = new List<ResearchProject>();

	protected long lastNoProjectWarningTick = -2147483648L;

	public ResearchType currentProjType;

	public ResearchProject CurrentProj
	{
		get
		{
			if (currentProjType == ResearchType.None)
			{
				return null;
			}
			return ProjectOfType(currentProjType);
		}
		set
		{
			currentProjType = value.rType;
		}
	}

	public bool ProjectsAreAvailable => projectList.Where((ResearchProject pr) => !pr.IsFinished).Any();

	public ResearchManager()
	{
		projectList = ResearchContent.DefaultProjectList;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref currentProjType, "CurrentProject", forceSave: true);
		Scribe.LookList(ref projectList, "ProjectList");
		IEnumerable<ResearchProject> source = ResearchContent.DefaultProjectList.Where((ResearchProject defProj) => !projectList.Any((ResearchProject hadProj) => hadProj.rType == defProj.rType));
		foreach (ResearchProject item in source.ToList())
		{
			projectList.Add(item);
		}
	}

	public ResearchProject ProjectOfType(ResearchType rType)
	{
		foreach (ResearchProject project in projectList)
		{
			if (project.rType == rType)
			{
				return project;
			}
		}
		Debug.LogError("Missing research project " + rType);
		return null;
	}

	public void ResearchProgressMade(float progressAmount)
	{
		if (DebugSettings.fastResearch)
		{
			progressAmount *= 50f;
		}
		progressAmount *= 1.2f;
		if (CurrentProj == null)
		{
			if (Find.TickManager.tickCount > lastNoProjectWarningTick + 1000)
			{
				lastNoProjectWarningTick = Find.TickManager.tickCount;
			}
			return;
		}
		CurrentProj.progress += progressAmount;
		if (CurrentProj.IsFinished)
		{
			FinishedProject();
		}
	}

	public void StartWorkOn(ResearchType Proj)
	{
		currentProjType = Proj;
	}

	protected void FinishedProject()
	{
		string text = "Research finished: " + CurrentProj.label + "\n\n" + CurrentProj.DiscoveredText;
		DiaNode diaNode = new DiaNode(text);
		diaNode.optionList.Add(DiaOption.DefaultOK);
		DiaOption diaOption = new DiaOption("Research Screen");
		diaOption.ResolveTree = true;
		diaOption.ChosenCallback = delegate
		{
			Find.UIRoot.dialogs.AddDialogBox(new DialogBox_Research());
		};
		diaNode.optionList.Add(diaOption);
		DialogBoxHelper.InitDialogTree(diaNode);
		currentProjType = ResearchType.None;
		ReapplyAllMods();
	}

	public bool HasResearched(ResearchType r)
	{
		return ProjectOfType(r).IsFinished;
	}

	public void ReapplyAllMods()
	{
		if (HasResearched(ResearchType.GunTurretCooling))
		{
			ThingDefDatabase.ThingDefNamed("Gun_L-15 LMG").verbDef.burstShotCount = 4;
		}
	}
}
