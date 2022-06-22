using UnityEngine;

public class PawnRenderer : Saveable
{
	private const float CarriedThingDrawAngle = 16f;

	private const float DownedAngleWidth = 30f;

	private const float DamageTakenDownedAngleShift = 10f;

	private const int IncapWigglePeriod = 400;

	private const int IncapWiggleLength = 80;

	private const float IncapWiggleSpeed = 0.2f;

	private const int TicksBetweenIncapIcons = 200;

	private Pawn pawn;

	private DamageFlasher flasher;

	private PawnHeadOverlays headOverlays;

	private PawnWoundDrawer woundOverlays;

	public Material baseIcon;

	public PawnHeadGraphic graphicHead;

	public PawnBodyGraphic graphicBody;

	private float downedAngle = RandomDownedAngle;

	private int ticksToIncapIcon;

	private static readonly Vector3 ShadowDrawOffset = new Vector3(0f, 0f, -0.3f);

	private Thing CarriedThing => pawn.carryHands.carriedThing;

	public Material CurMatBody
	{
		get
		{
			Material baseMat = graphicBody.body.MatAt(pawn.rotation);
			if (pawn.IsInBed())
			{
				baseMat = ((!pawn.IsSleeping()) ? graphicBody.body.MatFront : graphicBody.body.MatFront);
			}
			if (pawn.Incapacitated)
			{
				baseMat = graphicBody.MatIncap;
			}
			if (pawn.health == 0)
			{
				baseMat = graphicBody.MatDead;
			}
			return flasher.GetCurMatBasedOn(baseMat);
		}
	}

	public Material CurMatHead
	{
		get
		{
			Material baseMat = graphicHead.head.MatAt(pawn.rotation);
			if (pawn.IsInBed())
			{
				baseMat = ((!pawn.IsSleeping()) ? graphicHead.head.MatFront : graphicHead.matSleeping);
			}
			if (pawn.Incapacitated)
			{
				baseMat = graphicHead.matIncap;
			}
			if (pawn.health == 0)
			{
				baseMat = graphicHead.matDead;
			}
			return flasher.GetCurMatBasedOn(baseMat);
		}
	}

	public Mesh CurMeshBody
	{
		get
		{
			if (pawn.rotation != IntRot.west)
			{
				if (pawn.raceDef.pawnOverdraw)
				{
					return MeshPool.plane24Back;
				}
				return MeshPool.plane12Back;
			}
			if (pawn.raceDef.pawnOverdraw)
			{
				return MeshPool.plane24BackFlip;
			}
			return MeshPool.plane12BackFlip;
		}
	}

	public Mesh CurMeshHead
	{
		get
		{
			if (pawn.rotation != IntRot.west)
			{
				return MeshPool.plane14;
			}
			return MeshPool.plane14Flip;
		}
	}

	private Mesh CurMeshShadow => pawn.raceDef.shadowMesh;

	private static float RandomDownedAngle
	{
		get
		{
			float num = Random.Range(60f, 120f);
			if (Random.value < 0.5f)
			{
				num += 180f;
			}
			return num;
		}
	}

	private bool PawnIsDowned => pawn.Incapacitated || pawn.health == 0;

	public PawnRenderer(Pawn pawn)
	{
		this.pawn = pawn;
		flasher = new DamageFlasher(pawn);
		headOverlays = new PawnHeadOverlays(pawn);
		woundOverlays = new PawnWoundDrawer(pawn);
	}

