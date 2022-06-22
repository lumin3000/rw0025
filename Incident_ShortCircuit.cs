using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Incident_ShortCircuit : IncidentDefinition
{
	public Incident_ShortCircuit()
	{
		uniqueSaveKey = 91260;
		chance = 1.5f;
		global = true;
		minRefireInterval = 100000;
		favorability = IncidentFavorability.Bad;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		List<Building_Battery> list = (from Building_Battery bat in Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Building_Battery)
			where bat.GetComp<CompPowerBattery>().storedEnergy > 50f
			select bat).ToList();
		if (list.Count() == 0)
		{
			return false;
		}
		PowerNet powerNet = list.RandomElement().powerNet;
		List<Building> list2 = powerNet.transmitters.Where((Building trans) => trans.def.eType == EntityType.Building_PowerConduit).ToList();
		if (list2.Count == 0)
		{
			return false;
		}
		float num = 0f;
		foreach (CompPowerBattery batteryComp in powerNet.batteryComps)
		{
			num += batteryComp.storedEnergy;
			batteryComp.storedEnergy = 0f;
		}
		float num2 = Mathf.Sqrt(num) * 0.05f;
		if (num2 > 14.9f)
		{
			num2 = 14.9f;
		}
		Building building = list2.RandomElement();
		Explosion.DoExplosion(building.Position, num2, DamageType.Flame);
		if (num2 > 3.5f)
		{
			Explosion.DoExplosion(building.Position, num2 * 0.3f, DamageType.Bomb);
		}
		building.TakeDamage(new DamageInfo(DamageType.Bomb, 200));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("A fault in an electrical conduit has caused a short circuit.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("All " + num + " Wd of energy  in the connected batteries has been discharged in an electrical explosion.");
		if (num2 > 5f)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("This large amount of power has created an explosion of considerable size.");
		}
		if (num2 > 8f)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("That really is a huge explosion. Wow.");
		}
		Find.LetterStack.ReceiveLetter(new Letter(stringBuilder.ToString(), building.Position));
		return true;
	}
}
