using System;
using System.Collections.Generic;

public class MapFileCompressor : Saveable
{
	private byte[] typesMap;

	private string compressedString;

	public void ExposeData()
	{
		Scribe.LookField(ref compressedString, "CompressedThingMap");
	}

	public void PrepareDataForSave()
	{
		CompressionUtility.InitForSave();
		Func<IntVec3, byte> byteGetter = delegate(IntVec3 curSq)
		{
			Thing thing = Find.Grids.BlockerAt(curSq);
			return (byte)((thing != null && thing.IsSaveCompressible()) ? ((byte)thing.def.eType) : 0);
		};
		compressedString = GridSaveUtility.CompressedStringForByteGrid(byteGetter);
	}

	public IEnumerable<Thing> ThingsToSpawnAfterLoad()
	{
		foreach (GridSaveUtility.LoadedGridByte gridThing in GridSaveUtility.ThingsFromThingTypeGrid(compressedString))
		{
			EntityType valType = (EntityType)gridThing.val;
			if (valType != 0)
			{
				Thing th = ThingMaker.MakeThing(valType);
				th.SetRawPosition(gridThing.pos);
				yield return th;
			}
		}
	}
}
