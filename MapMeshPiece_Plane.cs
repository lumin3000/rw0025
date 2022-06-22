using System;
using System.Collections.Generic;
using UnityEngine;

public class MapMeshPiece_Plane : MapMeshPiece
{
	private Vector3 center;

	private Vector2 size;

	private Material mat;

	private float rot;

	public Vector2[] uvs = new Vector2[4];

	public Color32[] colors = new Color32[4];

	public override Material Mat => mat;

	public MapMeshPiece_Plane(Vector3 center, Vector2 size, Material mat, float rot)
		: this(center, size, mat, rot, UVflipped: false)
	{
	}

	public MapMeshPiece_Plane(Vector3 center, Vector2 size, Material mat, float rot, bool UVflipped)
	{
		this.center = center;
		this.size = size;
		this.mat = mat;
		this.rot = rot;
		if (!UVflipped)
		{
			ref Vector2 reference = ref uvs[0];
			reference = new Vector2(0f, 0f);
			ref Vector2 reference2 = ref uvs[1];
			reference2 = new Vector2(0f, 1f);
			ref Vector2 reference3 = ref uvs[2];
			reference3 = new Vector2(1f, 1f);
			ref Vector2 reference4 = ref uvs[3];
			reference4 = new Vector2(1f, 0f);
		}
		else
		{
			ref Vector2 reference5 = ref uvs[0];
			reference5 = new Vector2(1f, 0f);
			ref Vector2 reference6 = ref uvs[1];
			reference6 = new Vector2(1f, 1f);
			ref Vector2 reference7 = ref uvs[2];
			reference7 = new Vector2(0f, 1f);
			ref Vector2 reference8 = ref uvs[3];
			reference8 = new Vector2(0f, 0f);
		}
		for (int i = 0; i < 4; i++)
		{
			ref Color32 reference9 = ref colors[i];
			reference9 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public override void PrintOnto(MapSectionLayer_Pieces layer)
	{
		UnsetMesh unsetMesh = layer.unsetMesh;
		int count = unsetMesh.verts.Count;
		unsetMesh.verts.Add(new Vector3(-0.5f * size.x, 0f, -0.5f * size.y));
		unsetMesh.verts.Add(new Vector3(-0.5f * size.x, 0f, 0.5f * size.y));
		unsetMesh.verts.Add(new Vector3(0.5f * size.x, 0f, 0.5f * size.y));
		unsetMesh.verts.Add(new Vector3(0.5f * size.x, 0f, -0.5f * size.y));
		if (rot != 0f)
		{
			float num = rot * ((float)Math.PI / 180f);
			num *= -1f;
			for (int i = 0; i < 4; i++)
			{
				float x = unsetMesh.verts[count + i].x;
				float z = unsetMesh.verts[count + i].z;
				float num2 = Mathf.Cos(num);
				float num3 = Mathf.Sin(num);
				float x2 = x * num2 - z * num3;
				float z2 = x * num3 + z * num2;
				unsetMesh.verts[count + i] = new Vector3(x2, unsetMesh.verts[count + i].y, z2);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			List<Vector3> verts;
			List<Vector3> list = (verts = unsetMesh.verts);
			int index;
			int index2 = (index = count + j);
			Vector3 vector = verts[index];
			list[index2] = vector + center;
		}
		for (int k = 0; k < 4; k++)
		{
			unsetMesh.uvs.Add(uvs[k]);
		}
		for (int l = 0; l < 4; l++)
		{
			unsetMesh.colors.Add(colors[l]);
		}
		int index3 = layer.layerMats.IndexOf(Mat);
		List<int> list2 = unsetMesh.triangleSets[index3];
		list2.Add(count);
		list2.Add(count + 1);
		list2.Add(count + 2);
		list2.Add(count);
		list2.Add(count + 2);
		list2.Add(count + 3);
	}

	public override string ToString()
	{
		string text = "null";
		if (mat != null && mat.mainTexture != null)
		{
			text = mat.mainTexture.name;
		}
		return string.Concat("MapMeshPlane: center=", center, ", size=", size, ", mat=", text, ", rot=", rot);
	}
}
