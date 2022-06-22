public class RoofGrid : Saveable
{
	public EntityType[,] roofGrid;

	public RoofGrid()
	{
		ResetGrids();
	}

	public void ResetGrids()
	{
		roofGrid = new EntityType[Find.Map.Size.x, Find.Map.Size.z];
	}

	public void ExposeData()
	{
		string value = string.Empty;
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			RoofMaker.InstantFinishAll();
			value = GridSaveUtility.CompressedStringForByteGrid((IntVec3 loc) => (byte)roofGrid[loc.x, loc.z]);
		}
		Scribe.LookField(ref value, "Roofs");
		if (Scribe.mode != LoadSaveMode.LoadingVars)
		{
			return;
		}
		foreach (GridSaveUtility.LoadedGridByte item in GridSaveUtility.ThingsFromThingTypeGrid(value))
		{
			SetSquareRoofed(item.pos, (EntityType)item.val);
		}
	}

	public bool SquareIsRoofed(IntVec3 sq)
	{
		return roofGrid[sq.x, sq.z] != EntityType.Undefined;
	}

	public bool Roofed(IntVec3 sq)
	{
		return RoofDefAt(sq) != null;
	}

	public RoofDefinition RoofDefAt(IntVec3 sq)
	{
		return RoofDefDatabase.RoofDefOfType(roofGrid[sq.x, sq.z]);
	}

	public void SetSquareRoofed(IntVec3 sq, EntityType newVal)
	{
		if (roofGrid[sq.x, sq.z] != newVal)
		{
			roofGrid[sq.x, sq.z] = newVal;
			Find.GlowGrid.MarkGlowGridDirty(sq);
			Find.MapDrawer.MapChanged(sq, MapChangeType.Roofs);
		}
	}

	public void DebugDrawRoots()
	{
		if (!DebugSettings.drawRoofs)
		{
			return;
		}
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			if (SquareIsRoofed(allSquare))
			{
				DebugRender.RenderSquare(allSquare);
			}
		}
	}
}
