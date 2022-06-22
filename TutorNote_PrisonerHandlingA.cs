public class TutorNote_PrisonerHandlingA : TutorNote
{
	public TutorNote_PrisonerHandlingA()
	{
		showSignalsList.Add(TutorSignal.PrisonerInBed);
		codexPath = "Concepts/Prisoners/Prisoner handling";
		baseText = "Look in a prisoner's 'Prisoner' tab to set how they'll be treated.\n\nYou'll need a colonist with the Warden work to take care of the prisoner.";
	}
}
