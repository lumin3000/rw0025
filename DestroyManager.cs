using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyManager
{
	private List<Entity> destroyList = new List<Entity>();

	public void AddToDestroyList(Thing t)
	{
		t.DestroyFinalize();
	}

	public void DestroyTick()
	{
		Entity entity = null;
		try
		{
			foreach (Entity destroy in destroyList)
			{
				entity = destroy;
				destroy.DestroyFinalize();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Debug.Log("   Finalizing thing was " + entity);
			Debug.Log("   DestroyList was:");
			foreach (Thing destroy2 in destroyList)
			{
				Debug.Log("        " + destroy2);
			}
		}
		destroyList.Clear();
	}

	public void ForceFinalizations()
	{
		DestroyTick();
	}
}
