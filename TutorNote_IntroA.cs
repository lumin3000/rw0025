public class TutorNote_IntroA : TutorNote
{
	public TutorNote_IntroA()
	{
		showSignalsList.Add(TutorSignal.GameStarted);
		baseText = "Control the camera by clicking and dragging the MIDDLE MOUSE BUTTON or with the WASD keys.\n\nZoom with the MOUSEWHEEL or the PAGEUP/PAGEDOWN keys.";
		codexPath = "Concepts/Controls/Camera";
		nextItemType = typeof(TutorNote_IntroB);
	}
}
