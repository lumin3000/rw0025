using UnityEngine;

public class DropPod : Thing
{
	private const float OpenSoundVolume = 0.1f;

	private int ticksToOpen = 110;

	public DropPodContentsInfo contents;

	private static readonly AudioClip OpenClip = Res.LoadSound("Various/DropPodOpen");

	public override Mesh DrawMesh => MeshPool.plane30Twist;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref ticksToOpen, "TicksToOpen");
		Scribe.LookSaveable(ref contents, "Contents");
	}

	public override void Tick()
	{
		ticksToOpen--;
		if (ticksToOpen <= 0)
		{
			PodOpen();
		}
	}

	private void PodOpen()
	{
		Destroy();
		ThingMaker.Spawn(contents.containedThing, base.Position, rotation);
		GenSound.PlaySoundAt(base.Position, OpenClip, 0.1f);
	}
}
