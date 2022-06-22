using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CompPowerTrader : CompPower
{
	public Action PowerStartedCallback;

	public Action PowerStoppedCallback;

	public float powerOutput;

	private static readonly Texture2D ButtonIconDesirePower = Res.LoadTexture("UI/Commands/DesirePower");

	private bool powerOnInt;

	private bool desirePowerOnInt = true;

	public float EnergyPerTick => powerOutput * CompPower.WattsToWattDaysPerTick;

	public bool PowerOn
	{
		get
		{
			return powerOnInt;
		}
		set
		{
			if (powerOnInt == value)
			{
				return;
			}
			powerOnInt = value;
			if (powerOnInt)
			{
				if (!DesirePowerOn)
				{
					Debug.LogWarning(string.Concat("Tried to power on ", parent, " which did not desire it."));
					return;
				}
				if (PowerStartedCallback != null)
				{
					PowerStartedCallback();
				}
				if (parent.def.powerOnSound != null)
				{
					GenSound.PlaySoundAt(parent.Position, parent.def.powerOnSound, 0.15f);
				}
			}
			else
			{
				if (PowerStoppedCallback != null)
				{
					PowerStoppedCallback();
				}
				if (parent.def.powerOffSound != null)
				{
					GenSound.PlaySoundAt(parent.Position, parent.def.powerOffSound, 0.15f);
				}
			}
		}
	}

	public bool DesirePowerOn
	{
		get
		{
			return desirePowerOnInt;
		}
		set
		{
			if (desirePowerOnInt != value)
			{
				desirePowerOnInt = value;
				if (!desirePowerOnInt)
				{
					PowerOn = false;
				}
			}
		}
	}

	public string DebugString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(parent.Label + " CompPower:");
			stringBuilder.AppendLine("   PowerOn: " + PowerOn);
			stringBuilder.AppendLine("   energyProduction: " + powerOutput);
			return stringBuilder.ToString();
		}
	}

	public override void CompSpawnSetup()
	{
		base.CompSpawnSetup();
		powerOutput = -1f * parent.def.basePowerConsumption;
	}

	public override void CompExposeData()
	{
		Scribe.LookField(ref powerOnInt, "PowerOn");
		Scribe.LookField(ref desirePowerOnInt, "DesirePowerOn");
	}

	public override void CompDraw()
	{
		base.CompDraw();
		if (!DesirePowerOn)
		{
			OverlayDrawer.DrawOverlay(parent, OverlayTypes.PowerOff);
		}
		else if (!PowerOn)
		{
			OverlayDrawer.DrawOverlay(parent, OverlayTypes.NeedsPower);
		}
	}

	public override IEnumerable<Command> CompCommands()
	{
		yield return new Command_Toggle
		{
			hotKey = KeyCode.V,
			icon = ButtonIconDesirePower,
			tipDef = new TooltipDef("Toggle power."),
			isActive = () => DesirePowerOn,
			action = delegate
			{
				DesirePowerOn = !DesirePowerOn;
			}
		};
	}

	public override string CompInspectString()
	{
		string text = string.Empty;
		if (powerOutput <= 0f)
		{
			text = text + "Power needed: " + (0f - powerOutput).ToString("#####0") + " W";
		}
		else if (powerOutput > 0f)
		{
			text = text + "Power output: " + powerOutput.ToString("#####0") + " W";
		}
		return text + "\n" + base.CompInspectString();
	}
}
