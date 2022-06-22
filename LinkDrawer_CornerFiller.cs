using System.Collections.Generic;
using UnityEngine;

public class LinkDrawer_CornerFiller : LinkDrawer
{
	private const float ShiftUp = 0.07f;

	private const float CoverSize = 0.45f;

	private static readonly float CoverSizeCornerCorner = new Vector2(0.45f, 0.45f).magnitude;

	private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

	private static readonly float CoverOffsetDist = DistCenterCorner - CoverSizeCornerCorner * 0.5f;

	public override IEnumerable<MapMeshPiece> GetMapMeshPieces(Thing parent)
	{
		foreach (MapMeshPiece mapMeshPiece in base.GetMapMeshPieces(parent))
		{
			yield return mapMeshPiece;
		}
		IntVec3 parentPos = parent.Position;
		for (int i = 0; i < 4; i++)
		{
			IntVec3 neighSq = parent.Position + Gen.CornerDirectionsAround[i];
			if (ShouldLinkWith(neighSq, parent) && (i != 0 || (ShouldLinkWith(parentPos + IntVec3.west, parent) && ShouldLinkWith(parentPos + IntVec3.south, parent))) && (i != 1 || (ShouldLinkWith(parentPos + IntVec3.west, parent) && ShouldLinkWith(parentPos + IntVec3.north, parent))) && (i != 2 || (ShouldLinkWith(parentPos + IntVec3.east, parent) && ShouldLinkWith(parentPos + IntVec3.north, parent))) && (i != 3 || (ShouldLinkWith(parentPos + IntVec3.east, parent) && ShouldLinkWith(parentPos + IntVec3.south, parent))))
			{
				Vector3 pos = parent.DrawPos + Gen.CornerDirectionsAround[i].ToVector3().normalized * CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.07f);
				MapMeshPiece_Plane piece = new MapMeshPiece_Plane(pos, new Vector2(0.45f, 0.45f), LinkedDrawMatFrom(parent, parent.Position), 0f);
				ref Vector2 reference = ref piece.uvs[0];
				reference = new Vector2(0.5f, 0.6f);
				ref Vector2 reference2 = ref piece.uvs[1];
				reference2 = new Vector2(0.5f, 0.6f);
				ref Vector2 reference3 = ref piece.uvs[2];
				reference3 = new Vector2(0.5f, 0.6f);
				ref Vector2 reference4 = ref piece.uvs[3];
				reference4 = new Vector2(0.5f, 0.6f);
				yield return piece;
			}
		}
	}
}
