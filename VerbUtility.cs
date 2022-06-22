using System.Collections.Generic;
using UnityEngine;

public static class VerbUtility
{
	public static void DrawTargetingGUI_Update(this Verb verb)
	{
		verb.DrawHittableSquares();
		TargetPack targetPack = GenUI.ClickTargetUnderMouse(verb.VerbDef.targetParams);
		if (targetPack == null || !verb.CanHitTarget(targetPack))
		{
			return;
		}
		GenRender.RenderTargetHighlight(targetPack);
		if (verb.HighlightFieldRadiusAroundTarget() > 0.2f)
		{
			ShootLine shootLine = verb.ShootLineFromTo(verb.owner.Position, targetPack);
			if (shootLine.found)
			{
				Explosion.DisplayPredictedExplosiveRadius(shootLine.dest, verb.HighlightFieldRadiusAroundTarget());
			}
		}
	}

	public static void DrawTargetingGUI_OnGUI(this Verb verb)
	{
		Vector3 vector = Event.current.mousePosition;
		GUI.DrawTexture(new Rect(vector.x + 8f, vector.y + 8f, 32f, 32f), verb.UIIcon);
	}

	public static void DrawHittableSquares(this Verb verb)
	{
		verb.DrawHittableSquares(verb.owner.Position);
	}

	public static void DrawHittableSquares(this Verb verb, IntVec3 shotSource)
	{
		List<IntVec3> list = new List<IntVec3>();
		list.Add(shotSource);
		int num = Gen.NumSquaresInRadius(verb.VerbDef.range);
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = shotSource + Gen.RadialPattern[i];
			if (intVec.InBounds() && verb.CanHitTargetFrom(shotSource, new TargetPack(intVec)))
			{
				Thing thing = Find.Grids.BlockerAt(intVec);
				if (thing == null || thing.def.category != EntityCategory.Building || thing.def.canBeSeenOver)
				{
					list.Add(intVec);
				}
			}
		}
		foreach (Pawn allPawn in Find.PawnManager.AllPawns)
		{
			if (verb.CanHitTargetFrom(shotSource, new TargetPack(allPawn)))
			{
				list.Add(allPawn.Position);
			}
		}
		GenRender.RenderFieldEdges(list);
	}
}
