using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesignationManager : Saveable
{
	public List<Designation> designationList = new List<Designation>();

	public IEnumerable<Thing> DesignatedThingsToHaul => from d in designationList
		where d is Designation_Haul && d.MayBeAccessible && d.target.thing.carrier == null
		select d.target.thing;

	public IEnumerable<Thing> ShouldMineDesignatedThings
	{
		get
		{
			foreach (Designation des in designationList)
			{
				if (des is Designation_Mine && des.MayBeAccessible)
				{
					Thing i = MineUtility.MineableInSquare(des.target.Loc);
					if (i != null)
					{
						yield return i;
					}
				}
			}
		}
	}

	public void ExposeData()
	{
		Scribe.LookList(ref designationList, "DesignationList");
	}

	public void DrawDesignations()
	{
		foreach (Designation designation in designationList)
		{
			designation.DesignationDraw();
		}
	}

	public void AddDesignation(Designation newDes)
	{
		if (DesignationAt(newDes.target.Loc, newDes.dType) != null)
		{
			Debug.Log("Tried to double-add designation on " + newDes.target);
		}
		else
		{
			designationList.Add(newDes);
		}
	}

	public Designation DesignationOn(Thing t, DesignationType desType)
	{
		if (desType == DesignationType.Mine)
		{
			Debug.LogWarning("Mining designations are indexed by location only.");
		}
		foreach (Designation designation in designationList)
		{
			if (designation.target.thing == t && designation.dType == desType)
			{
				return designation;
			}
		}
		return null;
	}

	public Designation DesignationAt(IntVec3 sq, DesignationType desType)
	{
		foreach (Designation designation in designationList)
		{
			if (designation.target.Loc == sq && designation.dType == desType)
			{
				return designation;
			}
		}
		return null;
	}

	public IEnumerable<Designation> AllDesignationsOn(Thing t)
	{
		return designationList.Where((Designation d) => d.target.thing == t);
	}

	public IEnumerable<Designation> AllDesignationsAt(IntVec3 sq)
	{
		return designationList.Where((Designation d) => d.target.Loc == sq);
	}

	public void RemoveDesignation(Designation des)
	{
		designationList.Remove(des);
	}

	public void RemoveAllDesignationsOn(Thing t)
	{
		designationList.RemoveAll((Designation d) => d.target.thing == t);
	}

	public void RemoveAllDesignationsAt(IntVec3 loc)
	{
		designationList.RemoveAll((Designation d) => d.target.Loc == loc);
	}
}
