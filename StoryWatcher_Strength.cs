public class StoryWatcher_Strength
{
	public float StrengthRating
	{
		get
		{
			float num = 0f;
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				num = (colonist.Incapacitated ? (num + 0.4f) : (num + 1f));
			}
			foreach (Building allBuildingsColonistCombatTarget in Find.BuildingManager.AllBuildingsColonistCombatTargets)
			{
				if (allBuildingsColonistCombatTarget.def.eType == EntityType.Building_TurretGun)
				{
					num += 0.9f;
				}
			}
			return num;
		}
	}
}
