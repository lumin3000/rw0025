using System;
using System.Collections.Generic;
using UnityEngine;

public static class LightningBoltMeshMaker
{
	private const float LightningHeight = 200f;

	private const float LightningRootXVar = 50f;

	private const float VertexInterval = 0.25f;

	private const float MeshWidth = 2f;

	private const float UVIntervalY = 0.04f;

	private const float PerturbAmpMacro = 12f;

	private const float PerturbFreqMacro = 100f;

	private const float PerturbAmpMicro = 4f;

	private const float PerturbFreqMicro = 10f;

	private static List<Vector2> verts2D;

	private static Vector2 lightningTop;

	public static Mesh NewBoltMesh()
	{
		lightningTop = new Vector2(UnityEngine.Random.Range(-50f, 50f), 200f);
		MakeVerticesBase();
		PeturbVerticesRandomly();
		DoubleVertices();
		return MeshFromVerts();
	}

	private static void MakeVerticesBase()
	{
		int num = (int)Math.Ceiling((Vector2.zero - lightningTop).magnitude / 0.25f);
		Vector2 vector = lightningTop / num;
		verts2D = new List<Vector2>();
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < num; i++)
		{
			verts2D.Add(zero);
			zero += vector;
		}
	}

	private static void PeturbVerticesRandomly()
	{
		PerlinArb perlinArb = new PerlinArb();
		PerlinArb perlinArb2 = new PerlinArb();
		List<Vector2> list = verts2D.ListFullCopy();
		verts2D.Clear();
		int num = 0;
		foreach (Vector2 item2 in list)
		{
			float num2 = 0f;
			num2 += 12f * perlinArb.Noise((float)num / 100f);
			num2 += 4f * perlinArb2.Noise((float)num / 10f);
			Vector2 item = item2 + num2 * Vector2.right;
			verts2D.Add(item);
			num++;
		}
	}

	private static void DoubleVertices()
	{
		List<Vector2> list = verts2D.ListFullCopy();
		Vector3 vector = default(Vector3);
		Vector2 vector2 = default(Vector2);
		verts2D.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			if (i <= list.Count - 2)
			{
				vector = Quaternion.AngleAxis(90f, Vector3.up) * (list[i] - list[i + 1]);
				vector2 = new Vector2(vector.y, vector.z);
				vector2.Normalize();
			}
			Vector2 item = list[i] - 1f * vector2;
			Vector2 item2 = list[i] + 1f * vector2;
			verts2D.Add(item);
			verts2D.Add(item2);
		}
	}

	private static Mesh MeshFromVerts()
	{
		Vector3[] array = new Vector3[verts2D.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ref Vector3 reference = ref array[i];
			reference = new Vector3(verts2D[i].x, 0f, verts2D[i].y);
		}
		float num = 0f;
		Vector2[] array2 = new Vector2[verts2D.Count];
		for (int j = 0; j < verts2D.Count; j += 2)
		{
			ref Vector2 reference2 = ref array2[j];
			reference2 = new Vector2(0f, num);
			ref Vector2 reference3 = ref array2[j + 1];
			reference3 = new Vector2(1f, num);
			num += 0.04f;
		}
		int[] array3 = new int[verts2D.Count * 3];
		for (int k = 0; k < verts2D.Count - 2; k += 2)
		{
			int num2 = k * 3;
			array3[num2] = k;
			array3[num2 + 1] = k + 1;
			array3[num2 + 2] = k + 2;
			array3[num2 + 3] = k + 2;
			array3[num2 + 4] = k + 1;
			array3[num2 + 5] = k + 3;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
		return mesh;
	}
}
