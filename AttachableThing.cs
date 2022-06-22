using UnityEngine;

public abstract class AttachableThing : Thing
{
	public Thing parent;

	public override Vector3 DrawPos
	{
		get
		{
			if (parent != null)
			{
				return parent.DrawPos + Vector3.up * 0.1f;
			}
			return base.DrawPos;
		}
	}

	public abstract string InfoStringAddon { get; }

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookThingRef(ref parent, "Parent", this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && parent != null)
		{
			parent.attachList.Add(this);
		}
	}

	public void AttachTo(Thing newBase)
	{
		parent = newBase;
		newBase.attachList.Add(this);
	}

	public override void Destroy()
	{
		base.Destroy();
		if (parent != null)
		{
			parent.attachList.Remove(this);
		}
	}
}
