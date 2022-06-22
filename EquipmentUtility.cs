public static class EquipmentUtility
{
	public static Equipment AsThingEquipment(this Verb verb)
	{
		Equipment equipment = (Equipment)ThingMaker.MakeThing(EntityType.Equipment);
		equipment.verb = verb;
		return equipment;
	}
}
