public class Pawn_Ownership : Saveable
{
	private Pawn pawn;

	public Building_Bed ownedBed { get; private set; }

	public Room OwnedPrivateRoom
	{
		get
		{
			if (ownedBed == null)
			{
				return null;
			}
			Room roomAt = Find.Grids.GetRoomAt(ownedBed.Position);
			if (roomAt == null)
			{
				return null;
			}
			if (roomAt.RoomOwner == pawn)
			{
				return roomAt;
			}
			return null;
		}
	}

	public Pawn_Ownership(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Building_Bed refee = ownedBed;
		Scribe.LookThingRef(ref refee, "OwnedBed", this);
		ownedBed = refee;
		if (Scribe.mode == LoadSaveMode.PostLoadInit && ownedBed != null)
		{
			ownedBed.owner = pawn;
		}
	}

	public void ClaimBed(Building_Bed newBed)
	{
		UnclaimBed();
		if (newBed.owner != null)
		{
			newBed.owner.ownership.UnclaimBed();
		}
		newBed.owner = pawn;
		ownedBed = newBed;
	}

	public void UnclaimBed()
	{
		if (ownedBed != null)
		{
			ownedBed.owner = null;
			ownedBed = null;
		}
	}

	public void Notify_PawnDestroyed()
	{
		UnclaimBed();
	}

	public void Notify_TeamChangingTo(TeamType newTeam)
	{
		if (ownedBed != null && ((ownedBed.forPrisoners && newTeam != TeamType.Prisoner) || (!ownedBed.forPrisoners && newTeam == TeamType.Prisoner)))
		{
			UnclaimBed();
		}
	}
}
