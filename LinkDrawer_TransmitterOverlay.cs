using System.Collections.Generic;
using UnityEngine;

public class LinkDrawer_TransmitterOverlay : LinkDrawer
{
	protected override Material AtlasMat(Thing parent)
	{
		return PowerOverlayMats.MatTransmitterAtlas;
	}

	public override bool ShouldLinkWith(IntVec3 sq, Thing parent)
	{
		if (!sq.InBounds())
		{
			return false;
		}
		if (PowerNetGrid.TransmittedPowerNetAt(sq) != null)
		{
			return true;
		}
		return false;
	}

	public IEnumerable<MapMeshPiece> GetMapMeshPieces(Thing parent, IntVec3 sq)
	{
		Vector3 loc = sq.ToVector3ShiftedWithAltitude(AltitudeLayer.WorldDataOverlay);
		yield return new MapMeshPiece_Plane(loc, new Vector2(1f, 1f), LinkedDrawMatFrom(parent, sq), 0f);
	}
}
