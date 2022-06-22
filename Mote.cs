using UnityEngine;

public abstract class Mote : Thing
{
	protected const float MinimumVelocity = 0.02f;

	public Vector3 exactPosition;

	public float exactRotation;

	public Vector3 exactScale = new Vector3(1f, 1f, 1f);

	public float exactRotationRate;

	public float skidSpeedMultiplierPerTick = Random.Range(0.3f, 0.95f);

	protected int spawnTick;

	protected float spawnRealTime;

	private int lastMaintainTick;

	public float ScaleUniform
	{
		set
		{
			exactScale = new Vector3(value, 1f, value);
		}
	}

	protected float MoteAge
	{
		get
		{
			if (def.mote.realTime)
			{
				return (Time.realtimeSinceStartup - spawnRealTime) * 60f;
			}
			return Find.TickManager.tickCount - spawnTick;
		}
	}

	public override Vector3 DrawPos => exactPosition;

	public override Material DrawMat
	{
		get
		{
			if (MoteAge <= (float)def.mote.fadeinDuration)
			{
				float alpha = MoteAge / (float)def.mote.fadeinDuration;
				return FadedMaterialPool.FadedVersionOf(def.drawMat, alpha);
			}
			if (MoteAge >= (float)def.mote.ticksBeforeStartFadeout)
			{
				float alpha2 = 1f - (MoteAge - (float)def.mote.ticksBeforeStartFadeout) / (float)def.mote.fadeoutDuration;
				return FadedMaterialPool.FadedVersionOf(def.drawMat, alpha2);
			}
			return base.DrawMat;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		spawnTick = Find.TickManager.tickCount;
		spawnRealTime = Time.realtimeSinceStartup;
		Find.RealTime.moteList.MoteSpawned(this);
	}

	public override void DeSpawn()
	{
		base.DeSpawn();
		Find.RealTime.moteList.MoteDestroyed(this);
	}

	public void RealtimeUpdate()
	{
		if (MoteAge >= (float)(def.mote.ticksBeforeStartFadeout + def.mote.fadeoutDuration))
		{
			Destroy();
		}
	}

	public override void Tick()
	{
		if (def.mote.growthRate > 0f)
		{
			exactScale = new Vector3(exactScale.x + def.mote.growthRate, exactScale.y, exactScale.z + def.mote.growthRate);
		}
		if (def.mote.needsMaintenance && lastMaintainTick < Find.TickManager.tickCount - 1)
		{
			Destroy();
		}
	}

	public override void Draw()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(DrawPos, Quaternion.AngleAxis(exactRotation, Vector3.up), exactScale);
		Graphics.DrawMesh(MeshPool.plane10, matrix, DrawMat, 0);
	}

	public void Maintain()
	{
		lastMaintainTick = Find.TickManager.tickCount;
	}
}
