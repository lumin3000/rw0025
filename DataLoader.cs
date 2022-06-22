using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

public static class DataLoader
{
	private static Dictionary<string, XmlNode> nodesByName = new Dictionary<string, XmlNode>();

	public static List<T> LoadDataInFolder<T>(string folderPath) where T : new()
	{
		string path = "Text/" + folderPath;
		List<XmlNodeList> list = new List<XmlNodeList>();
		object[] array = Resources.LoadAll(path, typeof(TextAsset));
		object[] array2 = array;
		foreach (object obj in array2)
		{
			list.Add(NodesFromXml<T>(((TextAsset)obj).text));
		}
		nodesByName.Clear();
		foreach (XmlNodeList item in list)
		{
			LoadInheritanceInfoFromNodes<T>(item);
		}
		List<T> itemList = new List<T>();
		foreach (XmlNodeList item2 in list)
		{
			LoadDataFromNodes(ref itemList, item2);
		}
		return itemList;
	}

	private static XmlNodeList NodesFromXml<T>(string xmlString)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlString);
		return xmlDocument.DocumentElement.SelectNodes(typeof(T).Name);
	}

	private static void LoadInheritanceInfoFromNodes<T>(XmlNodeList itemNodes)
	{
		foreach (XmlNode itemNode in itemNodes)
		{
			XmlAttribute xmlAttribute = itemNode.Attributes["Name"];
			if (xmlAttribute != null)
			{
				if (nodesByName.ContainsKey(xmlAttribute.Value))
				{
					Debug.LogWarning("Duplicate inheritance name " + xmlAttribute.Value);
				}
				else
				{
					nodesByName.Add(xmlAttribute.Value, itemNode);
				}
			}
		}
	}

	private static void LoadDataFromNodes<T>(ref List<T> itemList, XmlNodeList itemNodes) where T : new()
	{
		foreach (XmlNode itemNode in itemNodes)
		{
			XmlAttribute xmlAttribute = itemNode.Attributes["Abstract"];
			if (xmlAttribute == null || !(xmlAttribute.Value.ToLower() == "true"))
			{
				itemList.Add(ItemFromXml<T>(itemNode, doPostLoad: true));
			}
		}
	}

	private static T ItemFromXml<T>(XmlNode itemRoot, bool doPostLoad) where T : new()
	{
		if (!itemRoot.HasChildNodes)
		{
			return default(T);
		}
		if (itemRoot.ChildNodes.Count == 1 && itemRoot.FirstChild.NodeType == XmlNodeType.Text)
		{
			return (T)ParseHelper.FromString(itemRoot.InnerText, typeof(T));
		}
		if (Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
		{
			List<T> list = ListFromXml<T>(itemRoot);
			int num = 0;
			foreach (T item in list)
			{
				int num2 = (int)(object)item;
				num |= num2;
			}
			return (T)(object)num;
		}
		if (typeof(T).HasGenericDefinition(typeof(List<>)))
		{
			Type[] genericArguments = typeof(T).GetGenericArguments();
			MethodInfo method = typeof(DataLoader).GetMethod("ListFromXml", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo = method.MakeGenericMethod(genericArguments);
			object[] parameters = new object[1] { itemRoot };
			object obj = methodInfo.Invoke(null, parameters);
			return (T)obj;
		}
		if (typeof(T).HasGenericDefinition(typeof(HashSet<>)))
		{
			Type[] genericArguments2 = typeof(T).GetGenericArguments();
			MethodInfo method2 = typeof(DataLoader).GetMethod("HashSetFromXml", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo2 = method2.MakeGenericMethod(genericArguments2);
			object[] parameters2 = new object[1] { itemRoot };
			object obj2 = methodInfo2.Invoke(null, parameters2);
			return (T)obj2;
		}
		if (typeof(T).HasGenericDefinition(typeof(Dictionary<, >)))
		{
			Type[] genericArguments3 = typeof(T).GetGenericArguments();
			MethodInfo method3 = typeof(DataLoader).GetMethod("DictionaryFromXml", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo3 = method3.MakeGenericMethod(genericArguments3);
			object[] parameters3 = new object[1] { itemRoot };
			object obj3 = methodInfo3.Invoke(null, parameters3);
			return (T)obj3;
		}
		XmlAttribute xmlAttribute = itemRoot.Attributes["ParentName"];
		T val;
		if (xmlAttribute != null)
		{
			if (nodesByName.ContainsKey(xmlAttribute.Value))
			{
				val = ItemFromXml<T>(nodesByName[xmlAttribute.Value], doPostLoad: false);
			}
			else
			{
				Debug.LogWarning("Failed to find inheritance parent named " + xmlAttribute.Value);
				val = new T();
			}
		}
		else
		{
			val = new T();
		}
		foreach (XmlNode childNode in itemRoot.ChildNodes)
		{
			BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo field = val.GetType().GetField(childNode.Name, bindingAttr);
			if (field == null)
			{
				Debug.LogWarning("Tried to load data " + childNode.OuterXml + " into type " + typeof(T).Name + " that does not have a matching member.");
			}
			else
			{
				Type[] typeArguments = new Type[1] { field.FieldType };
				MethodInfo method4 = typeof(DataLoader).GetMethod("ItemFromXml", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo methodInfo4 = method4.MakeGenericMethod(typeArguments);
				object value = methodInfo4.Invoke(null, new object[2] { childNode, doPostLoad });
				field.SetValue(val, value);
			}
		}
		if (doPostLoad)
		{
			val.GetType().GetMethod("PostLoad")?.Invoke(val, null);
		}
		return val;
	}

	private static List<T> ListFromXml<T>(XmlNode listRootNode) where T : new()
	{
		List<T> list = new List<T>();
		foreach (XmlNode childNode in listRootNode.ChildNodes)
		{
			list.Add(ItemFromXml<T>(childNode, doPostLoad: true));
		}
		return list;
	}

	private static HashSet<T> HashSetFromXml<T>(XmlNode hasherRootNode) where T : new()
	{
		HashSet<T> hashSet = new HashSet<T>();
		foreach (XmlNode childNode in hasherRootNode.ChildNodes)
		{
			hashSet.Add(ItemFromXml<T>(childNode, doPostLoad: true));
		}
		return hashSet;
	}

	private static Dictionary<K, V> DictionaryFromXml<K, V>(XmlNode listRootNode) where K : new()where V : new()
	{
		Dictionary<K, V> dictionary = new Dictionary<K, V>();
		foreach (XmlNode childNode in listRootNode.ChildNodes)
		{
			K key = ItemFromXml<K>(childNode["Key"], doPostLoad: true);
			V value = ItemFromXml<V>(childNode["Value"], doPostLoad: true);
			dictionary.Add(key, value);
		}
		return dictionary;
	}
}
