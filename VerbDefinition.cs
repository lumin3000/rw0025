using System;
using UnityEngine;

public class VerbDefinition
{
	public VerbID id;

	public Type verbType;

	public string label = "Verb label";

	public string description = "Verb needs description.";

	public int warmupTicks;

	public int cooldownTicks;

	public float range = 1f;

	public int burstShotCount = 1;

	public int ticksBetweenBurstShots = 15;

	public AudioClip soundCast;

	public float noiseRadius;

	public bool hasStandardCommand;

	public bool targetable = true;

	public TargetingParameters targetParams = new TargetingParameters();

	public bool requireLineOfSight = true;

	public bool mustCastOnOpenGround;

	public bool isBuildingDestroyer;

	public bool isWeapon = true;

	public ThingDefinition projDef;

	public bool canMiss;

	public float hitMultiplierPerDist = 1f;

	public float accuracy = 5f;
}
