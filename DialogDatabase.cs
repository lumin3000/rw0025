using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase
{
	private static List<DiaNodeDef> Nodes;

	private static List<DiaNodeList> NodeLists;

	static DialogDatabase()
	{
		Nodes = new List<DiaNodeDef>();
		NodeLists = new List<DiaNodeList>();
		LoadAllDialog();
	}

	private static void LoadAllDialog()
	{
		Nodes.Clear();
		Object[] array = Resources.LoadAll("Dialog", typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			TextAsset ass = @object as TextAsset;
			if (@object.name == "BaseEncounters" || @object.name == "GeneratedDialogs")
			{
				DialogLoader.LoadFileIntoList(ass, Nodes, NodeLists, DiaNodeType.BaseEncounters);
			}
			if (@object.name == "InsanityBattles")
			{
				DialogLoader.LoadFileIntoList(ass, Nodes, NodeLists, DiaNodeType.InsanityBattles);
			}
			if (@object.name == "SpecialEncounters")
			{
				DialogLoader.LoadFileIntoList(ass, Nodes, NodeLists, DiaNodeType.Special);
			}
		}
		foreach (DiaNodeDef node in Nodes)
		{
			node.PostLoad();
		}
		DialogLoader.MarkNonRootNodes(Nodes);
	}

	public static DiaNodeDef GetRandomEncounterRootNode(DiaNodeType NType)
	{
		List<DiaNodeDef> list = new List<DiaNodeDef>();
		foreach (DiaNodeDef node in Nodes)
		{
			if (node.IsRoot && (!node.Unique || !node.Used) && node.NodeType == NType)
			{
				list.Add(node);
			}
		}
		return list.RandomElement();
	}

	public static DiaNodeDef GetNodeNamed(string NodeName)
	{
		foreach (DiaNodeDef node in Nodes)
		{
			if (node.Name == NodeName)
			{
				return node;
			}
		}
		foreach (DiaNodeList nodeList in NodeLists)
		{
			if (nodeList.Name == NodeName)
			{
				return nodeList.RandomNodeFromList();
			}
		}
		Debug.LogError("Did not find node named '" + NodeName + "'.");
		return null;
	}
}
