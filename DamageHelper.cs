public static class DamageHelper
{
	public static bool HasForcefulImpact(this DamageType d)
	{
		if (d == DamageType.Healing || d == DamageType.Starvation || d == DamageType.Bleeding || d == DamageType.Suffocation || d == DamageType.Breakdown || d == DamageType.Flame)
		{
			return false;
		}
		return true;
	}

	public static bool HarmsHealth(this DamageType d)
	{
		if (d == DamageType.Healing || d == DamageType.Stun)
		{
			return false;
		}
		return true;
	}

	public static bool MakesBlood(this DamageType d)
	{
		if (d == DamageType.Healing || d == DamageType.Starvation || d == DamageType.Bleeding || d == DamageType.Suffocation || d == DamageType.Breakdown || d == DamageType.Flame)
		{
			return false;
		}
		return true;
	}

	public static float IncapChanceMultiplier(this DamageType d)
	{
		return d switch
		{
			DamageType.Bludgeon => 2f, 
			DamageType.Flame => 1.5f, 
			DamageType.Starvation => 3f, 
			DamageType.Suffocation => 3f, 
			_ => 1f, 
		};
	}
}
