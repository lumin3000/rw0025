using System.Collections.Generic;
using UnityEngine;

public class LinkDrawer
{
	protected virtual Material AtlasMat(Thing parent)
	{
		return parent.def.drawMat;
	}

	protected Material LinkedDrawMatFrom(Thing parent, IntVec3 sq)
	{
		int num = 0;
		int num2 = 1;
		foreach (IntVec3 item in sq.AdjacentSquaresCardinal())
		{
			if (ShouldLinkWith(item, parent))
			{
				num += num2;
			}
			num2 *= 2;
		}
		LinkDirections linkSet = (LinkDirections)num;
		return MaterialPool.SubMaterialFromAtlas(AtlasMat(parent), linkSet);
	}

	public virtual bool ShouldLinkWith(IntVec3 sq, Thing parent)
	{
		if (!sq.InBounds())
		{
			return (parent.def.linkFlags & LinkFlags.MapEdge) != 0;
		}
		return (LinkGrid.LinkFlagsAt(sq) & parent.def.linkFlags) != 0;
	}

	public virtual IEnumerable<MapMeshPiece> GetMapMeshPieces(Thing parent)
	{
		yield return new MapMeshPiece_Plane(parent.TrueCenter(), parent.def.size.ToVector2(), LinkedDrawMatFrom(parent, parent.Position), 0f);
	}
}
