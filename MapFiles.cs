using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MapFiles
{
	public const string MapFileExtension = ".rim";

	public static IEnumerable<FileInfo> AllMapFiles => from f in new DirectoryInfo(Application.persistentDataPath).GetFiles()
		where f.Extension == ".rim"
		orderby f.LastWriteTime descending
		select f;

	public static string FilePathForMap(string mapName)
	{
		return Application.persistentDataPath + Path.DirectorySeparatorChar + mapName + ".rim";
	}

	public static FileInfo FileInfoForMap(string mapName)
	{
		return new FileInfo(FilePathForMap(mapName));
	}

	public static bool IsAutoSave(string mapName)
	{
		if (mapName.Length < 8)
		{
			return false;
		}
		return mapName.Substring(0, 8) == "Autosave";
	}

	public static bool HaveMapNamed(string mapName)
	{
		foreach (string item in AllMapFiles.Select((FileInfo f) => Path.GetFileNameWithoutExtension(f.Name)))
		{
			if (item == mapName)
			{
				return true;
			}
		}
		return false;
	}

	public static string UnusedDefaultName()
	{
		string empty = string.Empty;
		int num = 1;
		do
		{
			empty = Find.ColonyInfo.ColonyName + num;
			num++;
		}
		while (HaveMapNamed(empty));
		return empty;
	}
}
