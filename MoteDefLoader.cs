using System.Collections.Generic;
using UnityEngine;

public static class MoteDefLoader
{
	private const string MoteTexPath = "Icons/Mote";

	public static IEnumerable<ThingDefinition> MoteDefsFromTextures()
	{
		Object[] files = Resources.LoadAll("Textures/Icons/Mote", typeof(Texture2D));
		Object[] array = files;
		foreach (Object obj in array)
		{
			Texture2D tex = (Texture2D)obj;
			string defName = "Mote_" + tex.name;
			ThingDefinition newDef = new ThingDefinition
			{
				label = Gen.SplitCamelCase(tex.name),
				definitionName = defName,
				thingClass = typeof(MoteThrown),
				category = EntityCategory.Mote,
				altitudeLayer = AltitudeLayer.FloorDeco,
				tickerType = TickerType.Normal,
				useStandardHealth = false,
				isSaveable = false,
				mote = new MoteProperties(),
				baseMaterial = MatBases.Mote,
				texturePath = "Icons/Mote/" + tex.name,
				rotatable = false
			};
			if (tex.name == "FeedbackGoto" || tex.name == "FeedbackAttack" || tex.name == "FeedbackEquip")
			{
				newDef.mote.realTime = true;
				newDef.mote.ticksBeforeStartFadeout = 15;
				newDef.mote.fadeoutDuration = 10;
			}
			if (tex.name == "Stun")
			{
				newDef.thingClass = typeof(MoteAttached);
				newDef.mote.ticksBeforeStartFadeout = 5000;
				newDef.mote.needsMaintenance = true;
			}
			if (tex.name == "Clean" || tex.name == "Sow" || tex.name == "Harvest")
			{
				newDef.thingClass = typeof(MoteAttached);
				newDef.mote.ticksBeforeStartFadeout = 5000;
			}
			if (tex.name == "ShotFlash")
			{
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.ticksBeforeStartFadeout = 6;
				newDef.mote.fadeoutDuration = 3;
			}
			if (tex.name == "SparkFlash")
			{
				newDef.thingClass = typeof(MoteAttached);
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.ticksBeforeStartFadeout = 5;
				newDef.mote.fadeoutDuration = 6;
			}
			if (tex.name == "Speech")
			{
				newDef.thingClass = typeof(MoteAttached);
				newDef.baseMaterial = MatBases.Transparent;
				newDef.mote.ticksBeforeStartFadeout = 70;
				newDef.mote.fadeoutDuration = 0;
				newDef.mote.attachedDrawOffset = new Vector3(0.75f, 0f, 0.75f);
			}
			if (tex.name == "ShotHit_Spark")
			{
				newDef.baseMaterial = MatBases.MotePostLight;
				newDef.mote.ticksBeforeStartFadeout = 4;
				newDef.mote.fadeoutDuration = 4;
			}
			if (tex.name == "ShotHit_Dirt")
			{
				newDef.baseMaterial = MatBases.Transparent;
				newDef.mote.ticksBeforeStartFadeout = 2;
				newDef.mote.fadeoutDuration = 6;
			}
			if (tex.name == "ExplosionFlash")
			{
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.ticksBeforeStartFadeout = 6;
				newDef.mote.fadeoutDuration = 3;
			}
			if (tex.name == "Blast")
			{
				newDef.baseMaterial = MatBases.MotePostLight;
				newDef.mote.ticksBeforeStartFadeout = 0;
				newDef.mote.fadeoutDuration = 12;
			}
			if (tex.name == "FireGlow")
			{
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.fadeinDuration = 48;
				newDef.mote.ticksBeforeStartFadeout = 112;
				newDef.mote.fadeoutDuration = 48;
			}
			if (tex.name == "MicroSparks")
			{
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.ticksBeforeStartFadeout = 70;
				newDef.mote.fadeinDuration = 40;
				newDef.mote.fadeoutDuration = 40;
			}
			if (tex.name == "LightningGlow")
			{
				newDef.baseMaterial = MatBases.LocalGlow;
				newDef.mote.ticksBeforeStartFadeout = 10;
				newDef.mote.fadeinDuration = 5;
				newDef.mote.fadeoutDuration = 20;
			}
			if (tex.name == "FoodBit")
			{
				newDef.baseMaterial = MatBases.Cutout;
				newDef.mote.fadeinDuration = 5;
				newDef.mote.ticksBeforeStartFadeout = 30;
				newDef.mote.fadeoutDuration = 15;
			}
			if (tex.name == "Footprint")
			{
				newDef.mote.fadeinDuration = 5;
				newDef.mote.ticksBeforeStartFadeout = 150;
				newDef.mote.fadeoutDuration = 150;
			}
			if (tex.name == "AirPuff")
			{
				newDef.mote.fadeinDuration = 3;
				newDef.mote.ticksBeforeStartFadeout = 5;
				newDef.mote.fadeoutDuration = 60;
			}
			if (tex.name == "DustPuff")
			{
				newDef.mote.fadeinDuration = 3;
				newDef.mote.ticksBeforeStartFadeout = 5;
				newDef.mote.fadeoutDuration = 60;
			}
			if (tex.name == "Smoke")
			{
				newDef.mote.fadeinDuration = 30;
				newDef.mote.ticksBeforeStartFadeout = 400;
				newDef.mote.fadeoutDuration = 200;
				newDef.mote.growthRate = 0.005f;
			}
			if (tex.name == "Spark")
			{
				newDef.mote.fadeinDuration = 5;
				newDef.mote.ticksBeforeStartFadeout = 25;
				newDef.mote.fadeoutDuration = 50;
			}
			if (tex.name == "TempRoof")
			{
				newDef.mote.ticksBeforeStartFadeout = 90;
				newDef.mote.fadeoutDuration = 90;
			}
			if (tex.name == "HealingCross" || tex.name == "SleepZ" || tex.name == "IncapIcon")
			{
				newDef.mote.fadeinDuration = 5;
				newDef.mote.ticksBeforeStartFadeout = 90;
				newDef.mote.fadeoutDuration = 90;
			}
			yield return newDef;
		}
	}
}
