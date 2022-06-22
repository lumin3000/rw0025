using System.Collections.Generic;

public class HomeZoneGrid : Saveable
{
	public bool[,] homeGrid;

	public IEnumerable<Thing> AllCleanableFilth
	{
		get
		{
			foreach (Filth f in FilthList.allFilth)
			{
				if (ShouldClean(f.Position))
				{
					yield return f;
				}
			}
		}
	}

	public HomeZoneGrid()
	{
		if (homeGrid == null)
		{
			homeGrid = new bool[Find.Map.Size.x, Find.Map.Size.z];
		}
	}

	public void ExposeData()
	{
		GridSaveUtility.ExposeBoolGrid(ref homeGrid, "CleanGrid");
	}

	public void SetShouldClean(IntVec3 sq, bool value)
	{
		if (sq.InBounds() && homeGrid[sq.x, sq.z] != value)
		{
			Find.MapDrawer.MapChanged(sq, MapChangeType.HomeZone);
			homeGrid[sq.x, sq.z] = value;
		}
	}

	public bool ShouldClean(IntVec3 sq)
	{
		if (!sq.InBounds())
		{
			return false;
		}
		return homeGrid[sq.x, sq.z];
	}
}
