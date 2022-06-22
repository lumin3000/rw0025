using UnityEngine;

public class Building_Grave : Building_Storage
{
	private static readonly Material IconEmpty = MaterialPool.MatFrom("Icons/Building/GraveEmpty");

	private static readonly Material IconFull = MaterialPool.MatFrom("Icons/Building/GraveFull");

	public override Material DrawMat
	{
		get
		{
			if (base.Position.ContainedStorable() == null)
			{
				return IconEmpty;
			}
			return IconFull;
		}
	}

	public override void Notify_ReceivedThing(Thing newItem)
	{
		Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things);
	}

	public override void Notify_LosingThing(Thing newItem)
	{
		Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things);
	}
}
