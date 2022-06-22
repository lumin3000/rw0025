public class AmmoContainer
{
	public Verb verb;

	public bool limitedAmmo;

	public int ammoMax;

	public int ammo;

	public AmmoContainer(Verb verb)
	{
		this.verb = verb;
	}

	public bool HasAmmo()
	{
		if (!limitedAmmo)
		{
			return true;
		}
		return ammo > 0;
	}

	public void ConsumeAmmo()
	{
		if (verb.owner.Team == TeamType.Colonist)
		{
			ammo--;
		}
	}

	public void RefillAmmo()
	{
		ammo = ammoMax;
	}
}
