public class TutorNote_CombatIntro : TutorNote
{
	public TutorNote_CombatIntro()
	{
		showSignalsList.Add(TutorSignal.EnemiesAttacking);
		baseText = "To control colonists directly for combat, Draft them by selecting them and hitting the Draft button.";
		codexPath = "Concepts/Combat/Drafting";
		nextItemType = typeof(TutorNote_CombatIntroB);
	}
}
