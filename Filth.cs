using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Filth : Thing, Interactive
{
	private const int MaxThickness = 5;

	private const float PositionVariance = 0.45f;

	private const float SizeVariance = 0.2f;

	private const int MinAgeToPickUp = 400;

	private const int MaxNumSources = 3;

	public int thickness = 1;

	private float cleaningWorkDone;

	public List<string> sources = new List<string>();

	private int spawnTick;

	public bool CanPickUpNow => def.canBePickedUp && thickness > 1 && Find.TickManager.tickCount - spawnTick > 400;

	public bool CanBeThickened => thickness < 5;

	public override string Label
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Label);
			if (sources.Count > 0)
			{
				stringBuilder.Append(" of ");
				for (int i = 0; i < sources.Count; i++)
				{
					stringBuilder.Append(sources[i]);
					if (i < sources.Count - 2)
					{
						stringBuilder.Append(", ");
					}
					if (i == sources.Count - 2)
					{
						stringBuilder.Append(" and ");
					}
				}
			}
			stringBuilder.Append(" x" + thickness);
			return stringBuilder.ToString();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref thickness, "Thickness");
		Scribe.LookField(ref cleaningWorkDone, "CleaningWorkDone", 0f);
		Scribe.LookList(ref sources, "Sources");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		FilthList.Notify_FilthSpawned(this);
		spawnTick = Find.TickManager.tickCount;
	}

	public override void Destroy()
	{
		base.Destroy();
		FilthList.Notify_FilthDestroyed(this);
	}

	public JobCondition InteractedWith(ReservationType iType, Pawn p)
	{
		cleaningWorkDone += 1f;
		if (cleaningWorkDone > def.cleaningWorkToReduceThickness)
		{
			ThinFilth();
			if (destroyed)
			{
				return JobCondition.Succeeded;
			}
		}
		return JobCondition.Ongoing;
	}

	public override IEnumerable<MapMeshPiece> EmitMapMeshPieces()
	{
		Vector3 trueCenter = this.TrueCenter();
		Random.seed = base.Position.GetHashCode();
		for (int i = 0; i < thickness; i++)
		{
			Material mat = def.RandomDrawMat;
			Vector3 adjustedCenter = trueCenter + new Vector3(Random.Range(-0.45f, 0.45f), 0f, Random.Range(-0.45f, 0.45f));
			Vector2 planeSize = new Vector2(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
			float rot = Random.Range(0, 361);
			bool flipped = Random.value < 0.5f;
			yield return new MapMeshPiece_Plane(adjustedCenter, planeSize, mat, rot, flipped);
		}
	}

	public void AddSource(string newSource)
	{
		while (sources.Count > 3)
		{
			sources.RemoveAt(0);
		}
		foreach (string source in sources)
		{
			if (source == newSource)
			{
				return;
			}
		}
		sources.Add(newSource);
	}

	public void AddSources(List<string> sources)
	{
		if (sources == null)
		{
			return;
		}
		foreach (string source in sources)
		{
			AddSource(source);
		}
	}

	public void ThickenFilth()
	{
		thickness++;
		UpdateMesh();
	}

	public void ThinFilth()
	{
		thickness--;
		if (spawnedInWorld)
		{
			if (thickness == 0)
			{
				Destroy();
			}
			else
			{
				UpdateMesh();
			}
		}
	}

	private void UpdateMesh()
	{
		Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things);
	}

	public bool CanDropAt(IntVec3 sq)
	{
		if (def.isTerrainSourceFilth)
		{
			return Find.TerrainGrid.TerrainAt(sq).acceptTerrainSourceFilth;
		}
		return true;
	}
}
