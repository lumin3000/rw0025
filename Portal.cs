using System.Collections.Generic;
using UnityEngine;

public class Portal : ThingWithComponents
{
	protected class QueuedPawn
	{
		public Pawn pawn;

		public int SpawnDelay;

		public QueuedPawn(Pawn newQueuedPawn, int newSpawnDelay)
		{
			pawn = newQueuedPawn;
			SpawnDelay = newSpawnDelay;
		}

		public void SpawnFrom(IntVec3 Loc)
		{
			ThingMaker.Spawn(pawn, Loc, IntRot.north);
		}
	}

	protected const float GraphicRotationPerTick = 1f;

	protected const int ScanRadius = 1;

	protected const int TicksBetweenScans = 50;

	protected const int MinSpewDelay = 50;

	protected const int MaxSpewDelay = 50;

	protected int TicksUntilNextSpew;

	protected List<QueuedPawn> PawnQueue = new List<QueuedPawn>();

	protected bool Spewing;

	protected int TicksUntilNextScan;

	protected float CurGraphicRotation;

	protected readonly Material ScanRadiusMat = MaterialPool.MatFrom("Icons/Special/PortalScanRad");

	public Portal()
	{
		GenerateNewPawnQueue();
		TicksUntilNextScan = Random.Range(0, 51);
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
	}

	public void UsedByForOneTick(Pawn p)
	{
	}

	public void UseEndingBy(Pawn p)
	{
	}

	public void StartSpewCountDown()
	{
		if (!CountingDownToSpew())
		{
			TicksUntilNextSpew = Random.Range(50, 51);
		}
	}

	private void ScanForDisturbances()
	{
		if (CountingDownToSpew())
		{
			return;
		}
		int num = Gen.NumSquaresToFillForRadius_ManualRadialPattern(1);
		for (int i = 0; i < num; i++)
		{
			IntVec3 sq = base.Position + Gen.ManualRadialPattern[i];
			foreach (Thing item in Find.Grids.ThingsAt(sq))
			{
				if (item.def.category == EntityCategory.Pawn)
				{
					StartSpewCountDown();
				}
			}
		}
	}

	private bool CountingDownToSpew()
	{
		return TicksUntilNextSpew > 0;
	}

	public override void Tick()
	{
		if (!CountingDownToSpew())
		{
			TicksUntilNextScan--;
			if (TicksUntilNextScan <= 0)
			{
				ScanForDisturbances();
				TicksUntilNextScan = 50;
			}
		}
		if (Spewing)
		{
			List<QueuedPawn> list = new List<QueuedPawn>();
			foreach (QueuedPawn item in PawnQueue)
			{
				item.SpawnDelay--;
				if (item.SpawnDelay <= 0)
				{
					item.SpawnFrom(base.Position);
					list.Add(item);
				}
			}
			foreach (QueuedPawn item2 in list)
			{
				PawnQueue.Remove(item2);
			}
			if (PawnQueue.Count == 0)
			{
				FinishedSpewing();
			}
		}
		if (TicksUntilNextSpew > 0)
		{
			TicksUntilNextSpew--;
			if (TicksUntilNextSpew <= 0)
			{
				BeginSpew();
			}
		}
		CurGraphicRotation += 1f;
	}

	private void BeginSpew()
	{
		Spewing = true;
	}

	private void FinishedSpewing()
	{
		Destroy();
	}

	private void GenerateNewPawnQueue()
	{
		PawnQueue.Clear();
		int num = Random.Range(1, 7);
		for (int i = 0; i < num; i++)
		{
			QueueSingleEnemy(i * 200);
		}
	}

	private void QueueSingleEnemy(int newDelay)
	{
		Pawn newQueuedPawn = PawnMaker.GeneratePawn("Scavenger", TeamType.Raider);
		QueuedPawn item = new QueuedPawn(newQueuedPawn, newDelay);
		PawnQueue.Add(item);
	}

	public override void Draw()
	{
		if (!CountingDownToSpew() && !Spewing)
		{
			Graphics.DrawMesh(MeshPool.plane30, base.Position.ToVector3ShiftedWithAltitude(def.altitude - 0.05f), Quaternion.identity, ScanRadiusMat, 0);
		}
		Graphics.DrawMesh(MeshPool.plane10, base.Position.ToVector3ShiftedWithAltitude(def.altitude), Quaternion.AngleAxis(CurGraphicRotation, Vector3.up), def.drawMat, 0);
	}
}
