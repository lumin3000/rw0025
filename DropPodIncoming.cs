using UnityEngine;

public class DropPodIncoming : Thing
{
	protected const int MinTicksToImpact = 120;

	protected const int MaxTicksToImpact = 200;

	private const int SoundAnticipationTicks = 100;

	private const float LandSoundVolume = 0.08f;

	public DropPodContentsInfo contents;

	protected int ticksToImpact = 120;

	private bool soundPlayed;

	private static readonly AudioClip LandClip = Res.LoadSound("Various/DropPodFall");

	private static readonly Material ShadowMat = MaterialPool.MatFrom("Icons/Special/DropPodShadow", MatBases.Transparent);

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		ticksToImpact = Random.Range(120, 201);
		if (Find.RoofGrid.Roofed(base.Position))
		{
			Debug.LogWarning("Drop pod dropped on roof at " + base.Position);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref ticksToImpact, "TicksToImpact");
		Scribe.LookSaveable(ref contents, "Contents");
	}

	public override void Tick()
	{
		ticksToImpact--;
		if (ticksToImpact <= 0)
		{
			PodImpact();
		}
		if (!soundPlayed && ticksToImpact < 100)
		{
			soundPlayed = true;
			GenSound.PlaySoundAt(base.Position, LandClip, 0.08f);
		}
	}

	public override void Draw()
	{
		Vector3 drawLoc = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Overworld);
		float num = (float)(ticksToImpact * ticksToImpact) * 0.01f;
		drawLoc.x -= num * 0.4f;
		drawLoc.z += num * 0.6f;
		DrawAt(drawLoc);
		float num2 = 2f + (float)ticksToImpact / 100f;
		Vector3 s = new Vector3(num2, 1f, num2);
		Matrix4x4 matrix = default(Matrix4x4);
		Vector3 drawPos = DrawPos;
		drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
		matrix.SetTRS(drawPos, rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10Back, matrix, ShadowMat, 0);
	}

	private void PodImpact()
	{
		for (int i = 0; i < 6; i++)
		{
			Vector3 spawnLoc = base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f);
			MoteMaker.ThrowDustPuff(spawnLoc, 1.2f);
		}
		MoteMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), 2f);
		DropPod dropPod = (DropPod)ThingMaker.MakeThing(EntityType.DropPod);
		dropPod.contents = contents;
		ThingMaker.Spawn(dropPod, base.Position, rotation);
		Destroy();
	}
}
