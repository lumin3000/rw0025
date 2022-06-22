using UnityEngine;

public static class MoteMaker
{
	private static MoteThrown NewBaseAirPuff(IntVec3 spawnSq)
	{
		MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_AirPuff"));
		ThingMaker.Spawn(moteThrown, spawnSq);
		moteThrown.ScaleUniform = 1.5f;
		moteThrown.exactRotationRate = Random.Range(-4, 5);
		return moteThrown;
	}

	public static void ThrowAirPuff(IntVec3 spawnSq, IntVec3 sprayIntoSq, bool highSpeed)
	{
		if (spawnSq.ShouldSpawnMotesAt())
		{
			MoteThrown moteThrown = NewBaseAirPuff(sprayIntoSq);
			if (sprayIntoSq != spawnSq)
			{
				moteThrown.exactPosition = spawnSq.ToVector3Shifted();
				moteThrown.exactPosition += (sprayIntoSq - spawnSq).ToVector3() * 0.51f;
				moteThrown.exactMoveDirection = (sprayIntoSq - spawnSq).AngleFlat;
				moteThrown.exactMoveDirection -= 30f;
				moteThrown.exactMoveDirection += 60f * Random.value;
			}
			else
			{
				moteThrown.exactPosition = spawnSq.ToVector3Shifted();
				moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
				moteThrown.exactPosition += new Vector3(Random.value, 0f, Random.value);
				moteThrown.exactMoveDirection = Random.Range(0, 360);
			}
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.HighMote);
			if (highSpeed)
			{
				moteThrown.exactVelocity = Random.Range(0.03f, 0.06f);
			}
			else
			{
				moteThrown.exactVelocity = Random.Range(0.02f, 0.025f);
			}
		}
	}

	public static void ThrowAirPuffUp(Vector3 spawnLoc, AltitudeLayer alt)
	{
		if (spawnLoc.ToIntVec3().ShouldSpawnMotesAt())
		{
			MoteThrown moteThrown = NewBaseAirPuff(spawnLoc.ToIntVec3());
			moteThrown.exactPosition = spawnLoc;
			moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
			moteThrown.exactPosition += new Vector3(Random.value, 0f, Random.value);
			moteThrown.exactMoveDirection = Random.Range(0, 360);
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(alt);
			moteThrown.exactVelocity = Random.Range(0.02f, 0.025f);
		}
	}

	public static void ThrowSmoke(Vector3 spawnLoc, float size)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			IntVec3 intVec = spawnLoc.ToIntVec3();
			if (!(intVec.ParticleSaturation() > 0.9f))
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Smoke"));
				ThingMaker.Spawn(moteThrown, intVec);
				moteThrown.ScaleUniform = Random.Range(1.5f, 2.5f) * size;
				moteThrown.exactRotationRate = Random.Range(-0.5f, 0.5f);
				moteThrown.exactPosition = spawnLoc;
				moteThrown.exactMoveDirection = Random.Range(30, 40);
				moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.OverheadMote);
				moteThrown.exactVelocity = Random.Range(0.008f, 0.012f);
			}
		}
	}

	public static void ThrowFireGlow(Vector3 spawnLoc, float size)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			IntVec3 intVec = spawnLoc.ToIntVec3();
			if (!(intVec.ParticleSaturation() > 1.25f))
			{
				spawnLoc.y = Altitudes.AltitudeFor(AltitudeLayer.Weather);
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_FireGlow"));
				ThingMaker.Spawn(moteThrown, intVec);
				moteThrown.ScaleUniform = Random.Range(4f, 6f) * size;
				moteThrown.exactRotationRate = Random.Range(-0.05f, 0.05f);
				moteThrown.exactPosition = spawnLoc;
				moteThrown.exactMoveDirection = Random.Range(0, 360);
				moteThrown.exactVelocity = Random.Range(0.0002f, 0.0002f);
			}
		}
	}

	public static void ThrowMicroSparks(Vector3 spawnLoc)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			IntVec3 intVec = spawnLoc.ToIntVec3();
			if (!(intVec.ParticleSaturation() > 1.25f))
			{
				spawnLoc.y = Altitudes.AltitudeFor(AltitudeLayer.Weather);
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_MicroSparks"));
				ThingMaker.Spawn(moteThrown, intVec);
				moteThrown.ScaleUniform = Random.Range(0.8f, 1.2f);
				moteThrown.exactRotationRate = Random.Range(-0.2f, 0.2f);
				moteThrown.exactPosition = spawnLoc;
				moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
				moteThrown.exactPosition += new Vector3(Random.value, 0f, Random.value);
				moteThrown.exactMoveDirection = Random.Range(35, 45);
				moteThrown.exactVelocity = Random.Range(0.02f, 0.02f);
			}
		}
	}

	public static void ThrowLightningGlow(Vector3 spawnLoc, float size)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			spawnLoc.y = Altitudes.AltitudeFor(AltitudeLayer.Weather);
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_LightningGlow"));
			ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
			moteThrown.ScaleUniform = Random.Range(4f, 6f) * size;
			moteThrown.exactRotationRate = Random.Range(-0.05f, 0.05f);
			moteThrown.exactPosition = spawnLoc + size * new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f);
			moteThrown.exactMoveDirection = Random.Range(0, 360);
			moteThrown.exactVelocity = Random.Range(0.0002f, 0.0002f);
		}
	}

	public static void ThrowFoodBit(Vector3 spawnLoc)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_FoodBit"));
			ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
			moteThrown.ScaleUniform = 0.5f;
			moteThrown.exactRotation = Random.Range(0, 360);
			moteThrown.exactPosition = spawnLoc + Gen.RandomHorizontalVector(0.3f);
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.OverWaist);
			moteThrown.exactMoveDirection = Random.Range(0, 360);
			moteThrown.exactVelocity = Random.Range(0f, 0.005f);
		}
	}

	public static void PlaceFootprint(Vector3 spawnLoc, float rot)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Footprint"));
			ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
			moteThrown.ScaleUniform = 0.5f;
			moteThrown.exactRotation = rot;
			moteThrown.exactPosition = spawnLoc;
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.FloorDeco);
		}
	}

	public static Mote MakeSpark(Vector3 spawnLoc)
	{
		if (!spawnLoc.ShouldSpawnMotesAt())
		{
			return null;
		}
		MoteAttached moteAttached = (MoteAttached)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_SparkFlash"));
		ThingMaker.Spawn(moteAttached, spawnLoc.ToIntVec3());
		moteAttached.ScaleUniform = 5f;
		moteAttached.exactPosition = spawnLoc;
		moteAttached.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.OverheadMote);
		return moteAttached;
	}

	public static void MakeSpark(Pawn worker, IntVec3 workSquare)
	{
		MakeSpark((worker.DrawPos + workSquare.ToVector3Shifted()) * 0.5f);
	}

	public static Mote MakeShotHitSpark(Vector3 spawnLoc)
	{
		if (!spawnLoc.ShouldSpawnMotesAt())
		{
			return null;
		}
		MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_ShotHit_Spark"));
		ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
		moteThrown.exactPosition = spawnLoc;
		return moteThrown;
	}

	public static Mote MakeShotHitDirt(Vector3 spawnLoc)
	{
		if (!spawnLoc.ShouldSpawnMotesAt())
		{
			return null;
		}
		MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_ShotHit_Dirt"));
		ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
		moteThrown.exactPosition = spawnLoc;
		return moteThrown;
	}

	public static MoteAttached MakeStunOverlay(Thing stunnedThing)
	{
		MoteAttached moteAttached = (MoteAttached)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Stun"));
		ThingMaker.Spawn(moteAttached, stunnedThing.Position);
		moteAttached.AttachTo(stunnedThing);
		return moteAttached;
	}

	public static void MakeSpeechOverlay(Pawn talker)
	{
		MoteAttached moteAttached = (MoteAttached)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Speech"));
		ThingMaker.Spawn(moteAttached, talker.Position);
		moteAttached.ScaleUniform = 1.25f;
		moteAttached.AttachTo(talker);
	}

	public static MoteAttached MakeInteractionOverlay(string moteName, Thing A, Thing B)
	{
		MoteAttached moteAttached = (MoteAttached)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_" + moteName));
		ThingMaker.Spawn(moteAttached, A.Position);
		moteAttached.ScaleUniform = 0.5f;
		moteAttached.AttachTo(A, B);
		return moteAttached;
	}

	public static void ThrowDustPuff(IntVec3 spawnSq, float scale)
	{
		Vector3 spawnLoc = spawnSq.ToVector3() + new Vector3(Random.value, 0f, Random.value);
		ThrowDustPuff(spawnLoc, scale);
	}

	public static void ThrowDustPuff(Vector3 spawnLoc, float scale)
	{
		if (spawnLoc.ShouldSpawnMotesAt())
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_DustPuff"));
			ThingMaker.Spawn(moteThrown, spawnLoc.ToIntVec3());
			moteThrown.ScaleUniform = 1.9f * scale;
			moteThrown.exactRotationRate = Random.Range(-1, 1);
			moteThrown.exactVelocity = Random.Range(0.01f, 0.013f);
			moteThrown.exactPosition = spawnLoc;
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.HighMote);
			moteThrown.exactMoveDirection = Random.Range(0, 360);
		}
	}

	public static void ThrowSpark(IntVec3 workerSq, IntVec3 touchingSq)
	{
		if (workerSq.ShouldSpawnMotesAt())
		{
			Vector3 vector = workerSq.ToVector3Shifted();
			Vector3 vector2 = (touchingSq.ToVector3Shifted() - workerSq.ToVector3Shifted()).normalized * 0.49f;
			vector += vector2;
			MakeSpark(vector);
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Spark"));
			ThingMaker.Spawn(moteThrown, workerSq);
			moteThrown.airTicksLeft = Random.Range(5, 10);
			moteThrown.ScaleUniform = Random.Range(0.8f, 2f);
			moteThrown.exactRotationRate = Random.Range(-4, 4);
			moteThrown.exactVelocity = Random.Range(0.12f, 0.4f);
			moteThrown.exactPosition = vector;
			moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.HighMote);
			moteThrown.exactMoveDirection = (workerSq - touchingSq).AngleFlat;
			moteThrown.exactMoveDirection -= 45f;
			moteThrown.exactMoveDirection += 90f * Random.value;
		}
	}

	public static void PlaceTempRoof(IntVec3 spawnSq)
	{
		if (spawnSq.ShouldSpawnMotesAt())
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_TempRoof"));
			ThingMaker.Spawn(mote, spawnSq);
			mote.exactPosition = spawnSq.ToVector3Shifted();
			mote.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
		}
	}

	public static void ThrowExplosionSquare(IntVec3 spawnSq)
	{
		if (spawnSq.ShouldSpawnMotesAt())
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_Blast"));
			ThingMaker.Spawn(mote, spawnSq);
			mote.exactRotation = 90 * Random.Range(0, 4);
			mote.exactPosition = spawnSq.ToVector3Shifted();
			mote.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
			if (Random.value < 0.7f)
			{
				ThrowDustPuff(spawnSq, 1.2f);
			}
		}
	}

	private static Mote ThrowIcon(IntVec3 spawnSq, string defName)
	{
		if (!spawnSq.ShouldSpawnMotesAt())
		{
			return null;
		}
		MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed(defName));
		ThingMaker.Spawn(moteThrown, spawnSq);
		moteThrown.ScaleUniform = 0.7f;
		moteThrown.exactRotationRate = Random.Range(-0.05f, 0.05f);
		moteThrown.exactVelocity = Random.Range(0.007f, 0.007f);
		moteThrown.exactPosition = spawnSq.ToVector3Shifted();
		moteThrown.exactPosition += new Vector3(0.35f, 0f, 0.35f);
		moteThrown.exactPosition += new Vector3(Random.value, 0f, Random.value) * 0.1f;
		moteThrown.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
		moteThrown.exactMoveDirection = Random.Range(30, 60);
		return moteThrown;
	}

	public static void ThrowHealingCross(IntVec3 spawnSq)
	{
		ThrowIcon(spawnSq, "Mote_HealingCross");
	}

	public static void ThrowSleepZ(IntVec3 spawnSq)
	{
		ThrowIcon(spawnSq, "Mote_SleepZ");
	}

	public static void ThrowIncapIcon(IntVec3 spawnSq)
	{
		ThrowIcon(spawnSq, "Mote_IncapIcon");
	}

	public static void ThrowFeedback(IntVec3 spawnSq, string feedbackName)
	{
		Mote mote = (Mote)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_" + feedbackName));
		ThingMaker.Spawn(mote, spawnSq);
		mote.exactPosition = spawnSq.ToVector3Shifted();
		mote.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
	}

	public static void ThrowFlash(IntVec3 spawnSq, string flashName, float scale)
	{
		Mote mote = (Mote)ThingMaker.MakeThing(ThingDefDatabase.ThingDefNamed("Mote_" + flashName));
		ThingMaker.Spawn(mote, spawnSq);
		mote.ScaleUniform = scale;
		mote.exactPosition = spawnSq.ToVector3Shifted();
		mote.exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
	}
}
