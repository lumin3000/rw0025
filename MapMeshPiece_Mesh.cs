using System.Collections.Generic;
using UnityEngine;

public class MapMeshPiece_Mesh : MapMeshPiece
{
	private Vector3 center;

	private Mesh mesh;

	private Material mat;

	public override Material Mat => mat;

	public MapMeshPiece_Mesh(Vector3 center, Mesh mesh, Material mat)
	{
		this.center = center;
		this.mesh = mesh;
		this.mat = mat;
	}

	public override void PrintOnto(MapSectionLayer_Pieces layer)
	{
		UnsetMesh unsetMesh = layer.unsetMesh;
		int count = unsetMesh.verts.Count;
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			unsetMesh.verts.Add(mesh.vertices[i] + center);
			if (mesh.colors32.Length > i)
			{
				unsetMesh.colors.Add(mesh.colors32[i]);
			}
			else
			{
				unsetMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
			if (mesh.uv.Length > i)
			{
				unsetMesh.uvs.Add(mesh.uv[i]);
			}
			else
			{
				unsetMesh.uvs.Add(Vector2.zero);
			}
		}
		int index = layer.layerMats.IndexOf(Mat);
		List<int> list = unsetMesh.triangleSets[index];
		int[] triangles = mesh.triangles;
		foreach (int num in triangles)
		{
			list.Add(count + num);
		}
	}

	public override string ToString()
	{
		return string.Concat("MapMeshPiece_Mesh[center=", center, ", meshverts=", mesh.vertexCount, ", mat=", mat.name, "]");
	}
}
