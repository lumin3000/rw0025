public class TutorNote_FireExtinguishing : TutorNote
{
	public TutorNote_FireExtinguishing()
	{
		showSignalsList.Add(TutorSignal.FireStarted);
		codexPath = "Concepts/Fire/Fire basics";
		baseText = "Your colonists will extinguish FIRES only if they are in HOME ZONES. Define a HOME ZONE with the Orders tab under the Architect menu.";
	}
}
