public class ThingReservation : Saveable
{
	public Pawn claimant;

	public ReservationType interaction;

	public TargetPack target;

	public void ExposeData()
	{
		Scribe.LookThingRef(ref claimant, "Claimant", this);
		Scribe.LookField(ref interaction, "InteractionType", forceSave: true);
		Scribe.LookSaveable(ref target, "Target");
	}
}
