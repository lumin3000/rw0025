using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MaterialLoader
{
	public static List<Material> MatsFromTexturesInFolder(string dirPath)
	{
		string path = "Textures/" + dirPath;
		return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList();
	}

	public static Material MatWithEnding(string dirPath, string ending)
	{
		Material material = (from mat in MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault();
		if (material == null)
		{
			Debug.LogWarning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
			return MatsSimple.BadMaterial;
		}
		return material;
	}
}
