using System.Collections.Generic;
using UnityEngine;

public class UnsetMesh
{
	public List<Vector3> verts = new List<Vector3>();

	public List<Vector2> uvs = new List<Vector2>();

	public List<List<int>> triangleSets = new List<List<int>>();

	public List<Color32> colors = new List<Color32>();

	public void Clear()
	{
		verts.Clear();
		colors.Clear();
		uvs.Clear();
		triangleSets.Clear();
	}

	public void AddTriangleSets(int count)
	{
		for (int i = 0; i < count; i++)
		{
			List<int> item = new List<int>();
			triangleSets.Add(item);
		}
	}

	public Mesh ToMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors32 = colors.ToArray();
		mesh.subMeshCount = triangleSets.Count;
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			mesh.SetTriangles(triangleSets[i].ToArray(), i);
		}
		return mesh;
	}
}
