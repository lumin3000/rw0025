using UnityEngine;

public class WeatherEvent_LightningStrike : WeatherEvent_Lightning
{
	private IntVec3 strikeLoc = IntVec3.Invalid;

	private Mesh boltMesh;

	private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt");

	public WeatherEvent_LightningStrike()
	{
	}

	public WeatherEvent_LightningStrike(IntVec3 forcedStrikeLoc)
		: this()
	{
		strikeLoc = forcedStrikeLoc;
	}

	public override void FireEvent()
	{
		base.FireEvent();
		if (!strikeLoc.IsValid)
		{
			strikeLoc = GenMap.RandomSquareWith((IntVec3 sq) => sq.Standable() && !Find.RoofGrid.Roofed(sq));
		}
		boltMesh = LightningBoltMeshPool.RandomBoltMesh;
		Explosion.DoExplosion(strikeLoc, 1.9f, DamageType.Flame);
		Vector3 spawnLoc = strikeLoc.ToVector3Shifted();
		for (int i = 0; i < 4; i++)
		{
			MoteMaker.ThrowSmoke(spawnLoc, 1.5f);
			MoteMaker.ThrowMicroSparks(spawnLoc);
			MoteMaker.ThrowLightningGlow(spawnLoc, 1.5f);
		}
	}

	public override void WeatherEventDraw()
	{
		Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(LightningMat, base.LightningBrightness), 0);
	}
}
