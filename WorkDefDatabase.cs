using System.Collections.Generic;
using System.Linq;

public static class WorkDefDatabase
{
	private static List<WorkDefinition> allWorkDefs;

	public static IEnumerable<WorkDefinition> AllWorkDefinitions => allWorkDefs;

	public static IEnumerable<WorkDefinition> AutomaticWorksInPriorityOrder => from wt in allWorkDefs
		where wt.automatic
		orderby wt.naturalPriority descending
		select wt;

	static WorkDefDatabase()
	{
		allWorkDefs = new List<WorkDefinition>();
		foreach (WorkDefinition item in WorkDefsHardcoded.AllWorkDefinitions())
		{
			allWorkDefs.Add(item);
		}
	}

	public static WorkDefinition DefinitionOf(WorkType w)
	{
		return allWorkDefs[(int)w];
	}

	public static WorkDefinition GetDefinition(this WorkType w)
	{
		return DefinitionOf(w);
	}

	public static string GetLabel(this WorkType l)
	{
		return DefinitionOf(l).pawnLabel;
	}
}
