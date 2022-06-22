using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponsControl
{
	public const int BatUIHeight = 32;

	public const int BatUIWidth = 150;

	public const int BatUISpacing = 2;

	protected List<WeaponBattery> BatteryList => null;

	public void WeaponsOnGUI()
	{
		Vector2 tL = new Vector2(0f, 400f);
		foreach (WeaponBattery battery in BatteryList)
		{
			battery.DoBatteryUI(tL);
			battery.DrawTargetIndicator_GUI();
			tL.y += 34f;
		}
		foreach (WeaponBattery battery2 in BatteryList)
		{
			battery2.DrawTargetIndicator_GUI();
		}
		if (Event.current.type == EventType.KeyDown)
		{
			int num = -1;
			switch (Event.current.keyCode)
			{
			case KeyCode.F1:
				num = 0;
				break;
			case KeyCode.F2:
				num = 1;
				break;
			case KeyCode.F3:
				num = 2;
				break;
			case KeyCode.F4:
				num = 3;
				break;
			case KeyCode.F5:
				num = 4;
				break;
			case KeyCode.F6:
				num = 5;
				break;
			case KeyCode.F7:
				num = 6;
				break;
			case KeyCode.F8:
				num = 7;
				break;
			case KeyCode.F9:
				num = 8;
				break;
			case KeyCode.F10:
				num = 9;
				break;
			}
			if (num != -1 && num <= BatteryList.Count - 1)
			{
				TargetPack targ = null;
				BatteryList[num].OrderAttack(targ);
				Event.current.Use();
			}
		}
	}

	public void WeaponsUpdate()
	{
		foreach (WeaponBattery battery in BatteryList)
		{
			battery.DrawTargetIndicator_Rendering();
		}
	}
}
