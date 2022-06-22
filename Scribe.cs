using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class Scribe
{
	public static LoadSaveMode mode;

	public static XmlWriter writer;

	public static XmlDocument doc;

	public static XmlNode curNode;

	public static bool writingForDebug;

	public static void EnterNode(string elementName)
	{
		if (mode == LoadSaveMode.Saving)
		{
			writer.WriteStartElement(elementName);
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			curNode = curNode[elementName];
		}
	}

	public static void ExitNode()
	{
		if (mode == LoadSaveMode.Saving)
		{
			writer.WriteEndElement();
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			curNode = curNode.ParentNode;
		}
	}

	public static void LookField<T>(ref T value, string label)
	{
		LookField(ref value, label, default(T));
	}

	public static void LookField<T>(ref T value, string label, T defaultValue)
	{
		LookField(ref value, label, defaultValue, forceSave: false);
	}

	public static void LookField<T>(ref T value, string label, bool forceSave)
	{
		LookField(ref value, label, default(T), forceSave);
	}

	public static void LookField<T>(ref T value, string label, T defaultValue, bool forceSave)
	{
		if (mode == LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (typeof(Saveable).IsAssignableFrom(typeof(T)))
		{
			Debug.LogWarning(string.Concat("Using Look with a Saveable reference (", value, "). Use LookSaveable instead."));
			return;
		}
		if (typeof(Thing).IsAssignableFrom(typeof(T)))
		{
			Debug.LogWarning(string.Concat("Using Look with a Thing reference (", value, "). Use LookRef instead."));
			return;
		}
		if (mode == LoadSaveMode.Saving && (!value.Equals(defaultValue) || forceSave))
		{
			writer.WriteElementString(label, value.ToString());
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			LoadFieldFromNode(curNode[label], ref value, defaultValue);
		}
	}

	private static void LoadFieldFromNode<T>(XmlNode subNode, ref T value, T defaultValue)
	{
		if (subNode != null)
		{
			value = (T)ParseHelper.FromString(subNode.InnerText, typeof(T));
		}
		else
		{
			value = defaultValue;
		}
	}

	public static void LookDefinition(ref ThingDefinition value, string label)
	{
		string value2 = ((value != null) ? value.definitionName : "null");
		LookField(ref value2, label, "null");
		if (mode == LoadSaveMode.LoadingVars)
		{
			if (value2 == "null")
			{
				value = null;
			}
			else
			{
				value = ThingDefDatabase.ThingDefNamed(value2);
			}
		}
	}

	public static void LookSaveable<T>(ref T target, string label) where T : Saveable
	{
		LookSaveable(ref target, label, new object[0]);
	}

	public static void LookSaveable<T>(ref T target, string label, object singleCtorArg) where T : Saveable
	{
		LookSaveable(ref target, label, new object[1] { singleCtorArg });
	}

	public static void LookSaveable<T>(ref T target, string label, object[] ctorArgs) where T : Saveable
	{
		if (mode == LoadSaveMode.Saving)
		{
			SaveSaveableToStream(target, label);
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			LoadSaveableFromNode(curNode[label], ref target, ctorArgs);
		}
	}

	private static void SaveSaveableToStream<T>(T target, string label) where T : Saveable
	{
		if (target == null)
		{
			EnterNode(label);
			writer.WriteAttributeString("IsNull", "True");
			ExitNode();
			return;
		}
		EnterNode(label);
		if (target.GetType() != typeof(T))
		{
			writer.WriteAttributeString("Class", target.GetType().FullName);
		}
		target.ExposeData();
		ExitNode();
	}

	private static void LoadSaveableFromNode<T>(XmlNode saveableNode, ref T target, object[] ctorArgs) where T : Saveable
	{
		//Discarded unreachable code: IL_0114
		if (saveableNode == null)
		{
			if (target == null)
			{
				Debug.LogWarning("Looked for a Saveable to fill NULL and found no XML node.");
			}
			else
			{
				Debug.LogWarning("Looked for a Saveable to fill " + target.ToString() + " and found no XML node.");
			}
			return;
		}
		XmlAttribute xmlAttribute = saveableNode.Attributes["IsNull"];
		if (xmlAttribute != null && xmlAttribute.Value == "True")
		{
			target = default(T);
			return;
		}
		try
		{
			Type type = null;
			XmlAttribute xmlAttribute2 = saveableNode.Attributes["Class"];
			type = ((xmlAttribute2 == null) ? typeof(T) : Type.GetType(xmlAttribute2.Value, throwOnError: true, ignoreCase: true));
			T val = (T)Activator.CreateInstance(type, ctorArgs);
			XmlNode xmlNode = curNode;
			curNode = saveableNode;
			val.ExposeData();
			curNode = xmlNode;
			PostLoadInitter.RegisterForPostLoadInit(val);
			target = val;
		}
		catch (Exception ex)
		{
			target = default(T);
			throw ex;
		}
	}

	public static void LookThingRef<T>(ref T refee, string label, Saveable requestor) where T : Thing
	{
		if (mode == LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (mode == LoadSaveMode.Saving)
		{
			if (refee == null)
			{
				writer.WriteElementString(label, "null");
				return;
			}
			if (Debug.isDebugBuild && refee.destroyed)
			{
				Debug.LogError(string.Concat("Tried to save a thing reference to destroyed thing ", refee, ". Requestor is ", requestor, ". Saving null ref."));
				writer.WriteElementString(label, "null");
				return;
			}
			if (Debug.isDebugBuild && !refee.def.HasThingIDNumber)
			{
				Debug.LogError("Trying to cross-reference save Thing which lacks ID number: " + refee);
				writer.WriteElementString(label, "null");
			}
			else if (Debug.isDebugBuild && refee.IsSaveCompressible())
			{
				Debug.LogError("Trying to save a reference to a thing that will be compressed away: " + refee);
				writer.WriteElementString(label, "null");
			}
			else
			{
				writer.WriteElementString(label, refee.ThingID);
			}
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			XmlNode xmlNode = curNode[label];
			if (xmlNode != null)
			{
				ThingRefHandler.RegisterDesiredCrossRef(requestor, xmlNode.InnerText);
			}
			else
			{
				refee = (T)null;
			}
		}
		if (mode == LoadSaveMode.ResolvingCrossRefs)
		{
			refee = (T)ThingRefHandler.NextResolvedRefFor(requestor);
		}
	}

	public static void LookList<T>(ref List<T> valueList, string listLabel)
	{
		if (mode == LoadSaveMode.PostLoadInit)
		{
			return;
		}
		string name = typeof(T).Name;
		bool flag = typeof(Saveable).IsAssignableFrom(typeof(T));
		if (mode == LoadSaveMode.Saving)
		{
			EnterNode(listLabel);
			foreach (T value2 in valueList)
			{
				if (flag)
				{
					SaveSaveableToStream((Saveable)(object)value2, name);
					continue;
				}
				T value = value2;
				LookField(ref value, name, forceSave: true);
			}
			ExitNode();
		}
		if (mode == LoadSaveMode.LoadingVars)
		{
			if (!flag)
			{
				valueList = LoadListFields<T>(listLabel, curNode[listLabel]);
			}
			else
			{
				valueList = LoadListSaveables<T>(listLabel, name, curNode[listLabel]);
			}
		}
	}

	private static List<T> LoadListFields<T>(string listLabel, XmlNode listRootNode)
	{
		List<T> list = new List<T>();
		if (listRootNode == null)
		{
			list = null;
			return null;
		}
		list = new List<T>();
		foreach (XmlNode childNode in listRootNode.ChildNodes)
		{
			T value = default(T);
			LoadFieldFromNode(childNode, ref value, default(T));
			list.Add(value);
		}
		return list;
	}

	private static List<T> LoadListSaveables<T>(string listLabel, string entryLabel, XmlNode listRootNode)
	{
		List<T> list = new List<T>();
		if (listRootNode == null)
		{
			list = null;
			return null;
		}
		list = new List<T>();
		foreach (XmlNode childNode in listRootNode.ChildNodes)
		{
			Saveable target = null;
			LoadSaveableFromNode(childNode, ref target, null);
			list.Add((T)target);
		}
		return list;
	}

	public static void LookHashSetThingRef(ref HashSet<Thing> valueHashSet, string listLabel, Saveable requestor)
	{
		List<Thing> valueList = valueHashSet.ToList();
		LookListThingRef(ref valueList, listLabel, requestor);
		valueHashSet.Clear();
		foreach (Thing item in valueList)
		{
			valueHashSet.Add(item);
		}
	}

	public static void LookListThingRef(ref List<Thing> valueList, string listLabel, Saveable requestor)
	{
		if (mode == LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (mode == LoadSaveMode.Saving || mode == LoadSaveMode.LoadingVars)
		{
			string label = "Ref";
			if (mode == LoadSaveMode.Saving)
			{
				EnterNode(listLabel);
				foreach (Thing value in valueList)
				{
					Thing refee = value;
					LookThingRef(ref refee, label, requestor);
				}
				ExitNode();
			}
			if (mode == LoadSaveMode.LoadingVars)
			{
				List<string> list = new List<string>();
				XmlNode xmlNode = curNode[listLabel];
				foreach (XmlNode childNode in xmlNode.ChildNodes)
				{
					list.Add(childNode.InnerText);
				}
				ThingRefHandler.RegisterDesiredCrossRefList(requestor, list);
			}
		}
		if (mode == LoadSaveMode.ResolvingCrossRefs)
		{
			valueList = ThingRefHandler.NextResolvedRefListFor(requestor);
		}
	}

	public static void LookDict<K, V>(ref Dictionary<K, V> dict, string dictLabel) where K : new()where V : new()
	{
		if (mode == LoadSaveMode.PostLoadInit)
		{
			return;
		}
		EnterNode(dictLabel);
		List<K> valueList = new List<K>();
		List<V> valueList2 = new List<V>();
		if (mode == LoadSaveMode.Saving)
		{
			foreach (KeyValuePair<K, V> item in dict)
			{
				valueList.Add(item.Key);
				valueList2.Add(item.Value);
			}
		}
		LookList(ref valueList, "KeyList");
		LookList(ref valueList2, "ValueList");
		if (mode == LoadSaveMode.LoadingVars)
		{
			dict.Clear();
			for (int i = 0; i < valueList.Count; i++)
			{
				dict.Add(valueList[i], valueList2[i]);
			}
		}
		ExitNode();
	}
}
