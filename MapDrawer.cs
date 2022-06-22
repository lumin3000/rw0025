using System;
using UnityEngine;

public class MapDrawer
{
	protected MapSection[,] sections;

	protected IntVec2 NumSects
	{
		get
		{
			IntVec2 result = default(IntVec2);
			result.x = (int)Math.Ceiling((double)Find.Map.Size.x / 17.0);
			result.z = (int)Math.Ceiling((double)Find.Map.Size.z / 17.0);
			return result;
		}
	}

	public void MapChanged(IntVec3 loc, MapChangeType changeType)
	{
		bool regenAdjacentSquares = changeType == MapChangeType.FogOfWar || changeType == MapChangeType.Blockers;
		bool regenAdjacentSections = changeType == MapChangeType.GroundGlow;
		MapChanged(loc, changeType, regenAdjacentSquares, regenAdjacentSections);
	}

	public void MapChanged(IntVec3 loc, MapChangeType changeType, bool regenAdjacentSquares, bool regenAdjacentSections)
	{
		if (!Find.Map.initialized)
		{
			return;
		}
		MapSection mapSection = SectionAt(loc);
		mapSection.changesThisFrame |= changeType;
		if (regenAdjacentSquares)
		{
			foreach (IntVec3 item in loc.AdjacentSquares8Way())
			{
				if (item.InBounds())
				{
					SectionAt(item).changesThisFrame |= changeType;
				}
			}
		}
		if (!regenAdjacentSections)
		{
			return;
		}
		IntVec2 intVec = SectionCoordsAt(loc);
		IntVec3[] adjacentSquares = Gen.AdjacentSquares;
		for (int i = 0; i < adjacentSquares.Length; i++)
		{
			IntVec3 intVec2 = adjacentSquares[i];
			IntVec2 intVec3 = intVec + new IntVec2(intVec2.x, intVec2.z);
			IntVec2 numSects = NumSects;
			if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= numSects.x - 1 && intVec3.z <= numSects.z - 1)
			{
				MapSection mapSection2 = sections[intVec3.x, intVec3.z];
				mapSection2.changesThisFrame |= changeType;
			}
		}
	}

	public void MapMeshDrawerUpdate_First()
	{
		IntRect currentViewRect = Find.CameraMap.CurrentViewRect;
		currentViewRect.ClipInsideMap();
		IntVec2 intVec = SectionCoordsAt(currentViewRect.BottomLeft);
		IntVec2 intVec2 = SectionCoordsAt(currentViewRect.TopRight);
		IntRect intRect = IntRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
		bool flag = false;
		foreach (IntVec3 item in intRect)
		{
			MapSection sect = sections[item.x, item.z];
			if (TryUpdateSection(sect))
			{
				flag = true;
			}
		}
		if (flag)
		{
			return;
		}
		for (int i = 0; i < NumSects.x; i++)
		{
			for (int j = 0; j < NumSects.z; j++)
			{
				if (TryUpdateSection(sections[i, j]))
				{
					return;
				}
			}
		}
	}

	private bool TryUpdateSection(MapSection sect)
	{
		if (sect.changesThisFrame == MapChangeType.None)
		{
			return false;
		}
		foreach (int value in Enum.GetValues(typeof(MapChangeType)))
		{
			if (value != 1 && ((uint)sect.changesThisFrame & (uint)value) != 0)
			{
				sect.RegenerateLayers((MapChangeType)value);
			}
		}
		sect.changesThisFrame = MapChangeType.None;
		return true;
	}

	public void DrawMapMesh()
	{
		MapSection[,] array = sections;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				MapSection mapSection = array[i, j];
				mapSection.DrawSection();
			}
		}
	}

	private IntVec2 SectionCoordsAt(IntVec3 loc)
	{
		return new IntVec2(Mathf.FloorToInt(loc.x / 17), Mathf.FloorToInt(loc.z / 17));
	}

	public MapSection SectionAt(IntVec3 loc)
	{
		IntVec2 intVec = SectionCoordsAt(loc);
		return sections[intVec.x, intVec.z];
	}

	public void RegenerateEverythingNow()
	{
		if (sections == null)
		{
			sections = new MapSection[NumSects.x, NumSects.z];
		}
		for (int i = 0; i < NumSects.x; i++)
		{
			for (int j = 0; j < NumSects.z; j++)
			{
				if (sections[i, j] == null)
				{
					sections[i, j] = new MapSection(new IntVec2(i, j));
				}
				sections[i, j].RegenerateAllLayers();
			}
		}
	}

	public void WholeMapChanged(MapChangeType change)
	{
		for (int i = 0; i < NumSects.x; i++)
		{
			for (int j = 0; j < NumSects.z; j++)
			{
				sections[i, j].changesThisFrame |= change;
			}
		}
	}
}
