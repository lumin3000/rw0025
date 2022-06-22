using System.Collections.Generic;

public class MapSectionLayer_ThingsGeneral : MapSectionLayer_Things
{
	public MapSectionLayer_ThingsGeneral(MapSection section)
		: base(section)
	{
		relevantChangeTypes.Add(MapChangeType.Things);
		requireAddToMapMesh = true;
	}

	protected override IEnumerable<MapMeshPiece> MeshPiecesFrom(Thing t)
	{
		return t.EmitMapMeshPieces();
	}
}
