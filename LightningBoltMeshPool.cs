using System.Collections.Generic;
using UnityEngine;

public static class LightningBoltMeshPool
{
	private const int NumBoltMeshesMax = 20;

	private static List<Mesh> boltMeshes = new List<Mesh>();

	public static Mesh RandomBoltMesh
	{
		get
		{
			if (boltMeshes.Count < 20)
			{
				Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
				boltMeshes.Add(mesh);
				return mesh;
			}
			return boltMeshes.RandomElement();
		}
	}
}
