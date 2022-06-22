using System;
using UnityEngine;

public class Building_Door : Building
{
	private const int AutomaticCloseDelayTicks = 60;

	private const int VisualOpenTicksMax = 10;

	private const float VisualDoorOffsetStart = 0.25f;

	private const float VisualDoorOffsetEnd = 0.75f;

	private const float DoorSoundVolume = 0.04f;

	private const float DoorSoundVolumeManual = 0.1f;

	public CompPowerTrader powerComp;

	public bool isOpen = true;

	protected int ticksUntilClose;

	protected int visualTicksOpen;

	private static readonly Material MatDoor = MaterialPool.MatFrom("Icons/Building/Door");

	private static readonly AudioClip DoorOpenSound = Res.LoadSound("Door/DoorOpen");

	private static readonly AudioClip DoorCloseSound = Res.LoadSound("Door/DoorClose");

	private static readonly AudioClip DoorOpenSoundManual = Res.LoadSound("Door/DoorOpenManual");

	private static readonly AudioClip DoorCloseSoundManual = Res.LoadSound("Door/DoorCloseManual");

	public bool CloseBlocked
	{
		get
		{
			foreach (Thing item in Find.Grids.ThingsAt(base.Position))
			{
				if (!item.def.IsPlant || item.def.passability != 0)
				{
					if (item.def.category == EntityCategory.Pawn)
					{
						return true;
					}
					if (item.def.category == EntityCategory.SmallObject)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		powerComp = GetComp<CompPowerTrader>();
	}

	public override void Tick()
	{
		base.Tick();
		if (!isOpen && visualTicksOpen > 0)
		{
			visualTicksOpen--;
		}
		if (!isOpen)
		{
			return;
		}
		if (visualTicksOpen < 10)
		{
			visualTicksOpen++;
		}
		if (Find.Grids.SquareContains(base.Position, EntityType.Pawn))
		{
			ticksUntilClose = 60;
			return;
		}
		ticksUntilClose--;
		if (powerComp.PowerOn && ticksUntilClose <= 0)
		{
			DoorTryClose();
		}
	}

	public void Notify_PawnApproaching(Pawn p)
	{
		if (WillOpenFor(p) && powerComp.PowerOn)
		{
			DoorOpen();
		}
	}

	public bool WillOpenFor(Pawn p)
	{
		return p.inventory.Has(EntityType.DoorKey);
	}

	public override bool BlocksPawn(Pawn p)
	{
		if (isOpen)
		{
			return false;
		}
		return !WillOpenFor(p);
	}

	protected void DoorOpen()
	{
		isOpen = true;
		ticksUntilClose = 60;
		if (powerComp.PowerOn)
		{
			GenSound.PlaySoundAt(base.Position, DoorOpenSound, 0.04f);
		}
		else
		{
			GenSound.PlaySoundAt(base.Position, DoorOpenSoundManual, 0.1f);
		}
	}

	protected void DoorTryClose()
	{
		if (!CloseBlocked)
		{
			isOpen = false;
			if (powerComp.PowerOn)
			{
				GenSound.PlaySoundAt(base.Position, DoorCloseSound, 0.04f);
			}
			else
			{
				GenSound.PlaySoundAt(base.Position, DoorCloseSoundManual, 0.1f);
			}
		}
	}

	public void StartManualOpenBy(Pawn opener)
	{
		DoorOpen();
	}

	public void StartManualCloseBy(Pawn closer)
	{
		DoorTryClose();
	}

	public override void Draw()
	{
		rotation = DoorRotationAt(base.Position);
		float num = (float)visualTicksOpen / 10f;
		float num2 = 0.25f + 0.5f * num;
		for (int i = 0; i < 2; i++)
		{
			Vector3 drawPos = DrawPos;
			drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.DoorMoveable);
			Vector3 vector = default(Vector3);
			vector = ((i != 0) ? new Vector3(0f, 0f, -1f) : new Vector3(0f, 0f, 1f));
			IntRot intRot = rotation;
			intRot.Rotate(RotationDirection.Clockwise);
			vector = intRot.AsQuat * vector;
			drawPos += vector * num2;
			Vector3 s = new Vector3(0.5f, 0.5f, 0.5f);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, intRot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MatDoor, 0);
		}
		Comps_Draw();
	}

	public static IntRot DoorRotationAt(IntVec3 Pos)
	{
		Func<IntVec3, bool> func = delegate(IntVec3 Vec)
		{
			if (!Vec.Walkable())
			{
				return true;
			}
			Blueprint blueprint = Find.Grids.ThingAt(Vec, EntityType.Blueprint) as Blueprint;
			return (blueprint != null && blueprint.def.entityDefToBuild.passability == Traversability.Impassable) ? true : false;
		};
		IntVec3[] array = new IntVec3[4]
		{
			Pos + new IntVec3(1, 0, 0),
			Pos + new IntVec3(-1, 0, 0),
			Pos + new IntVec3(0, 0, 1),
			Pos + new IntVec3(0, 0, -1)
		};
		for (int i = 0; i < 4; i++)
		{
			if (func(array[i]))
			{
				if (i <= 1)
				{
					return IntRot.north;
				}
				return IntRot.east;
			}
		}
		return IntRot.north;
	}
}
