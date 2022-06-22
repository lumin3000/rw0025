using System;

public class JobDefinition
{
	public JobType jType;

	public Type driverClass;

	public ReservationType reservationType;

	public string interactMoteName = string.Empty;

	public bool interactTakeBreaks = true;

	public InteractionLocationType interactLocation = InteractionLocationType.AnyAdjacent;

	public Func<Thing, bool> interactFailCondition;

	public Type interactEffectMakerType;

	public bool interruptOnHarmfulDamage;

	public SpeechConfig speechToGive;

	public MeleeAttackMode meleeMode;

	public JobReport jobReportSpecial;

	public bool easyInterrupt = true;
}
