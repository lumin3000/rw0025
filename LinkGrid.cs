public static class LinkGrid
{
	private static LinkFlags[,,] linkGrid;

	public static void ResetStaticData()
	{
		linkGrid = new LinkFlags[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
	}

	public static LinkFlags LinkFlagsAt(IntVec3 sq)
	{
		return linkGrid[sq.x, sq.y, sq.z];
	}

	public static void Notify_LinkerCreatedOrDestroyed(Thing linker)
	{
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(linker))
		{
			LinkFlags linkFlags = LinkFlags.None;
			foreach (Thing item2 in Find.Grids.ThingsAt(item))
			{
				linkFlags |= item2.def.linkFlags;
			}
			linkGrid[item.x, item.y, item.z] = linkFlags;
		}
	}
}
