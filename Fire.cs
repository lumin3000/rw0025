using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : AttachableThing
{
	public const float MinFireSize = 0.1f;

	private const float MinSizeForSpark = 1f;

	private const float TicksBetweenSparksBase = 150f;

	private const float TicksBetweenSparksReductionPerFireSize = 40f;

	private const float MinTicksBetweenSparks = 75f;

	private const float MinFireSizeToEmitSpark = 1f;

	private const float SquareIgniteChancePerTickPerSize = 0.01f;

	private const float MinSizeForSquareIgnite = 0.6f;

	private const float FireBaseGrowthPerTick = 0.0005f;

	private const float BaseTicksBetweenDamage = 95f;

	private const float TicksBetweenDamageReductionPerFireSize = 40f;

	private const float MinTicksBetweenDamage = 15f;

	private const int SmokeIntervalRandomAddon = 10;

	private const float BaseSkyExtinguishChance = 0.04f;

	private const int BaseSkyExtinguishDamage = 2;

	public float fireSize = 0.1f;

	private int ticksSinceSpark;

	private int ticksSinceDamage;

	private Material curDrawFrame = MatsSimple.BadMaterial;

	private int ticksUntilFrameChange;

	private Vector2 curDrawOffset = Vector2.zero;

	private int ticksUntilSmoke;

	private static readonly IntRange SmokeIntervalRange = new IntRange(70, 107);

	private static readonly IntRange FrameChangeIntervalRange = new IntRange(10, 20);

	public override string Label
	{
		get
		{
			if (parent != null)
			{
				return "Fire on " + parent.Label;
			}
			return "Fire";
		}
	}

	public override string InfoStringAddon => "Burning (fire size " + fireSize.ToString("###0.0") + ")";

	private float TicksBeforeSpark
	{
		get
		{
			if (fireSize < 1f)
			{
				return 999999f;
			}
			float num = 150f - (fireSize - 1f) * 40f;
			if (num < 75f)
			{
				num = 75f;
			}
			return num;
		}
	}

	private float TicksBeforeDamage
	{
		get
		{
			float num = 95f - fireSize * 40f;
			if (num < 15f)
			{
				num = 15f;
			}
			return num;
		}
	}

	public override Material DrawMat => curDrawFrame;

	public override Vector3 DrawPos => base.DrawPos + new Vector3(curDrawOffset.x, 0f, curDrawOffset.y) * fireSize;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref fireSize, "FireSize");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		RecalcPathsOnAndAroundMe();
		Find.Tutor.Signal(TutorSignal.FireStarted);
	}

	public override void Destroy()
	{
		base.Destroy();
		RecalcPathsOnAndAroundMe();
	}

	private void RecalcPathsOnAndAroundMe()
	{
		IntVec3[] adjacentSquaresAndInside = Gen.AdjacentSquaresAndInside;
		foreach (IntVec3 intVec in adjacentSquaresAndInside)
		{
			IntVec3 intVec2 = base.Position + intVec;
			if (intVec2.InBounds())
			{
				Find.PathGrid.RecalculatePathCostAt(intVec2);
			}
		}
	}

	public override void Tick()
	{
		if (fireSize > 0.7f && Random.value < fireSize * 0.01f)
		{
			MoteMaker.ThrowMicroSparks(DrawPos);
		}
		ticksUntilSmoke--;
		if (ticksUntilSmoke <= 0)
		{
			MoteMaker.ThrowSmoke(DrawPos, fireSize);
			if (parent == null)
			{
				Vector3 spawnLoc = DrawPos + fireSize * new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f);
				MoteMaker.ThrowFireGlow(spawnLoc, fireSize);
			}
			float num = fireSize / 2f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
			ticksUntilSmoke = SmokeIntervalRange.Lerped(num) + (int)(10f * Random.value);
		}
		ticksUntilFrameChange--;
		if (ticksUntilFrameChange <= 0)
		{
			ticksUntilFrameChange = FrameChangeIntervalRange.RandomInRange;
			Material material = curDrawFrame;
			while (curDrawFrame == material)
			{
				curDrawFrame = def.folderDrawMats.RandomElement();
			}
			curDrawOffset = new Vector3(Random.Range(-0.05f, 0.05f), 0f, Random.Range(-0.05f, 0.05f));
		}
		if (parent == null && fireSize > 0.6f)
		{
			float num2 = fireSize * 0.01f;
			foreach (Thing item in Find.Grids.ThingsAt(base.Position).ListFullCopy())
			{
				if (Random.value < num2 && item.CanEverAttachFire())
				{
					item.TryIgnite(fireSize / 2f);
				}
			}
		}
		ticksSinceDamage++;
		if ((float)ticksSinceDamage >= TicksBeforeDamage)
		{
			Thing thing = null;
			if (parent != null)
			{
				thing = parent;
			}
			else
			{
				List<Thing> list = (from t in Find.Grids.ThingsAt(base.Position)
					where t.def.Flammable
					select t).ToList();
				if (list.Count > 0)
				{
					thing = list.RandomElement();
				}
				if (thing == null)
				{
					Destroy();
					return;
				}
			}
			if (!thing.CanEverAttachFire() || fireSize > 0.6f)
			{
				thing.TakeDamage(new DamageInfo(DamageType.Flame, 1));
			}
			ticksSinceDamage = 0;
		}
		ticksSinceSpark++;
		if ((float)ticksSinceSpark >= TicksBeforeSpark)
		{
			ThrowSpark();
			ticksSinceSpark = 0;
		}
		fireSize += 0.0005f;
		if (Find.WeatherManager.RainRate > 0.01f && ((Find.Grids.BlockerAt(base.Position)?.def.holdsRoof ?? false) || !Find.RoofGrid.SquareIsRoofed(base.Position)) && Random.value < 0.04f)
		{
			TakeDamage(new DamageInfo(DamageType.Extinguish, 2));
		}
	}

	public override void Draw()
	{
		float num = fireSize / 1.2f;
		if (num > 1.2f)
		{
			num = 1.2f;
		}
		Vector3 s = new Vector3(num, 1f, num);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(DrawPos, rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, DrawMat, 0);
	}

	protected override void ApplyDamage(DamageInfo d)
	{
		if (d.type == DamageType.Extinguish)
		{
			fireSize -= (float)d.Amount / 100f;
			if (fireSize <= 0.1f)
			{
				Destroy();
			}
		}
	}

	protected void ThrowSpark()
	{
		IntVec3 position = base.Position;
		position = ((!(Random.value < 0.8f)) ? (base.Position + Gen.ManualRadialPattern[Random.Range(10, 21)]) : (base.Position + Gen.ManualRadialPattern[Random.Range(1, 9)]));
		Spark spark = (Spark)ThingMaker.Spawn(EntityType.Proj_Spark, base.Position, IntRot.random);
		spark.Launch(new TargetPack(position));
	}
}
