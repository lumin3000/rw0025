using System.Collections.Generic;
using UnityEngine;

public class LinkDrawer_Transmitter : LinkDrawer
{
	public override bool ShouldLinkWith(IntVec3 sq, Thing parent)
	{
		if (!sq.InBounds())
		{
			return false;
		}
		if (base.ShouldLinkWith(sq, parent) || PowerNetGrid.TransmittedPowerNetAt(sq) != null)
		{
			return true;
		}
		return false;
	}

	public override IEnumerable<MapMeshPiece> GetMapMeshPieces(Thing parent)
	{
		foreach (MapMeshPiece mapMeshPiece in base.GetMapMeshPieces(parent))
		{
			yield return mapMeshPiece;
		}
		int i = 0;
		foreach (IntVec3 neigh in parent.Position.AdjacentSquaresCardinal())
		{
			if (neigh.InBounds())
			{
				Thing t = Find.Grids.BlockerAt(neigh);
				if (t != null && t.def.transmitsPower && t.def.linkDrawer == null)
				{
					LinkDirections linkDir = ((i != 0 && i != 2) ? (LinkDirections.Right | LinkDirections.Left) : (LinkDirections.Up | LinkDirections.Down));
					yield return new MapMeshPiece_Plane(mat: MaterialPool.SubMaterialFromAtlas(parent.def.drawMat, linkDir), center: neigh.ToVector3ShiftedWithAltitude(parent.def.altitude), size: Vector2.one, rot: 0f);
				}
				i++;
			}
		}
	}
}
