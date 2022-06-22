using System.Text;
using UnityEngine;

public class Corpse : ThingWithComponents, ThoughtGiver
{
	public Pawn sourcePawn;

	private int timeOfDeath;

	public int Age => Find.TickManager.tickCount - timeOfDeath;

	public override string Label => sourcePawn.Label + " (dead)";

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		timeOfDeath = Find.TickManager.tickCount;
		sourcePawn.rotation = IntRot.south;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref timeOfDeath, "TimeOfDeath");
		Scribe.LookSaveable(ref sourcePawn, "SourcePawn");
	}

	public override void DrawAt(Vector3 drawLoc)
	{
		Building building = this.StoringBuilding();
		if (building == null || building.TType != EntityType.Building_Grave)
		{
			sourcePawn.drawer.renderer.RenderPawnAt(drawLoc);
		}
	}

	public Thought GiveObservedThought()
	{
		if (!sourcePawn.raceDef.humanoid)
		{
			return null;
		}
		Thing thing = this.StoringBuilding();
		if (thing == null)
		{
			return new Thought_Observation(ThoughtType.ObservedLayingCorpse, this);
		}
		if (thing.TType == EntityType.Area_Dump)
		{
			if (sourcePawn.Team == TeamType.Colonist)
			{
				return new Thought_Observation(ThoughtType.ObservedDeadColonistInDump, this);
			}
		}
		else if (thing.TType == EntityType.Building_GibbetCage)
		{
			if (sourcePawn.Team == TeamType.Colonist)
			{
				return new Thought_Observation(ThoughtType.ObservedGibbetCageFullColonist, this);
			}
			return new Thought_Observation(ThoughtType.ObservedGibbetCageFullStranger, this);
		}
		return null;
	}

	public override void TickRare()
	{
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Team: " + sourcePawn.Team);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Dead since " + timeOfDeath.AsDate());
		stringBuilder.AppendLine("Dead for " + Age.TicksInDaysString());
		return stringBuilder.ToString();
	}
}
