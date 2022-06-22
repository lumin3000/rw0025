using System.Collections.Generic;
using UnityEngine;

public class ThinkNode_SatisfyNeeds : ThinkNode
{
	private class NeedScanned
	{
		public Need needType;

		public StatusLevel piece;

		public float level => piece.curLevel;

		public NeedScanned(Need NType, StatusLevel Piece)
		{
			needType = NType;
			piece = Piece;
		}

		public override string ToString()
		{
			return string.Concat(needType, "=", level);
		}
	}

	private JobGiver_GetFood giverGetFood = new JobGiver_GetFood();

	private JobGiver_GetRest giverGetRest = new JobGiver_GetRest();

	public ThinkNode_SatisfyNeeds()
	{
		subNodes.Add(giverGetFood);
		subNodes.Add(giverGetRest);
	}

	private static int CompareNeedsByLevel(NeedScanned A, NeedScanned B)
	{
		if (A.level < B.level)
		{
			return -1;
		}
		if (A.level == B.level)
		{
			return 0;
		}
		return 1;
	}

	public override JobPackage TryIssueJobPackage()
	{
		List<NeedScanned> list = new List<NeedScanned>();
		if (pawn.food != null)
		{
			list.Add(new NeedScanned(Need.Food, pawn.food.Food));
		}
		if (pawn.rest != null)
		{
			list.Add(new NeedScanned(Need.Rest, pawn.rest.Rest));
		}
		if (list.Count == 0)
		{
			Debug.LogError(string.Concat(pawn, " has SatisfyNeeds JobGiver but no needs to satisfy."));
			return null;
		}
		list.Sort(CompareNeedsByLevel);
		foreach (NeedScanned item in list)
		{
			if (!item.piece.ShouldTrySatisfy)
			{
				return null;
			}
			JobPackage jobPackage = null;
			if (item.needType == Need.Food)
			{
				jobPackage = giverGetFood.TryIssueJobPackage();
			}
			if (item.needType == Need.Rest)
			{
				jobPackage = giverGetRest.TryIssueJobPackage();
			}
			if (jobPackage != null)
			{
				return jobPackage;
			}
		}
		return null;
	}
}
