using System.Collections.Generic;
using UnityEngine;

public static class RoofMaker
{
	private class RoofGenRequest
	{
		private const int RoofGenDelay = 75;

		private const float StartPitchOffset = -0.25f;

		private const float PitchRiseRate = 0.04f;

		public Room room;

		public int requestMadeTick;

		private readonly List<IntVec3> squaresToEffect = new List<IntVec3>();

		private readonly int startingZIntercept;

		private bool completed;

		private bool failedRoof;

		private IntVec3 failedRoofSq = default(IntVec3);

		private SoundLooperThing looper;

		private int TicksBetweenSteps = 3;

		private static readonly AudioClip RoofMakeLoopSound = Res.LoadSound("Interface/DesignateDragLoop_Mech");

		private static readonly AudioClip RoofCompleteSound = Res.LoadSound("Building/RoofComplete");

		public bool ShouldRemove => completed || room.roomDeleted;

		public RoofGenRequest(Room room, int requestTick)
		{
			this.room = room;
			requestMadeTick = requestTick;
			if (room.squaresList.Count > 600)
			{
				completed = true;
				return;
			}
			foreach (IntVec3 squares in room.squaresList)
			{
				foreach (IntVec3 item in squares.AdjacentSquares8Way())
				{
					if (!Find.RoofGrid.SquareIsRoofed(item) && !squaresToEffect.Contains(item))
					{
						if (RoofCollapseChecker.IsSupported(item))
						{
							Find.RoofGrid.SetSquareRoofed(item, EntityType.Roof_Metal);
							squaresToEffect.Add(item);
						}
						else
						{
							failedRoofSq = item;
							failedRoof = true;
						}
					}
				}
			}
			if (squaresToEffect.Count == 0)
			{
				completed = true;
				return;
			}
			int num = 99999;
			IntVec3 intVec = default(IntVec3);
			foreach (IntVec3 item2 in squaresToEffect)
			{
				int num2 = item2.x - item2.z;
				if (num2 < num)
				{
					intVec = item2;
					num = num2;
				}
			}
			startingZIntercept = intVec.z - intVec.x;
		}

		public void RoofRequestTick()
		{
			if (looper != null)
			{
				looper.Maintain();
				looper.looperSource.pitch += 0.04f;
			}
			if (!room.roomDeleted)
			{
				int num = (Find.TickManager.tickCount - (requestMadeTick + 75)) / TicksBetweenSteps;
				if (num >= 0 && !DoRoofEffectAlongSlice(startingZIntercept - num))
				{
					Complete();
				}
			}
		}

		public void ForceInstantFinish()
		{
			Complete();
		}

		private void Complete()
		{
			if (failedRoof)
			{
				Find.LetterStack.ReceiveLetter(new Letter("You weren't able to completely roof a room because it was too large. Add supports.", failedRoofSq));
			}
			GenSound.PlaySoundAt(room.squaresList[0], RoofCompleteSound, 0.1f);
			completed = true;
		}

		private bool DoRoofEffectAlongSlice(int sliceZIntercept)
		{
			if (squaresToEffect.Count == 0)
			{
				Debug.LogWarning("Could not do roof effect: squaresToEffect.Count is zero");
				return false;
			}
			if (looper == null)
			{
				looper = new SoundLooperThing(squaresToEffect[0], RoofMakeLoopSound, 0.25f, SoundLooperMaintenanceType.PerTick);
				looper.looperSource.pitch -= -0.25f;
			}
			bool result = false;
			foreach (IntVec3 item in squaresToEffect)
			{
				if (item.x == item.z - sliceZIntercept)
				{
					result = true;
					MoteMaker.PlaceTempRoof(item);
				}
			}
			return result;
		}
	}

	private static List<RoofGenRequest> requestList = new List<RoofGenRequest>();

	public static void RoofGenerationRequest(Room room)
	{
		requestList.Add(new RoofGenRequest(room, Find.TickManager.tickCount));
	}

	public static void RoofMakerTick()
	{
		foreach (RoofGenRequest request in requestList)
		{
			request.RoofRequestTick();
		}
		requestList.RemoveAll((RoofGenRequest req) => req.ShouldRemove);
	}

	public static void InstantFinishAll()
	{
		foreach (RoofGenRequest request in requestList)
		{
			request.ForceInstantFinish();
		}
		requestList.Clear();
	}
}
