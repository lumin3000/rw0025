using System.Collections.Generic;
using UnityEngine;

public abstract class MapSectionLayer_Pieces : MapSectionLayer
{
	public List<MapMeshPiece> meshPieces = new List<MapMeshPiece>();

	public UnsetMesh unsetMesh = new UnsetMesh();

	public MapSectionLayer_Pieces(MapSection section)
		: base(section)
	{
	}

	public override void RegenerateMesh()
	{
		meshPieces.Clear();
		CollectMeshPieces();
		MakeMeshFromPieces();
	}

	protected abstract void CollectMeshPieces();

	private void MakeMeshFromPieces()
	{
		layerMats.Clear();
		foreach (MapMeshPiece meshPiece in meshPieces)
		{
			if (!layerMats.Contains(meshPiece.Mat))
			{
				layerMats.Add(meshPiece.Mat);
			}
		}
		unsetMesh.Clear();
		unsetMesh.AddTriangleSets(layerMats.Count);
		foreach (MapMeshPiece meshPiece2 in meshPieces)
		{
			meshPiece2.PrintOnto(this);
		}
		Object.Destroy(layerMesh);
		layerMesh = unsetMesh.ToMesh();
	}
}
