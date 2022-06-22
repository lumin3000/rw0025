using System.Collections.Generic;
using UnityEngine;

public class MapSectionLayer_TerrainScatter : MapSectionLayer_Pieces
{
	private class Scatterable
	{
		public ScatterableDefinition def;

		public Vector3 loc;

		public float size;

		public float rotation;

		public bool IsOnValidTerrain => def.scatterTypes.Contains(Find.TerrainGrid.TerrainAt(loc.ToIntVec3()).scatterType);

		public Scatterable(ScatterableDefinition def, Vector3 loc)
		{
			this.def = def;
			this.loc = loc;
			size = Random.Range(def.minSize, def.maxSize);
			rotation = Random.Range(0f, 360f);
		}

		public MapMeshPiece_Plane GetMeshPiece()
		{
			return new MapMeshPiece_Plane(loc, Vector2.one * size, def.mat, rotation);
		}
	}

	private List<Scatterable> scatsList = new List<Scatterable>();

	public MapSectionLayer_TerrainScatter(MapSection section)
		: base(section)
	{
		relevantChangeTypes.Add(MapChangeType.Terrain);
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawTerrain)
		{
			base.DrawLayer();
		}
	}

	public override void RegenerateMesh()
	{
		if (DebugSettings.drawTerrain)
		{
			base.RegenerateMesh();
		}
	}

	protected override void CollectMeshPieces()
	{
		scatsList.RemoveAll((Scatterable scat) => !scat.IsOnValidTerrain);
		int num = 0;
		TerrainDefinition[,,] terrainGrid = Find.TerrainGrid.terrainGrid;
		foreach (IntVec3 item in section.MapRect)
		{
			if (terrainGrid[item.x, item.y, item.z].scatterType != 0)
			{
				num++;
			}
		}
		num /= 40;
		int num2 = 0;
		while (scatsList.Count < num && num2 < 200)
		{
			num2++;
			Vector3 vector = new Vector3((float)section.botLeft.x + Random.value * 17f, 0f, (float)section.botLeft.z + Random.value * 17f);
			IntVec3 intVec = vector.ToIntVec3();
			if (!intVec.InBounds())
			{
				continue;
			}
			ScatterType scatterType = Find.TerrainGrid.TerrainAt(intVec).scatterType;
			if (scatterType != 0)
			{
				int num3 = 0;
				ScatterableDefinition scatterableDefinition;
				do
				{
					scatterableDefinition = ScatterableDatabase.RandomScatterable();
				}
				while (!scatterableDefinition.scatterTypes.Contains(scatterType) && num3 < 10);
				Scatterable scatterable = new Scatterable(scatterableDefinition, vector);
				scatsList.Add(scatterable);
				meshPieces.Add(scatterable.GetMeshPiece());
			}
		}
		foreach (Scatterable scats in scatsList)
		{
			meshPieces.Add(scats.GetMeshPiece());
		}
	}
}
