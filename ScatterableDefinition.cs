using System.Collections.Generic;
using UnityEngine;

public class ScatterableDefinition
{
	public string texturePath = string.Empty;

	public List<string> relevantTerrains = new List<string>();

	public float minSize;

	public float maxSize;

	public float selectionWeight = 100f;

	public List<ScatterType> scatterTypes = new List<ScatterType>();

	public Material mat;

	public void PostLoad()
	{
		mat = MaterialPool.MatFrom(texturePath, MatBases.Transparent);
	}
}
