public class TutorNote_WorkSettingsA : TutorNote
{
	public TutorNote_WorkSettingsA()
	{
		baseText = "You can set each colonist's work settings in the OVERVIEW at the bottom right of the screen.\n\nThey will always do the highest priority work type that they can find.";
		codexPath = "Concepts/Work/";
		nextItemType = typeof(TutorNote_WorkSettingsB);
	}
}
