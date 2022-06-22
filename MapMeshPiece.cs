using UnityEngine;

public abstract class MapMeshPiece
{
	public abstract Material Mat { get; }

	public abstract void PrintOnto(MapSectionLayer_Pieces layer);
}
