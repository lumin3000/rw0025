using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

public static class DialogLoader
{
	public static void LoadFileIntoList(TextAsset ass, List<DiaNodeDef> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
	{
		//Discarded unreachable code: IL_00d0, IL_0198
		TextReader reader = new StringReader(ass.text);
		XPathDocument xPathDocument = new XPathDocument(reader);
		XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
		xPathNavigator.MoveToFirst();
		xPathNavigator.MoveToFirstChild();
		foreach (XPathNavigator item2 in xPathNavigator.Select("Node"))
		{
			try
			{
				TextReader textReader = new StringReader(item2.OuterXml);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiaNodeDef));
				DiaNodeDef diaNodeDef = (DiaNodeDef)xmlSerializer.Deserialize(textReader);
				diaNodeDef.NodeType = NodesType;
				NodeListToFill.Add(diaNodeDef);
				textReader.Dispose();
			}
			catch (Exception ex)
			{
				Debug.Log("Exception deserializing " + item2.OuterXml + ":\n" + ex.InnerException);
			}
		}
		foreach (XPathNavigator item3 in xPathNavigator.Select("NodeList"))
		{
			try
			{
				TextReader textReader2 = new StringReader(item3.OuterXml);
				XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(DiaNodeList));
				DiaNodeList item = (DiaNodeList)xmlSerializer2.Deserialize(textReader2);
				ListListToFill.Add(item);
			}
			catch (Exception ex2)
			{
				Debug.Log("Exception deserializing " + item3.OuterXml + ":\n" + ex2.InnerException);
			}
		}
	}

	public static void MarkNonRootNodes(List<DiaNodeDef> NodeList)
	{
		foreach (DiaNodeDef Node in NodeList)
		{
			RecursiveSetIsRootFalse(Node);
		}
		foreach (DiaNodeDef Node2 in NodeList)
		{
			foreach (DiaNodeDef Node3 in NodeList)
			{
				foreach (DiaOptionDef option in Node3.OptionList)
				{
					bool flag = false;
					foreach (string childNodeName in option.ChildNodeNames)
					{
						if (childNodeName == Node2.Name)
						{
							flag = true;
						}
					}
					if (option.MissionToStart != null && option.MissionToStart.VicNodeNames.Contains(Node2.Name))
					{
						flag = true;
					}
					if (flag)
					{
						Node2.IsRoot = false;
					}
				}
			}
		}
	}

	private static void RecursiveSetIsRootFalse(DiaNodeDef d)
	{
		foreach (DiaOptionDef option in d.OptionList)
		{
			foreach (DiaNodeDef childNode in option.ChildNodes)
			{
				childNode.IsRoot = false;
				RecursiveSetIsRootFalse(childNode);
			}
		}
	}
}
