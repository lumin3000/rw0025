using System.Collections.Generic;
using UnityEngine;

internal static class MeshMakerPlanes
{
	private const float BackLiftAmount = 0.027999999f;

	private const float TwistAmount = 0.016f;

	public static Mesh NewPlaneMesh(float size)
	{
		return NewPlaneMesh(size, flipped: false);
	}

	public static Mesh NewPlaneMesh(float size, bool flipped)
	{
		return NewPlaneMesh(size, flipped, backLift: false);
	}

	public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
	{
		return NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist: false);
	}

	public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
	{
		return NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
	}

	public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
	{
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		ref Vector3 reference = ref array[0];
		reference = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
		ref Vector3 reference2 = ref array[1];
		reference2 = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
		ref Vector3 reference3 = ref array[2];
		reference3 = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
		ref Vector3 reference4 = ref array[3];
		reference4 = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
		if (backLift)
		{
			array[1].y = 0.027999999f;
			array[2].y = 0.027999999f;
			array[3].y = 0.0112f;
		}
		if (twist)
		{
			array[0].y = 0.016f;
			array[1].y = 0.008f;
			array[2].y = 0f;
			array[3].y = 0.008f;
		}
		if (!flipped)
		{
			ref Vector2 reference5 = ref array2[0];
			reference5 = new Vector2(0f, 0f);
			ref Vector2 reference6 = ref array2[1];
			reference6 = new Vector2(0f, 1f);
			ref Vector2 reference7 = ref array2[2];
			reference7 = new Vector2(1f, 1f);
			ref Vector2 reference8 = ref array2[3];
			reference8 = new Vector2(1f, 0f);
		}
		else
		{
			ref Vector2 reference9 = ref array2[0];
			reference9 = new Vector2(1f, 0f);
			ref Vector2 reference10 = ref array2[1];
			reference10 = new Vector2(1f, 1f);
			ref Vector2 reference11 = ref array2[2];
			reference11 = new Vector2(0f, 1f);
			ref Vector2 reference12 = ref array2[3];
			reference12 = new Vector2(0f, 0f);
		}
		array3[0] = 0;
		array3[1] = 1;
		array3[2] = 2;
		array3[3] = 0;
		array3[4] = 2;
		array3[5] = 3;
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.SetTriangles(array3, 0);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Mesh NewWholeMapPlane()
	{
		Mesh mesh = NewPlaneMesh(2000f, flipped: false, backLift: false);
		Vector2[] array = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			ref Vector2 reference = ref array[i];
			reference = mesh.uv[i] * 200f;
		}
		mesh.uv = array;
		return mesh;
	}

	public static Mesh NewShadowMesh(float baseEdgeLength, float tallness)
	{
		return NewShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
	}

	public static Mesh NewShadowMesh(float baseWidth, float baseHeight, float tallness)
	{
		Color32 item = new Color32(0, 0, 0, 0);
		Color32 item2 = new Color32(byte.MaxValue, 0, 0, (byte)(255f * tallness));
		float num = baseWidth / 2f;
		float num2 = baseHeight / 2f;
		List<Vector3> list = new List<Vector3>();
		List<Color32> list2 = new List<Color32>();
		List<int> list3 = new List<int>();
		list.Add(new Vector3(0f - num, 0f, 0f - num2));
		list2.Add(item);
		list.Add(new Vector3(0f - num, 0f, num2));
		list2.Add(item);
		list.Add(new Vector3(num, 0f, num2));
		list2.Add(item);
		list.Add(new Vector3(num, 0f, 0f - num2));
		list2.Add(item);
		list3.Add(0);
		list3.Add(1);
		list3.Add(2);
		list3.Add(0);
		list3.Add(2);
		list3.Add(3);
		int count = list.Count;
		list.Add(new Vector3(0f - num, 0f, 0f - num2));
		list2.Add(item2);
		list.Add(new Vector3(0f - num, 0f, num2));
		list2.Add(item2);
		list3.Add(0);
		list3.Add(count);
		list3.Add(count + 1);
		list3.Add(0);
		list3.Add(count + 1);
		list3.Add(1);
		int count2 = list.Count;
		list.Add(new Vector3(num, 0f, num2));
		list2.Add(item2);
		list.Add(new Vector3(num, 0f, 0f - num2));
		list2.Add(item2);
		list3.Add(2);
		list3.Add(count2);
		list3.Add(count2 + 1);
		list3.Add(count2 + 1);
		list3.Add(3);
		list3.Add(2);
		int count3 = list.Count;
		list.Add(new Vector3(0f - num, 0f, 0f - num2));
		list2.Add(item2);
		list.Add(new Vector3(num, 0f, 0f - num2));
		list2.Add(item2);
		list3.Add(0);
		list3.Add(3);
		list3.Add(count3);
		list3.Add(3);
		list3.Add(count3 + 1);
		list3.Add(count3);
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.colors32 = list2.ToArray();
		mesh.triangles = list3.ToArray();
		return mesh;
	}
}
