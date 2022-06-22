using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Plant : Thing, Interactive, Edible
{
	private const float RotDamagePerTick = 0.005f;

	public const float BaseGrowthPercent = 0.05f;

	private const int MinFoodFromFoodYieldingPlants = 2;

	private const float TotalWork_Harvest = 150f;

	private const float TotalWork_Sow = 220f;

	private const float MaxAirPressureForDOT = 0.6f;

	private const float SuffocationMaxDOTPerTick = 0.01f;

	private const float GridPosRandomnessFactor = 0.3f;

	private const float MinGrowthToEat = 0.8f;

	private const int TicksWithoutLightBeforeRot = 50000;

	private PlantReproducer reproducer;

	public float growthPercent;

	private float workToSowComplete = 220f;

	private float workToHarvestComplete = 150f;

	private int age;

	private int ticksSinceLit;

	private static readonly Material MatSowing = MaterialPool.MatFrom("Icons/Plant/Plant_Sowing");

	protected static readonly AudioClip SoundHarvestReady = Res.LoadSound("Various/HarvestReady");

	public float CurNutrition => def.food.nutrition;

	public bool HarvestableNow => def.plant.Harvestable && growthPercent > 0.8f;

	public bool EdibleNow => growthPercent > 0.8f;

	public bool Rotting
	{
		get
		{
			if (ticksSinceLit > 50000)
			{
				return true;
			}
			return def.plant.LimitedLifespan && age > def.plant.lifeSpan;
		}
	}

	private string GrowthPercentString => (growthPercent * 100f).ToString("##0");

	public override string LabelMouseover
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(def.label);
			stringBuilder.Append(" (" + GrowthPercentString + "% growth");
			if (Rotting)
			{
				stringBuilder.Append(", dying");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}

	private bool HasEnoughLightToGrow => Find.GlowGrid.PsychGlowAt(base.Position) >= def.plant.minGlowToGrow;

	public override Material DrawMat
	{
		get
		{
			if (LifeStage == PlantLifeStage.Sowing)
			{
				return MatSowing;
			}
			return base.DrawMat;
		}
	}

	private float LocalFertility => Find.FertilityGrid.FertilityAt(base.Position);

	public PlantLifeStage LifeStage
	{
		get
		{
			if (growthPercent < 0.001f)
			{
				return PlantLifeStage.Sowing;
			}
			if (growthPercent > 0.999f)
			{
				return PlantLifeStage.Mature;
			}
			return PlantLifeStage.Growing;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		if (reproducer == null && def.plant.SeedEmitAveragePer20kTicks > 0f)
		{
			reproducer = new PlantReproducer(this);
		}
		if (def.plant.LimitedLifespan && !Find.Map.initialized)
		{
			age = Random.Range(0, def.plant.lifeSpan + 3000);
		}
		else
		{
			age = 0;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref growthPercent, "GrowthPercent");
		Scribe.LookField(ref age, "Age", 0);
		Scribe.LookField(ref ticksSinceLit, "TicksSinceLit", 0);
		Scribe.LookField(ref workToHarvestComplete, "WorkToHarvestComplete", 150f);
		Scribe.LookField(ref workToSowComplete, "WorkToSowComplete", 220f);
	}

	public void Notify_Eaten()
	{
		PlantCollected();
	}

	private void PlantCollected()
	{
		if (def.plant.destroyOnHarvest)
		{
			Destroy();
			return;
		}
		growthPercent = 0.08f;
		Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things);
		foreach (Designation item in Find.DesignationManager.AllDesignationsOn(this).ToList())
		{
			if (item.dType == DesignationType.HarvestPlant)
			{
				item.Delete();
			}
		}
	}

	public override void TickRare()
	{
		bool hasEnoughLightToGrow = HasEnoughLightToGrow;
		if (!hasEnoughLightToGrow)
		{
			ticksSinceLit += 250;
		}
		else
		{
			ticksSinceLit = 0;
		}
		if (LifeStage == PlantLifeStage.Growing && hasEnoughLightToGrow)
		{
			float num = LocalFertility * def.plant.fertilityFactorGrowthRate + (1f - def.plant.fertilityFactorGrowthRate);
			float num2 = 1f;
			if (DateHandler.CurDayPercent < 0.2f || DateHandler.CurDayPercent > 0.8f)
			{
				num2 *= 0.5f;
			}
			growthPercent += num * num2 * 250f * (def.plant.growthPer20kTicks / 20000f);
			if (LifeStage == PlantLifeStage.Mature && !def.plant.wild)
			{
				GenSound.PlaySoundAt(base.Position, SoundHarvestReady, 0.15f);
			}
		}
		if (def.plant.LimitedLifespan)
		{
			age += 250;
		}
		if (Rotting)
		{
			int newAmount = Mathf.CeilToInt(1.25f);
			TakeDamage(new DamageInfo(DamageType.Rotting, newAmount));
		}
		if (!destroyed && reproducer != null)
		{
			reproducer.PlantReproducerTickRare();
		}
	}

	public JobCondition InteractedWith(ReservationType iType, Pawn interactor)
	{
		float num = 0.2f + 0.12f * (float)interactor.skills.LevelOf(SkillType.Growing);
		num *= interactor.healthTracker.CurEffectivenessPercent;
		float num2 = num * 0.8f;
		if (iType == ReservationType.Sowing)
		{
			if (LifeStage != 0)
			{
				Debug.LogError(string.Concat(this, " getting sowing work while not in Sowing life stage."));
			}
			workToSowComplete -= num2;
			if (workToSowComplete <= 0f)
			{
				growthPercent = 0.05f;
				workToHarvestComplete = 150f;
				return JobCondition.Succeeded;
			}
		}
		if (iType == ReservationType.Total)
		{
			workToHarvestComplete -= num2;
			if (workToHarvestComplete <= 0f)
			{
				ThingDefinition thingDefinition = EntityType.Food.DefinitionOfType();
				ThingResource thingResource = (ThingResource)ThingMaker.MakeThing(thingDefinition);
				thingResource.stackCount = FoodYieldNow();
				ThingMaker.Spawn(thingResource, GenMap.SpotNearForResourceSpawn(interactor.Position));
				PlantCollected();
				workToHarvestComplete = 150f;
				return JobCondition.Succeeded;
			}
		}
		return JobCondition.Ongoing;
	}

	private int FoodYieldNow()
	{
		if (def.plant.maxFoodYield == 0f)
		{
			return 0;
		}
		if (def.plant.maxFoodYield == 1f)
		{
			return 1;
		}
		float maxFoodYield = def.plant.maxFoodYield;
		maxFoodYield *= (float)health / (float)def.maxHealth;
		maxFoodYield *= growthPercent;
		int num = Gen.RandomRoundToInt(maxFoodYield);
		if (num < 2)
		{
			num = Mathf.Min(2, Gen.RandomRoundToInt(def.plant.maxFoodYield));
		}
		return num;
	}

	public override IEnumerable<MapMeshPiece> EmitMapMeshPieces()
	{
		Vector3 trueCenter = this.TrueCenter();
		Random.seed = base.Position.GetHashCode();
		float positionVariance = ((def.plant.maxMeshCount != 1) ? 0.5f : 0.05f);
		int meshCount = Mathf.CeilToInt(growthPercent * (float)def.plant.maxMeshCount);
		if (meshCount < 1)
		{
			meshCount = 1;
		}
		int gridWidth = 1;
		switch (def.plant.maxMeshCount)
		{
		case 1:
			gridWidth = 1;
			break;
		case 4:
			gridWidth = 2;
			break;
		case 9:
			gridWidth = 3;
			break;
		case 16:
			gridWidth = 4;
			break;
		case 32:
			gridWidth = 5;
			break;
		default:
			Debug.LogError(string.Concat(def, " must have plant.MaxMeshCount that is a perfect square."));
			break;
		}
		float gridSpacing = 1f / (float)gridWidth;
		List<int> posIndexList = new List<int>();
		for (int i = 0; i < def.plant.maxMeshCount; i++)
		{
			posIndexList.Add(i);
		}
		posIndexList.Shuffle();
		Vector3 adjustedCenter = default(Vector3);
		Vector2 planeSize = default(Vector2);
		int meshesYielded = 0;
		foreach (int posIndex in posIndexList)
		{
			float size = def.plant.sizeRange.LerpThroughRange(growthPercent);
			planeSize = new Vector2(size, size);
			if (def.plant.maxMeshCount == 1)
			{
				adjustedCenter = trueCenter + new Vector3(Random.Range(0f - positionVariance, positionVariance), 0f, Random.Range(0f - positionVariance, positionVariance));
				float squareBottom = Mathf.Floor(trueCenter.z);
				if (adjustedCenter.z - planeSize.y / 2f < squareBottom)
				{
					adjustedCenter.z = squareBottom + planeSize.y / 2f;
				}
			}
			else
			{
				adjustedCenter = base.Position.ToVector3();
				adjustedCenter.y = def.altitude;
				adjustedCenter.x += 0.5f * gridSpacing;
				adjustedCenter.z += 0.5f * gridSpacing;
				int xInd = posIndex / gridWidth;
				int zInd = posIndex % gridWidth;
				adjustedCenter.x += (float)xInd * gridSpacing;
				adjustedCenter.z += (float)zInd * gridSpacing;
				float gridPosRandomness = gridSpacing * 0.3f;
				adjustedCenter += new Vector3(Random.Range(0f - gridPosRandomness, gridPosRandomness), 0f, Random.Range(0f - gridPosRandomness, gridPosRandomness));
			}
			MapMeshPiece_Plane newPlane = new MapMeshPiece_Plane(UVflipped: Random.value < 0.5f, mat: def.RandomDrawMat, center: adjustedCenter, size: planeSize, rot: 0f);
			newPlane.colors[1].a = (newPlane.colors[2].a = (byte)(255f * def.plant.topWindExposure));
			newPlane.colors[0].a = (newPlane.colors[3].a = 0);
			yield return newPlane;
			meshesYielded++;
			if (meshesYielded >= meshCount)
			{
				break;
			}
		}
		if (def.sunShadowMesh != null)
		{
			float shadowOffsetFactor2 = 0.85f;
			shadowOffsetFactor2 = ((!(planeSize.y < 1f)) ? 0.81f : 0.6f);
			Vector3 sunShadowLoc = adjustedCenter;
			sunShadowLoc.z -= planeSize.y / 2f * shadowOffsetFactor2;
			sunShadowLoc.y -= 0.04f;
			yield return new MapMeshPiece_Mesh(sunShadowLoc, def.sunShadowMesh, MatBases.SunShadowFade);
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(GrowthPercentString + "% growth");
		if (LifeStage == PlantLifeStage.Sowing)
		{
			stringBuilder.AppendLine("Sowing work left:  " + workToSowComplete.ToString("######0"));
		}
		else if (LifeStage == PlantLifeStage.Growing)
		{
			if (!HasEnoughLightToGrow)
			{
				stringBuilder.AppendLine("Not growing now (needs to be " + def.plant.minGlowToGrow.HumanName().ToLower() + ").");
			}
			else
			{
				stringBuilder.AppendLine("Growing");
			}
			int numTicks = (int)((1f - growthPercent) * (20000f / def.plant.growthPer20kTicks));
			if (def.plant.Harvestable)
			{
				stringBuilder.AppendLine("Harvestable in " + numTicks.TicksInDaysString() + " of growth.");
			}
			else
			{
				stringBuilder.AppendLine("Fully grown in " + numTicks.TicksInDaysString() + " of growth.");
			}
		}
		else if (LifeStage == PlantLifeStage.Mature)
		{
			if (workToHarvestComplete < 149.99f)
			{
				stringBuilder.AppendLine("Harvesting work left:  " + workToHarvestComplete.ToString("######0"));
			}
			else if (def.plant.Harvestable)
			{
				stringBuilder.AppendLine("Ready to harvest.");
			}
			else
			{
				stringBuilder.AppendLine("Mature.");
			}
		}
		return stringBuilder.ToString();
	}

	public void CropBlighted()
	{
		Destroy();
	}

	public override void Destroy()
	{
		base.Destroy();
		Find.DesignationManager.RemoveAllDesignationsOn(this);
	}
}
