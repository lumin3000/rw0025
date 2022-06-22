public class TutorNote_MiningA : TutorNote
{
	public TutorNote_MiningA()
	{
		baseText = "Mining works the same way as building: you designate what you want mined, and the colonists schedules themselves to get it done.";
		codexPath = "Concepts/Controls/Mining";
		nextItemType = typeof(TutorNote_MiningB);
	}
}
