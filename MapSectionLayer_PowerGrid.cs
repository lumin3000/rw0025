using System.Collections.Generic;

public class MapSectionLayer_PowerGrid : MapSectionLayer_Things
{
	public MapSectionLayer_PowerGrid(MapSection section)
		: base(section)
	{
		requireAddToMapMesh = false;
		relevantChangeTypes.Add(MapChangeType.PowerGrid);
	}

	public override void DrawLayer()
	{
		if (OverlayDrawHandler.ShouldDrawPowerGrid)
		{
			base.DrawLayer();
		}
	}

	protected override IEnumerable<MapMeshPiece> MeshPiecesFrom(Thing t)
	{
		Building b = t as Building;
		if (b == null)
		{
			yield break;
		}
		foreach (MapMeshPiece item in b.EmitMapMeshPiecesForPowerGrid())
		{
			yield return item;
		}
	}
}
