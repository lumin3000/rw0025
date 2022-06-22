using System.Collections.Generic;
using UnityEngine;

public abstract class MapSectionLayer
{
	protected MapSection section;

	public List<MapChangeType> relevantChangeTypes = new List<MapChangeType>();

	public Mesh layerMesh = new Mesh();

	public List<Material> layerMats = new List<Material>();

	public MapSectionLayer(MapSection Section)
	{
		section = Section;
	}

	public virtual void DrawLayer()
	{
		for (int i = 0; i < layerMats.Count; i++)
		{
			Graphics.DrawMesh(layerMesh, new Vector3(0f, 0f, 0f), Quaternion.identity, layerMats[i], 0, null, i);
		}
	}

	public abstract void RegenerateMesh();

	protected void ResetLayerMesh()
	{
		Object.Destroy(layerMesh);
		layerMesh = new Mesh();
	}
}
