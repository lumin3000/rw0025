using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class CodexDatabase
{
	public static CodexSection curSection;

	public static CodexArticle curArticle;

	private static readonly CodexArticle defaultArticle;

	public static List<CodexSection> sectionList;

	static CodexDatabase()
	{
		sectionList = new List<CodexSection>();
		LoadAllCodexData();
		defaultArticle = new CodexArticle("Codex");
		CodexContent_Text item = new CodexContent_Text("In this database, you can look up anything about RimWorld - from controls to explanations of game systems to listings of buildings and weapons.\n\nWarning: since the game is unfinished and changing rapidly, it may be out of date.");
		defaultArticle.contentList.Add(item);
		curArticle = defaultArticle;
	}

	private static void LoadAllCodexData()
	{
		object[] source = Resources.LoadAll("Text/Codex", typeof(TextAsset));
		foreach (TextAsset item in source.Cast<TextAsset>())
		{
			LoadCodexDataFromXml(item.text);
		}
		curSection = sectionList[0];
	}

	private static void LoadCodexDataFromXml(string xmlString)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlString);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/Codex");
		XmlNodeList xmlNodeList = xmlNode.SelectNodes("Section");
		foreach (XmlNode item2 in xmlNodeList)
		{
			CodexSection codexSection = new CodexSection(item2.Attributes.GetNamedItem("Title").Value);
			XmlNodeList xmlNodeList2 = item2.SelectNodes("Category");
			foreach (XmlNode item3 in xmlNodeList2)
			{
				CodexCategory codexCategory = new CodexCategory(item3.Attributes.GetNamedItem("Title").Value);
				XmlNodeList xmlNodeList3 = item3.SelectNodes("Article");
				foreach (XmlNode item4 in xmlNodeList3)
				{
					CodexArticle codexArticle = new CodexArticle(item4.Attributes.GetNamedItem("Title").Value);
					XmlNodeList childNodes = item4.ChildNodes;
					foreach (XmlNode item5 in childNodes)
					{
						CodexContent item = null;
						if (item5.Name == "p")
						{
							item = new CodexContent_Text(item5.InnerText);
						}
						if (item5.Name == "img")
						{
							item = new CodexContent_Image(item5.InnerText);
						}
						codexArticle.contentList.Add(item);
					}
					codexCategory.articleList.Add(codexArticle);
				}
				codexSection.categoryList.Add(codexCategory);
			}
			sectionList.Add(codexSection);
		}
	}

	public static void OpenPath(string path)
	{
		string[] pathAr = path.Split('/');
		curSection = sectionList.Where((CodexSection sect) => sect.title == pathAr[0]).FirstOrDefault();
		if (curSection == null)
		{
			Debug.LogError("Could not resolve section in codex path " + path);
			curSection = sectionList[0];
			return;
		}
		CodexCategory codexCategory = ((!(pathAr[1] != string.Empty)) ? curSection.categoryList.FirstOrDefault() : curSection.categoryList.Where((CodexCategory cat) => cat.articleList.Where((CodexArticle art) => art.title == pathAr[2]).Any()).FirstOrDefault());
		if (codexCategory == null)
		{
			Debug.LogError("Could not resolve category in codex path " + path);
			return;
		}
		codexCategory.isOpen = true;
		if (pathAr[2] != string.Empty)
		{
			curArticle = codexCategory.articleList.Where((CodexArticle art) => art.title == pathAr[2]).FirstOrDefault();
		}
		else
		{
			curArticle = codexCategory.articleList.FirstOrDefault();
		}
		if (curArticle == null)
		{
			Debug.LogError("Could not resolve article in codex path " + path);
			curArticle = defaultArticle;
		}
	}
}
