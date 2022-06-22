using UnityEngine;

public class MapPointer_TakeGun : MapPointer
{
	private Thing gun;

	protected override string LabelText => "Pick up this gun by selecting a colonist and then right clicking on it.";

	protected override Vector3 Location => gun.Position.ToVector3Shifted();

	public override bool Completed
	{
		get
		{
			if (gun == null)
			{
				InitializePointer();
			}
			return gun == null || gun.destroyed || gun.carrier != null || !gun.spawnedInWorld;
		}
	}

	public override void InitializePointer()
	{
		int num = Gen.NumSquaresInRadius(15f);
		for (int i = 0; i < num; i++)
		{
			IntVec3 sq = Genner_PlayerStuff.PlayerStartSpot + Gen.RadialPattern[i];
			foreach (Thing item in Find.Grids.ThingsAt(sq))
			{
				if (item.def.eType == EntityType.Equipment)
				{
					gun = item;
					break;
				}
			}
			if (gun != null)
			{
				break;
			}
		}
		if (gun != null)
		{
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookThingRef(ref gun, "Gun", this);
	}
}
