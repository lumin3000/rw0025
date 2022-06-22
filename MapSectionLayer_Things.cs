using System.Collections.Generic;

public abstract class MapSectionLayer_Things : MapSectionLayer_Pieces
{
	protected bool requireAddToMapMesh;

	private static HashSet<Thing> bigThingsAddedHash = new HashSet<Thing>();

	public MapSectionLayer_Things(MapSection section)
		: base(section)
	{
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawThingsPrinted)
		{
			base.DrawLayer();
		}
	}

	protected override void CollectMeshPieces()
	{
		bigThingsAddedHash.Clear();
		IntRect mapRect = section.MapRect;
		foreach (IntVec3 item in mapRect)
		{
			foreach (Thing item2 in Find.Grids.ThingsAt(item))
			{
				if ((!item2.def.addToMapMesh && requireAddToMapMesh) || ((item2.def.size.x > 1 || item2.def.size.z > 1) && (bigThingsAddedHash.Contains(item2) || !mapRect.Contains(item2.Position))))
				{
					continue;
				}
				if (item2.def.size.x > 1 || item2.def.size.z > 1)
				{
					bigThingsAddedHash.Add(item2);
				}
				foreach (MapMeshPiece item3 in MeshPiecesFrom(item2))
				{
					meshPieces.Add(item3);
				}
			}
		}
	}

	protected abstract IEnumerable<MapMeshPiece> MeshPiecesFrom(Thing t);
}
