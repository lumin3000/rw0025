public class TutorNote_CaptureEnemy : TutorNote
{
	public TutorNote_CaptureEnemy()
	{
		showSignalsList.Add(TutorSignal.EnemyIncapacitated);
		codexPath = "Concepts/Prisoners/";
		nextItemType = typeof(TutorNote_CaptureEnemyB);
	}

	protected override string GetFullText()
	{
		return "An enemy has been incapacitated. You can capture them!";
	}
}
