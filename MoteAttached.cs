using UnityEngine;

public class MoteAttached : Mote
{
	private class MoteAttachee
	{
		public Thing thing;

		public Vector3 drawPos;

		public bool isSet;

		public Thing Attachee
		{
			get
			{
				return thing;
			}
			set
			{
				thing = value;
				if (thing != null)
				{
					isSet = true;
					UpdateLastDrawPos();
				}
			}
		}

		public void UpdateLastDrawPos()
		{
			drawPos = thing.DrawPos;
		}
	}

	private MoteAttachee att1 = new MoteAttachee();

	private MoteAttachee att2 = new MoteAttachee();

	public void AttachTo(Thing a)
	{
		att1.Attachee = a;
	}

	public void AttachTo(Thing a, Thing b)
	{
		att1.Attachee = a;
		att2.Attachee = b;
	}

	public override void Draw()
	{
		if (att1.isSet && att2.isSet)
		{
			if (!att1.thing.destroyed)
			{
				att1.UpdateLastDrawPos();
			}
			if (!att2.thing.destroyed)
			{
				att2.UpdateLastDrawPos();
			}
			exactPosition = (att1.drawPos + att2.drawPos) * 0.5f;
		}
		else if (att1.isSet)
		{
			if (!att1.thing.destroyed)
			{
				att1.UpdateLastDrawPos();
			}
			exactPosition = att1.drawPos + def.mote.attachedDrawOffset;
		}
		exactPosition.y = Altitudes.AltitudeFor(AltitudeLayer.OverheadMote);
		base.Draw();
	}
}
