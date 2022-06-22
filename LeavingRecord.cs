public class LeavingRecord
{
	public int count;

	private EntityType tType;

	private string defName = string.Empty;

	public ThingDefinition ThingDefToSpawn
	{
		get
		{
			if (tType != 0)
			{
				return tType.DefinitionOfType();
			}
			if (defName != string.Empty)
			{
				return ThingDefDatabase.ThingDefNamed(defName);
			}
			return null;
		}
	}

	public LeavingRecord()
	{
	}

	public LeavingRecord(EntityType tType, int count)
	{
		this.tType = tType;
		this.count = count;
	}

	public override string ToString()
	{
		return string.Concat("LeavingRecord(tType=", tType, " defName=", defName, " count=", count, ")");
	}
}
