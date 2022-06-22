public class Building_ResearchBench : Building, Interactive
{
	public JobCondition InteractedWith(ReservationType WType, Pawn Worker)
	{
		if (WType == ReservationType.Research)
		{
			float num = 0.1f + 0.15f * (float)Worker.skills.LevelOf(SkillType.Research);
			num *= Worker.healthTracker.CurEffectivenessPercent;
			if (Find.GlowGrid.PsychGlowAt(Worker.Position) == PsychGlow.Dark)
			{
				num *= 0.5f;
			}
			Find.ResearchManager.ResearchProgressMade(num);
			Worker.skills.Learn(SkillType.Research, 0.22f);
		}
		return JobCondition.Ongoing;
	}
}
