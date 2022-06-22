using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ThingWithComponents
{
	public Verb verb;

	public IEnumerable<Verb> AllVerbs
	{
		get
		{
			yield return verb;
		}
	}

	public void InitVerb()
	{
		if (verb == null)
		{
			verb = (Verb)Activator.CreateInstance(def.verbDef.verbType);
			verb.equipment = this;
		}
	}

	public void TakenAndEquippedBy(Pawn p)
	{
		p.equipment.MakeRoomFor(this);
		p.equipment.AddEquipment(this);
		GenSound.PlaySoundAt(p.Position, def.interactSound, 0.2f);
		DeSpawn();
	}

	public override void DrawAt(Vector3 drawLoc)
	{
		float angle = (float)((thingIDNumber * thingIDNumber) ^ 0x18D) % 80f - 40f;
		Graphics.DrawMesh(DrawMesh, drawLoc, Quaternion.AngleAxis(angle, Vector3.up), DrawMat, 0);
	}
}
