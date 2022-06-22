using UnityEngine;

public class Designation : Saveable
{
	public const float ClaimedDesignationDrawAltitude = 15f;

	public DesignationType dType;

	protected Material iconMat;

	public TargetPack target;

	public float DesignationDrawAltitude => Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);

	public bool MayBeAccessible
	{
		get
		{
			foreach (IntVec3 item in target.Loc.AdjacentSquaresCardinal())
			{
				if (item.Standable())
				{
					return true;
				}
			}
			return false;
		}
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref target, "Target");
	}

	public void Delete()
	{
		Find.DesignationManager.RemoveDesignation(this);
	}

	public virtual void DesignationDraw()
	{
		Graphics.DrawMesh(MeshPool.plane10, target.Loc.ToVector3ShiftedWithAltitude(DesignationDrawAltitude), Quaternion.identity, iconMat, 0);
	}

	public override string ToString()
	{
		return string.Format(string.Concat("Designation:(dType=", dType, ", Target=", target, ")"));
	}
}