	public void ExposeData()
	{
		string value = string.Empty;
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			value = graphicBody.bodyName;
		}
		Scribe.LookField(ref value, "BodyName");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			graphicBody = PawnGraphicDatabase.GetBodyNamed(value);
		}
		if (!pawn.kindDef.UseStandardGraphics)
		{
			string value2 = string.Empty;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				value2 = graphicHead.headName;
			}
			Scribe.LookField(ref value2, "HeadName");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				graphicHead = PawnGraphicDatabase.GetHeadNamed(value2, pawn.gender);
			}
		}
	}

	public void ResolveGraphics()
	{
		if (graphicBody == null)
		{
			if (pawn.kindDef.UseStandardGraphics)
			{
				graphicBody = PawnGraphicDatabase.GetBodyNamed(pawn.kindDef.bodyGraphicNames.RandomElement());
			}
			else if (pawn.story == null)
			{
				Debug.LogError(string.Concat(pawn, " tried to resolve character graphics without having a story. Defaulting to Fatman."));
				graphicBody = PawnGraphicDatabase.GetBodyNamed("Fatman");
			}
			else
			{
				graphicBody = PawnGraphicDatabase.GetBodyNamed(pawn.story.Adulthood.bodyGraphicNames[pawn.gender]);
				graphicHead = PawnGraphicDatabase.GetHeadRandom(pawn.gender);
			}
		}
	}

	private void RenderHeadAt(Vector3 loc, Quaternion quat)
	{
		if (graphicHead != null)
		{
			Graphics.DrawMesh(CurMeshHead, loc + Altitudes.AltIncVect, quat, CurMatHead, 0);
			headOverlays.RenderHeadOverlays(loc, quat, CurMeshHead);
		}
	}

	private void RenderBodyAt(Vector3 loc, Quaternion quat)
	{
		Graphics.DrawMesh(CurMeshBody, loc, quat, CurMatBody, 0);
		woundOverlays.RenderOverBody(loc, CurMeshBody, quat);
	}

	public void RenderPawnAt(Vector3 drawLoc)
	{
		Building_Bed building_Bed = pawn.CurrentBed();
		if (building_Bed != null)
		{
			RenderPawnInBed(building_Bed);
		}
		else if (PawnIsDowned)
		{
			RenderPawnDownedAt(drawLoc);
		}
		else
		{
			RenderPawnNormalAt(drawLoc);
		}
	}

	private void RenderPawnInBed(Building_Bed pawnBed)
	{
		IntRot rotation = pawnBed.rotation;
		rotation.AsInt += 2;
		Quaternion asQuat = rotation.AsQuat;
		Vector3 vector = pawnBed.Position.ToVector3ShiftedWithAltitude(pawnBed.def.altitudeLayer) + Altitudes.AltIncVect * 2f;
		RenderHeadAt(vector, asQuat);
		if (pawnBed.def.bed_ShowSleeperBody)
		{
			float num = 0f - graphicBody.headOffsets[2].z;
			Vector3 vector2 = rotation.FacingSquare.ToVector3();
			Vector3 loc = vector + vector2 * num - Altitudes.AltIncVect;
			RenderBodyAt(loc, asQuat);
		}
	}

	private void RenderPawnDownedAt(Vector3 drawLoc)
	{
		Vector3 vector = graphicBody.headOffsets[2];
		Quaternion quaternion = Quaternion.AngleAxis(downedAngle, Vector3.up);
		vector = quaternion * vector;
		RenderHeadAt(drawLoc + vector + Altitudes.AltIncVect, quaternion);
		RenderBodyAt(drawLoc, quaternion);
	}

	private void RenderPawnNormalAt(Vector3 drawLoc)
	{
		RenderBodyAt(drawLoc, Quaternion.identity);
		Vector3 vector = graphicBody.headOffsets[pawn.rotation.AsInt] + Altitudes.AltIncVect;
		if (pawn.rotation == IntRot.north)
		{
			vector.y -= 0.08f;
		}
		RenderHeadAt(drawLoc + vector, Quaternion.identity);
		if (pawn.carryHands.carriedThing != null)
		{
			Vector3 vector2 = drawLoc + Altitudes.AltIncVect * 3f;
			if (CarriedThing is Pawn || CarriedThing is Corpse)
			{
				vector2.x += 0.5f;
				CarriedThing.DrawAt(vector2);
			}
			else
			{
				float angle = 0f;
				if (CarriedThing.def.eType == EntityType.Pawn && !((Pawn)CarriedThing).Incapacitated)
				{
					vector2 += new Vector3(0.32f, 0f, -0.05f);
				}
				else
				{
					vector2 += new Vector3(0.18f, 0f, 0.05f);
					angle = 16f;
				}
				Graphics.DrawMesh(pawn.carryHands.carriedThing.DrawMesh, vector2, Quaternion.AngleAxis(angle, Vector3.up), pawn.carryHands.carriedThing.DrawMat, 0);
			}
		}
		Graphics.DrawMesh(CurMeshShadow, drawLoc + ShadowDrawOffset, Quaternion.identity, MatBases.SunShadow, 0);
	}

	public void Notify_DamageApplied(DamageInfo dam)
	{
		flasher.Notify_DamageApplied(dam);
		if (!PawnIsDowned || !dam.type.HasForcefulImpact())
		{
			return;
		}
		downedAngle += 10f * Random.Range(-1f, 1f);
		if (downedAngle > 300f)
		{
			downedAngle = 300f;
		}
		if (downedAngle < 60f)
		{
			downedAngle = 60f;
		}
		if (downedAngle > 120f && downedAngle < 240f)
		{
			if (downedAngle > 180f)
			{
				downedAngle = 240f;
			}
			else
			{
				downedAngle = 120f;
			}
		}
	}

	public void RendererTick()
	{
		if (pawn.Incapacitated && pawn.spawnedInWorld && !pawn.IsInBed())
		{
			ticksToIncapIcon--;
			if (ticksToIncapIcon <= 0)
			{
				MoteMaker.ThrowIncapIcon(pawn.Position);
				ticksToIncapIcon = 200;
			}
			int num = Find.TickManager.tickCount % 400 * 2;
			if (num < 80)
			{
				downedAngle += 0.2f;
			}
			else if (num < 480 && num >= 400)
			{
				downedAngle -= 0.2f;
			}
		}
	}
}
