using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GenFile
{
	public static string GetTextWithoutBOM(TextAsset textAsset)
	{
		MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
		StreamReader streamReader = new StreamReader(memoryStream, detectEncodingFromByteOrderMarks: true);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		memoryStream.Close();
		return result;
	}

	public static IEnumerable<string> StringsFromFile(string assetPath)
	{
		TextAsset namesAsset = (TextAsset)Resources.Load("Text/" + assetPath);
		string names = namesAsset.text;
		string[] separators = new string[2] { "\r\n", "\n" };
		string[] array = names.Split(separators, StringSplitOptions.None);
		foreach (string str in array)
		{
			if (!str.Contains("//") && str.Length != 0)
			{
				yield return str;
			}
		}
	}
}
