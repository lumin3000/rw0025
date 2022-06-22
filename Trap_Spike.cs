using UnityEngine;

public class Trap_Spike : Building
{
	protected const int SpringCooldownTicksMax = 3000;

	protected bool trapResetting;

	protected Material matSet;

	protected Material matSprung;

	public override Material DrawMat
	{
		get
		{
			if (!trapResetting)
			{
				return matSet;
			}
			return matSprung;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		matSet = MaterialPool.MatFrom("Icons/FurnitureSpecial/Trap_Spike_Set");
		matSprung = MaterialPool.MatFrom("Icons/FurnitureSpecial/Trap_Spike_Sprung");
		GetComp<CompTouchTrigger>().SetTouchCallback(Touched);
	}

	private void Touched(Pawn p)
	{
		SpringTrap(p);
	}

	private void SpringTrap(Pawn p)
	{
		DamageInfo d = new DamageInfo(DamageType.Bullet, 25, Vector3.forward);
		p.TakeDamage(d);
		trapResetting = true;
	}
}
