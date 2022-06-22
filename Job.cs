using System;
using UnityEngine;

public class Job : Saveable
{
	public JobType jType;

	public TargetPack targetA;

	public TargetPack targetB;

	public int expiryTime = int.MaxValue;

	public int maxNumMeleeAttacks = int.MaxValue;

	public bool exitMapOnArrival;

	public bool killIncappedTarget;

	public MoveSpeed moveSpeed = MoveSpeed.Jog;

	public ThingDefinition plantDefToSow;

	public Verb verbToUse;

	public JobDefinition Def => JobDefs.DefinitionOf(jType);

	public int TimeLimit
	{
		set
		{
			expiryTime = value + Find.TickManager.tickCount;
		}
	}

	public Job()
	{
	}

	public Job(JobType jType)
		: this(jType, null)
	{
	}

	public Job(JobType jType, TargetPack target)
		: this(jType, target, null)
	{
	}

	public Job(JobType jType, TargetPack target, TargetPack targetB)
	{
		if (jType == JobType.Undefined)
		{
			Debug.LogError("Cannot create undefined job.");
		}
		this.jType = jType;
		targetA = target;
		this.targetB = targetB;
	}

	public Job(JobType jType, int timeLimit)
	{
		if (jType == JobType.Undefined)
		{
			Debug.LogError("Cannot create undefined job.");
		}
		this.jType = jType;
		TimeLimit = timeLimit;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref jType, "JType", forceSave: true);
		Scribe.LookSaveable(ref targetA, "TargetA");
		Scribe.LookSaveable(ref targetB, "TargetB");
		Scribe.LookField(ref expiryTime, "ExpiryTime", int.MaxValue);
		Scribe.LookField(ref maxNumMeleeAttacks, "MaxNumMeleeAttacks", int.MaxValue);
		Scribe.LookField(ref exitMapOnArrival, "ExitMapOnArrival", forceSave: false);
		Scribe.LookField(ref killIncappedTarget, "KillIncappedTarget", forceSave: false);
		Scribe.LookDefinition(ref plantDefToSow, "PlantDefToSow");
	}

	public JobDriver GetDriver(Pawn DriverPawn)
	{
		Type driverClass = JobDefs.DefinitionOf(jType).driverClass;
		if (driverClass == null)
		{
			Debug.LogError("Missing driver class for job type " + jType);
		}
		return (JobDriver)Activator.CreateInstance(driverClass, DriverPawn);
	}

	public virtual bool MightBeReachable()
	{
		return true;
	}

	public bool JobIsSameAs(Job other)
	{
		if (other == null)
		{
			return false;
		}
		if (jType != other.jType)
		{
			return false;
		}
		if (targetA == null && other.targetA != null)
		{
			return false;
		}
		if (other.targetA != null && targetA == null)
		{
			return false;
		}
		if (targetA != null && other.targetA != null && !targetA.SameAs(other.targetA))
		{
			return false;
		}
		if (verbToUse != other.verbToUse)
		{
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		string text = ((targetA == null) ? "null" : targetA.ToString());
		string text2 = ((targetB == null) ? "null" : targetB.ToString());
		return string.Concat("Job(jType=", jType, ", targetA=", text, ", targetB=", text2, ")");
	}
}
