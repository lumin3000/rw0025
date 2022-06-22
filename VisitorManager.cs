using System.Collections.Generic;

public class VisitorManager : Saveable
{
	public List<Visitor> VisitorList = new List<Visitor>();

	public Trader ActiveTrader;

	public void ExposeData()
	{
		Scribe.LookList(ref VisitorList, "VisitorList");
	}

	public void AddVisitor(Visitor Vis)
	{
		VisitorList.Add(Vis);
	}

	public void VisitorDeparts(Visitor Vis)
	{
		VisitorList.Remove(Vis);
	}

	public void VisitorManagerTick()
	{
		foreach (Visitor item in VisitorList.ListFullCopy())
		{
			item.VisitorTick();
		}
	}
}
