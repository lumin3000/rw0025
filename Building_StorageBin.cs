public class Building_StorageBin : Building
{
	public StorageUnit Storage;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		Storage.TotalCapacity = 20;
	}
}
