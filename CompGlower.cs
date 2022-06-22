using System;

public class CompGlower : ThingComp
{
	public bool canOverLight;

	public float glowRadius = 10f;

	public ColorInt glowColor = new ColorInt(255, 0, 0, 255);

	private bool glowOnInt;

	public bool GlowOn
	{
		get
		{
			return glowOnInt;
		}
		set
		{
			if (glowOnInt != value)
			{
				glowOnInt = value;
				if (!value)
				{
					Find.MapDrawer.MapChanged(parent.Position, MapChangeType.Things);
					Find.GlowGrid.DeRegisterGlower(this);
				}
				else
				{
					Find.MapDrawer.MapChanged(parent.Position, MapChangeType.Things);
					Find.GlowGrid.RegisterGlower(this);
				}
			}
		}
	}

	public int RadiusIntCeiling => (int)Math.Ceiling(glowRadius);

	public override void CompSpawnSetup()
	{
		if (GlowOn)
		{
			Find.GlowGrid.RegisterGlower(this);
		}
	}

	public override void CompExposeData()
	{
		Scribe.LookField(ref glowOnInt, "GlowOn");
	}

	public override void CompDestroy()
	{
		base.CompDestroy();
		GlowOn = false;
	}
}
