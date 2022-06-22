using UnityEngine;

public static class MatLoader
{
	public static Material LoadMat(string matPath)
	{
		Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
		if (material == null)
		{
			Debug.LogWarning("Could not load material " + matPath);
		}
		return material;
	}
}
