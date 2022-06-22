using System.Collections.Generic;
using UnityEngine;

public class Blueprint : Building, Interactive
{
	protected readonly Color DrawColor = new Color(0.5f, 0.8f, 1f, 0.35f);

	public Thing SmallThingBlockingPlacement
	{
		get
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
			{
				foreach (Thing item2 in item.ThingsInSquare())
				{
					if (BlockedBy(item2))
					{
						return item2;
					}
				}
			}
			return null;
		}
	}

	public bool BlockedBy(Thing t)
	{
		if (def.entityDefToBuild is TerrainDefinition)
		{
			return false;
		}
		return t.def.BlockBuilding;
	}

	public JobCondition InteractedWith(ReservationType WType, Pawn pawn)
	{
		if (SmallThingBlockingPlacement != null)
		{
			return JobCondition.Incompletable;
		}
		Find.ReservationManager.UnReserve(this, ReservationType.Construction);
		Destroy();
		BuildingFrame buildingFrame = ConstructionUtility.PlaceBuildingFrameOf(def.entityDefToBuild, base.Position, rotation);
		pawn.ThinkNodeRoot.GetThinkNode<JobGiver_Orders>().QueueJob(new Job(JobType.Construct, buildingFrame));
		return JobCondition.Succeeded;
	}

	public override void Tick()
	{
		base.Tick();
		if (!ConstructionUtility.BuildingCanGoOnTerrain(def.entityDefToBuild, base.Position, rotation))
		{
			CancelBlueprint();
		}
	}

	public void CancelBlueprint()
	{
		GenMap.ReclaimResourcesFor(this);
		Destroy();
	}

	public override IEnumerable<Command> GetCommandOptions()
	{
		yield return new Command_Action
		{
			icon = Res.LoadTexture("UI/DesignationIcons/Cancel"),
			tipDef = new TooltipDef("[C]ancel this construction."),
			hotKey = KeyCode.C,
			action = CancelBlueprint
		};
	}
}
