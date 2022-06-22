using System;
using System.Collections.Generic;
using UnityEngine;

public class Genner_BaseTerrain
{
	private const float SanctityFadeFactor = 0.25f;

	private const float ThreshRockRoofThick = 0.4f;

	private const float ThreshRockRoofThin = 0.43f;

	private const float ThreshHighRocks = 0.45f;

	private const float ThreshRoughStone = 0.46f;

	private const float ThreshGravel = 0.47f;

	private const float ThreshSoil = 0.51f;

	private const float GrossFadeSpan = 0.43f;

	private const float GrossFadeStrength = 0.65f;

	private const float ThreshStoneDebris = 0.55f;

	private const float StoneDebrisProbabilityPerSquare = 0.01f;

	private const float RockRubbleProbability = 0.5f;

	private PerlinNoiseOctaves2D pNoise;

	private Func<IntVec3, float> baseGrossFadeFactor;

	public Genner_BaseTerrain()
	{
		float fadeSpan = (float)Find.Map.Size.x * 0.43f;
		float fadeStart = (float)Find.Map.Size.x * 0.57f;
		int num = UnityEngine.Random.Range(0, 4);
		if (num == 0)
		{
			baseGrossFadeFactor = (IntVec3 sq) => ((float)sq.x - fadeStart) / fadeSpan;
		}
		if (num == 1)
		{
			baseGrossFadeFactor = (IntVec3 sq) => ((float)sq.z - fadeStart) / fadeSpan;
		}
		if (num == 2)
		{
			baseGrossFadeFactor = (IntVec3 sq) => ((float)(Find.Map.Size.x - sq.x) - fadeStart) / fadeSpan;
		}
		if (num == 3)
		{
			baseGrossFadeFactor = (IntVec3 sq) => ((float)(Find.Map.Size.z - sq.z) - fadeStart) / fadeSpan;
		}
		pNoise = new PerlinNoiseOctaves2D(new PerlinNoiseOctaves2DConfig
		{
			AmpLow = 0.4f,
			AmpMid = 0.3f,
			AmpHigh = 0.3f,
			FieldSize = new Vector2(Find.Map.Size.x, Find.Map.Size.z)
		});
	}

	public void AddTallRocks()
	{
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			float num = MapGenerator.sanctity.SanctityAt(allSquare);
			if (num < 1f)
			{
				if (GrossFadedNoiseAt(allSquare) < 0.45f * (1f - num * 0.25f))
				{
					ThingMaker.Spawn(EntityType.Rock, allSquare);
				}
				if (GrossFadedNoiseAt(allSquare) < 0.4f * (1f - num * 0.25f))
				{
					Find.RoofGrid.SetSquareRoofed(allSquare, EntityType.Roof_RockThick);
				}
				else if (GrossFadedNoiseAt(allSquare) < 0.43f * (1f - num * 0.25f))
				{
					Find.RoofGrid.SetSquareRoofed(allSquare, EntityType.Roof_RockThin);
				}
			}
		}
	}

	public void AddRockDebris()
	{
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			float num = MapGenerator.sanctity.SanctityAt(allSquare);
			if (num < 1f && pNoise.NoiseAt(allSquare.x, allSquare.z) < 0.55f * (1f - num * 0.25f) && UnityEngine.Random.value < 0.01f)
			{
				GrowLowRockFormationFrom(allSquare);
			}
		}
	}

	public void SetTerrains()
	{
		TerrainDefinition newTerr = TerrainDefDatabase.TerrainWithLabel("Sand");
		TerrainDefinition newTerr2 = TerrainDefDatabase.TerrainWithLabel("Soil");
		TerrainDefinition newTerr3 = TerrainDefDatabase.TerrainWithLabel("Gravel");
		TerrainDefinition newTerr4 = TerrainDefDatabase.TerrainWithLabel("Rough stone");
		TerrainGrid terrainGrid = Find.TerrainGrid;
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			float num = GrossFadedNoiseAt(allSquare);
			if (num < 0.46f)
			{
				terrainGrid.SetTerrain(allSquare, newTerr4);
			}
			else if (num < 0.47f)
			{
				terrainGrid.SetTerrain(allSquare, newTerr3);
			}
			else if (num < 0.51f)
			{
				terrainGrid.SetTerrain(allSquare, newTerr2);
			}
			else
			{
				terrainGrid.SetTerrain(allSquare, newTerr);
			}
		}
	}

	private void GrowLowRockFormationFrom(IntVec3 Root)
	{
		ThingDefinition filthDef = ThingDefDatabase.ThingDefNamed("RockRubble");
		List<IntVec3> list = new List<IntVec3>(Gen.CardinalDirections);
		list.Remove(list.RandomElement());
		IntVec3 intVec = Root;
		while (true)
		{
			intVec += list.RandomElement();
			if (!intVec.InBounds() || Find.Grids.BlockerAt(intVec) != null)
			{
				break;
			}
			float num = MapGenerator.sanctity.SanctityAt(intVec);
			if (num >= 0.99f || pNoise.NoiseAt(intVec.x, intVec.z) > 0.55f * (1f - num * 0.25f))
			{
				break;
			}
			ThingMaker.Spawn(EntityType.DebrisRock, intVec, IntRot.random);
			IntVec3[] adjacentSquaresAndInside = Gen.AdjacentSquaresAndInside;
			foreach (IntVec3 intVec2 in adjacentSquaresAndInside)
			{
				if (UnityEngine.Random.value < 0.5f)
				{
					IntVec3 sq = intVec + intVec2;
					if (sq.InBounds())
					{
						FilthUtility.AddFilthAt(sq, filthDef);
					}
				}
			}
		}
	}

	private float GrossFadedNoiseAt(IntVec3 sq)
	{
		float num = baseGrossFadeFactor(sq);
		float num2 = ((!(num > 0f)) ? 1f : (1f - num));
		return pNoise.NoiseAt(sq.x, sq.z) * num2;
	}
}
