using System.Collections.Generic;
using UnityEngine;

public static class MaterialPool
{
	private class GeneratedMaterialDictionary
	{
		private Dictionary<int, Material> innerDict = new Dictionary<int, Material>();

		public Material this[Texture2D tex, Material srcMat] => MatOf(tex, srcMat);

		private int KeyOf(Texture2D tex, Material srcMat)
		{
			return tex.GetHashCode() * srcMat.GetHashCode();
		}

		public void Add(Texture2D tex, Material srcMat, Material outMat)
		{
			innerDict.Add(KeyOf(tex, srcMat), outMat);
		}

		public Material MatOf(Texture2D tex, Material srcMat)
		{
			int key = KeyOf(tex, srcMat);
			if (innerDict.TryGetValue(key, out var value))
			{
				return value;
			}
			return null;
		}
	}

	private class MaterialAtlas
	{
		protected Material[] SubMatList = new Material[16];

		public MaterialAtlas(Material newRootMat)
		{
			for (int i = 0; i < 16; i++)
			{
				Material material = new Material(newRootMat);
				material.SetTextureScale("_MainTex", new Vector2(125f / 512f, 125f / 512f));
				float x = (float)(i % 4) * 0.25f + 0.0029296875f;
				float y = (float)(i / 4) * 0.25f + 0.0029296875f;
				material.SetTextureOffset("_MainTex", new Vector2(x, y));
				SubMatList[i] = material;
			}
		}

		public Material SubMat(LinkDirections linkSet)
		{
			if ((int)linkSet >= SubMatList.Length)
			{
				Debug.LogWarning("Cannot get submat of index " + (int)linkSet + ": out of range.");
				return MatsSimple.BadMaterial;
			}
			return SubMatList[(int)linkSet];
		}
	}

	private static GeneratedMaterialDictionary matDict = new GeneratedMaterialDictionary();

	private static Dictionary<Material, MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlas>();

	private static readonly Material DefaultBaseMaterial = MatBases.Cutout;

	public static Material MatFrom(string texPath, bool reportFailure)
	{
		return MatFrom(Res.LoadTexture(texPath, reportFailure), DefaultBaseMaterial);
	}

	public static Material MatFrom(string texPath)
	{
		return MatFrom(Res.LoadTexture(texPath), DefaultBaseMaterial);
	}

	public static Material MatFrom(Texture2D srcTex)
	{
		return MatFrom(srcTex, DefaultBaseMaterial);
	}

	public static Material MatFrom(string texPath, Material baseMat)
	{
		return MatFrom(Res.LoadTexture(texPath), baseMat);
	}

	public static Material MatFrom(Texture2D srcTex, Material baseMat)
	{
		if (srcTex == null)
		{
			return MatsSimple.BadMaterial;
		}
		if (baseMat == null)
		{
			Debug.LogWarning("No source material with " + srcTex.name);
			return MatsSimple.BadMaterial;
		}
		Material material = matDict[srcTex, baseMat];
		if (material != null)
		{
			return material;
		}
		Material material2 = new Material(baseMat);
		material2.mainTexture = srcTex;
		material2.name = baseMat.name + "_" + srcTex.name;
		material2.renderQueue = baseMat.renderQueue;
		matDict.Add(srcTex, baseMat, material2);
		if (baseMat == MatBases.PlantCutout)
		{
			WindManager.Notify_PlantMaterialCreated(material2);
		}
		return material2;
	}

	public static IEnumerable<Material> MatsFromFolder(string folderPath, Material baseMat)
	{
		string wholePath = "Textures/" + folderPath;
		Object[] texList = Resources.LoadAll(wholePath, typeof(Texture2D));
		if (texList.Length == 0)
		{
			Debug.LogWarning("Asked for RandomMatFromTextureInFolder for an empty or nonexistent folder " + folderPath);
			yield break;
		}
		Object[] array = texList;
		foreach (Object obj in array)
		{
			yield return MatFrom((Texture2D)obj, baseMat);
		}
	}

	public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
	{
		if (!atlasDict.ContainsKey(mat))
		{
			atlasDict.Add(mat, new MaterialAtlas(mat));
		}
		return atlasDict[mat].SubMat(LinkSet);
	}
}
