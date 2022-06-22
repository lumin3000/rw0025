using System;
using System.Collections.Generic;
using UnityEngine;

public class Verb
{
	public Thing owner;

	public Equipment equipment;

	protected VerbDefinition vDefNative;

	public VerbState state;

	protected TargetPack currentTarget;

	protected int burstShotsLeft;

	protected int ticksToNextBurstShot;

	public Action castCompleteCallback;

	public VerbDefinition VerbDef
	{
		get
		{
			if (vDefNative != null)
			{
				return vDefNative;
			}
			return equipment.def.verbDef;
		}
	}

	public Pawn OwnerPawn => owner as Pawn;

	public bool OwnerIsPawn => owner is Pawn;

	public bool MeleeRange => VerbDef.range < 1.1f;

	protected virtual int ShotsPerBurst => 1;

	public virtual string TooltipText => VerbDef.label + ": " + InfoTextShort;

	public virtual Texture2D UIIcon
	{
		get
		{
			if (equipment != null)
			{
				return equipment.def.uiIcon;
			}
			return GenUI.MissingContentTex;
		}
	}

	public virtual string InfoTextShort => VerbDef.description;

	public virtual string InfoTextFull => InfoTextShort;

	public override string ToString()
	{
		return VerbDef.label;
	}

	public virtual float HighlightFieldRadiusAroundTarget()
	{
		return 0f;
	}

	public bool TryStartCastOn(TargetPack castTarg)
	{
		if (owner == null)
		{
			Debug.LogError("Verb needs owner to work.");
			return false;
		}
		if (state == VerbState.Bursting || !CanHitTarget(castTarg))
		{
			return false;
		}
		currentTarget = castTarg;
		if (OwnerIsPawn && VerbDef.warmupTicks > 0)
		{
			ShootLine newShootLine = ShootLineFromTo(owner.Position, castTarg);
			OwnerPawn.drawer.Notify_WarmingCastAlongLine(newShootLine, owner.Position);
			OwnerPawn.stances.SetStance(new Stance_Warmup(VerbDef.warmupTicks, castTarg, InitBurst));
		}
		else
		{
			InitBurst();
		}
		if (castTarg.HasThing && (castTarg.thing.def.eType == EntityType.Pawn || castTarg.thing.def.eType == EntityType.Building_TurretGun) && castTarg.thing.Team == TeamType.Colonist && owner.Team.IsHostileToTeam(TeamType.Colonist))
		{
			Find.TickManager.slower.SignalForceNormalSpeed();
		}
		return true;
	}

	protected void InitBurst()
	{
		burstShotsLeft = ShotsPerBurst;
		state = VerbState.Bursting;
		TryFireNextBurstShot();
		if (OwnerIsPawn && OwnerPawn.skills != null)
		{
			float xp = 10f;
			if (currentTarget.thing != null && currentTarget.thing.def.eType == EntityType.Pawn)
			{
				xp = ((!currentTarget.thing.Team.IsHostileToTeam(owner.Team)) ? 50f : 200f);
			}
			OwnerPawn.skills.Learn(SkillType.Shooting, xp);
		}
	}

	public void VerbTick()
	{
		if (state == VerbState.Bursting)
		{
			ticksToNextBurstShot--;
			if (ticksToNextBurstShot <= 0)
			{
				TryFireNextBurstShot();
			}
		}
	}

	protected void TryFireNextBurstShot()
	{
		if (TryShotSpecialEffect())
		{
			DoShotCommonEffect();
			burstShotsLeft--;
		}
		else
		{
			burstShotsLeft = 0;
		}
		if (burstShotsLeft > 0)
		{
			ticksToNextBurstShot = VerbDef.ticksBetweenBurstShots;
			if (OwnerIsPawn)
			{
				OwnerPawn.stances.SetStance(new Stance_Cooldown(VerbDef.ticksBetweenBurstShots));
			}
			return;
		}
		state = VerbState.Idle;
		if (OwnerIsPawn)
		{
			int ticks = Mathf.RoundToInt((float)VerbDef.cooldownTicks * 1.35f);
			OwnerPawn.stances.SetStance(new Stance_Cooldown(ticks));
		}
		if (castCompleteCallback != null)
		{
			castCompleteCallback();
		}
	}

	protected virtual bool TryShotSpecialEffect()
	{
		return false;
	}

	protected void DoShotCommonEffect()
	{
		if (VerbDef.soundCast != null)
		{
			GenSound.PlaySoundAt(owner.Position, VerbDef.soundCast, 0.5f);
		}
	}

	public bool CanHitTarget(TargetPack Targ)
	{
		return CanHitTargetFrom(owner.Position, Targ);
	}

	public virtual bool CanHitTargetFrom(IntVec3 pos, TargetPack targ)
	{
		if (targ.thing != null && targ.thing == owner)
		{
			return VerbDef.targetParams.canTargetSelf;
		}
		return ShootLineFromTo(pos, targ).found;
	}

	public ShootLine ShootLineFromTo(IntVec3 Pos, TargetPack Targ)
	{
		IEnumerable<IntVec3> enumerable = ((!OwnerIsPawn) ? Gen.SquaresOccupiedBy(owner) : ShootLeanUtility.ShootingSourcesFromTo(Pos, Targ.Loc));
		foreach (IntVec3 item in enumerable)
		{
			if (Targ.thing != null)
			{
				foreach (IntVec3 item2 in ShootLeanUtility.ShootableSquaresOf(Targ.thing))
				{
					if (CanCastBetweenSquares(item, item2))
					{
						return new ShootLine(item, item2);
					}
				}
			}
			else if (CanCastBetweenSquares(item, Targ.Loc))
			{
				return new ShootLine(item, Targ.Loc);
			}
		}
		return ShootLine.NotFound;
	}

	protected virtual bool CanCastBetweenSquares(IntVec3 sourceRoot, IntVec3 targetLoc)
	{
		if (MeleeRange)
		{
			return sourceRoot.AdjacentTo8WayOrInside(targetLoc);
		}
		if (!sourceRoot.WithinHorizontalDistanceOf(targetLoc, VerbDef.range))
		{
			return false;
		}
		if (VerbDef.mustCastOnOpenGround && (!targetLoc.Standable() || Find.Grids.SquareContains(targetLoc, EntityCategory.Pawn)))
		{
			return false;
		}
		if (VerbDef.requireLineOfSight && !GenGrid.LineOfSight(sourceRoot, targetLoc))
		{
			return false;
		}
		return true;
	}
}
