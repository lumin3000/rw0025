using System.Collections.Generic;
using UnityEngine;

public class WeaponBattery
{
	protected const int VolleyIntervalTicks = 20;

	public List<Weapon> WeaponList = new List<Weapon>();

	public bool QueuedCast;

	public TargetPack QueuedCastTarg;

	protected int VolleyIntervalTicksLeft;

	protected int VolleyCurIndex;

	protected TargetPack VolleyTarg;

	public bool GroupCooling => TicksToCooledDown > 0;

	public int TicksToCooledDown
	{
		get
		{
			int num = 0;
			foreach (Weapon weapon in WeaponList)
			{
				if (weapon.CooldownTicksLeft > (float)num)
				{
					num = (int)weapon.CooldownTicksLeft;
				}
			}
			return num;
		}
	}

	public int TotalCooldown
	{
		get
		{
			int num = 0;
			foreach (Weapon weapon in WeaponList)
			{
				if (weapon.def.mach_CooldownTicks > num)
				{
					num = weapon.def.mach_CooldownTicks;
				}
			}
			return num;
		}
	}

	public void AddWeapon(Weapon w)
	{
		if (w.def.mach_AutoFire)
		{
			Debug.LogWarning(string.Concat("Cannot add automatic weapon ", w, " to battery."));
		}
		else
		{
			WeaponList.Add(w);
		}
	}

	public void RemoveWeapon(Weapon w)
	{
		if (WeaponList.Contains(w))
		{
			WeaponList.Remove(w);
		}
		else
		{
			Debug.LogWarning("Removed weapon not in battery: " + w.Label);
		}
	}

	public void BatteryTick()
	{
		foreach (Weapon weapon in WeaponList)
		{
			if (!weapon.spawnedInWorld)
			{
				weapon.Tick();
			}
		}
		if (QueuedCast && !GroupCooling)
		{
			StartVolleyAt(QueuedCastTarg);
			QueuedCast = false;
		}
		if (VolleyTarg != null)
		{
			VolleyIntervalTicksLeft--;
			if (VolleyIntervalTicksLeft <= 0)
			{
				VolleyShot();
			}
		}
	}

	public void OrderAttack(TargetPack Targ)
	{
		if (!GroupCooling)
		{
			StartVolleyAt(Targ);
			return;
		}
		QueuedCast = true;
		QueuedCastTarg = Targ;
	}

	protected void StartVolleyAt(TargetPack Targ)
	{
		if (WeaponList.Count != 0)
		{
			VolleyTarg = Targ;
			VolleyCurIndex = 0;
			VolleyShot();
		}
	}

	protected void VolleyShot()
	{
		WeaponList[VolleyCurIndex].TryCastAt(VolleyTarg);
		VolleyCurIndex++;
		if (VolleyCurIndex < WeaponList.Count)
		{
			VolleyIntervalTicksLeft = 20;
		}
		else
		{
			VolleyTarg = null;
		}
	}

	public void DoBatteryUI(Vector2 TL)
	{
		GUI.BeginGroup(new Rect(TL.x, TL.y, 150f, 32f));
		if (GUI.Button(new Rect(0f, 0f, 150f, 32f), string.Empty))
		{
		}
		GUI.DrawTexture(new Rect(0f, 0f, 150f, 32f), UIBackgroundTex());
		GenUI.SetFontTiny();
		string empty = string.Empty;
		string text = empty;
		empty = text + "F" + BatteryIndex() + ". ";
		foreach (Weapon weapon in WeaponList)
		{
			empty += weapon.Label;
			if (WeaponList.IndexOf(weapon) != WeaponList.Count - 1)
			{
				empty += ", ";
			}
		}
		GUI.Label(new Rect(3f, 1f, 150f, 32f), empty);
		Texture2D redTex = GenUI.RedTex;
		UIWidgets.FillableBar(new Rect(5f, 19f, (float)TotalCooldown / 30f, 10f), 1f - (float)TicksToCooledDown / (float)TotalCooldown, redTex);
		GUI.EndGroup();
	}

	private int BatteryIndex()
	{
		return 1;
	}

	private Texture2D UIBackgroundTex()
	{
		if (GroupCooling)
		{
			if (QueuedCast)
			{
				return Res.LoadTexture("UI/MachUI/MachBG_Red");
			}
			return Res.LoadTexture("UI/MachUI/MachBG_Gray");
		}
		return Res.LoadTexture("UI/MachUI/MachBG_Green");
	}

	public void DrawTargetIndicator_GUI()
	{
	}

	public void DrawTargetIndicator_Rendering()
	{
	}

	private bool ShouldDrawTargetIndicator()
	{
		if (QueuedCast)
		{
			return true;
		}
		return false;
	}
}
