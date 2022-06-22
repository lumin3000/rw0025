using System.Collections.Generic;
using UnityEngine;

public abstract class ThinkNode
{
	public Pawn pawn;

	public List<ThinkNode> subNodes = new List<ThinkNode>();

	public abstract JobPackage TryIssueJobPackage();

	public virtual void SetPawn(Pawn newPawn)
	{
		pawn = newPawn;
		foreach (ThinkNode subNode in subNodes)
		{
			subNode.SetPawn(newPawn);
		}
	}

	public IEnumerable<ThinkNode> WholeSubtree()
	{
		foreach (ThinkNode subNode in subNodes)
		{
			foreach (ThinkNode item in subNode.WholeSubtree())
			{
				yield return item;
			}
			yield return subNode;
		}
	}

	public T GetThinkNode<T>() where T : ThinkNode
	{
		foreach (ThinkNode item in WholeSubtree())
		{
			if (item.GetType() == typeof(T))
			{
				return (T)item;
			}
		}
		Debug.LogWarning(string.Concat(pawn, " looked for ThinkNode of type ", typeof(T), " they didn't have."));
		return (T)null;
	}
}
