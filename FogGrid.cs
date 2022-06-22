public class FogGrid : Saveable
{
	public bool[,] fogGrid;

	public void ExposeData()
	{
		GridSaveUtility.ExposeBoolGrid(ref fogGrid, "Fog");
	}

	public void RemakeEntireFogGrid()
	{
		if (fogGrid == null)
		{
			fogGrid = new bool[Find.Map.Size.x, Find.Map.Size.z];
		}
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			fogGrid[allSquare.x, allSquare.z] = true;
		}
		IntVec3 playerStartSpot = Genner_PlayerStuff.PlayerStartSpot;
		FogFloodFiller.FloodUnfogQueue(playerStartSpot);
	}

	public void Notify_FogBlockerDestroyed(IntVec3 loc)
	{
		if (!Find.Map.initialized)
		{
			return;
		}
		Unfog(loc);
		bool flag = false;
		foreach (IntVec3 item in loc.AdjacentSquares8Way())
		{
			if (item.InBounds() && item.IsFogged())
			{
				Thing thing = Find.Grids.BlockerAt(item);
				if (thing == null || !thing.def.makeFog)
				{
					flag = true;
					FogFloodFiller.FloodUnfogRecursive(item);
				}
				Unfog(item);
			}
		}
		if (flag)
		{
			Find.LetterStack.ReceiveLetter(new Letter("A new area has been revealed.", loc));
		}
	}

	public bool ShouldBeFogged(IntVec3 loc)
	{
		Thing thing = Find.Grids.BlockerAt(loc);
		if (thing == null || !thing.def.makeFog)
		{
			return false;
		}
		foreach (IntVec3 item in loc.AdjacentSquares8Way())
		{
			if (item.InBounds())
			{
				Thing thing2 = Find.Grids.BlockerAt(item);
				if (thing2 == null || !thing2.def.makeFog)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void Unfog(IntVec3 sq)
	{
		if (fogGrid[sq.x, sq.z])
		{
			fogGrid[sq.x, sq.z] = false;
			if (Find.Map.initialized)
			{
				Find.Map.mapDrawer.MapChanged(sq, MapChangeType.FogOfWar);
			}
			Designation designation = Find.DesignationManager.DesignationAt(sq, DesignationType.Mine);
			if (designation != null && MineUtility.MineableInSquare(sq) == null)
			{
				designation.Delete();
			}
		}
	}

	public bool IsFogged(IntVec3 Sq)
	{
		if (!DebugSettings.drawFog)
		{
			return false;
		}
		if (!Sq.InBounds())
		{
			return false;
		}
		return fogGrid[Sq.x, Sq.z];
	}

	public void ClearAllFog()
	{
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.z; j++)
			{
				Unfog(new IntVec3(i, 0, j));
			}
		}
	}

	public void ClearFogCircle(IntVec3 Center, float Radius)
	{
	}
}
