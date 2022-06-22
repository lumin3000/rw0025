using System;
using System.Collections.Generic;
using UnityEngine;

public static class MeshMakerCircles
{
	public static Mesh MakePieMesh(int DegreesWide)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(0f, 0f));
		for (int i = 0; i < DegreesWide; i++)
		{
			float num = (float)i / 180f * (float)Math.PI;
			Vector2 item = new Vector2(0f, 0f);
			item.x = (float)(0.550000011920929 * Math.Cos(num));
			item.y = (float)(0.550000011920929 * Math.Sin(num));
			list.Add(item);
		}
		Vector3[] array = new Vector3[list.Count];
		for (int j = 0; j < array.Length; j++)
		{
			ref Vector3 reference = ref array[j];
			reference = new Vector3(list[j].x, 0f, list[j].y);
		}
		Triangulator triangulator = new Triangulator(list.ToArray());
		int[] triangles = triangulator.Triangulate();
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = new Vector2[list.Count];
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Mesh MakeCircleMesh(float radius)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(0f, 0f));
		for (int i = 0; i < 360; i += 4)
		{
			float num = (float)i / 180f * (float)Math.PI;
			Vector2 item = new Vector2(0f, 0f);
			item.x = (float)((double)radius * Math.Cos(num));
			item.y = (float)((double)radius * Math.Sin(num));
			list.Add(item);
		}
		Vector3[] array = new Vector3[list.Count];
		for (int j = 0; j < array.Length; j++)
		{
			ref Vector3 reference = ref array[j];
			reference = new Vector3(list[j].x, 0f, list[j].y);
		}
		Triangulator triangulator = new Triangulator(list.ToArray());
		int[] triangles = triangulator.Triangulate();
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = new Vector2[list.Count];
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}
}
