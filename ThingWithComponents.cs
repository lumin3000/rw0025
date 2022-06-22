using System.Collections.Generic;
using System.Text;

public class ThingWithComponents : Thing
{
	private List<ThingComp> compList = new List<ThingComp>();

	public void AddComp(ThingComp newComp)
	{
		compList.Add(newComp);
		newComp.parent = this;
	}

	public T GetComp<T>() where T : ThingComp
	{
		foreach (ThingComp comp in compList)
		{
			T val = comp as T;
			if (val != null)
			{
				return val;
			}
		}
		return (T)null;
	}

	public ThingComp GetCompBySetup(CompSetup setup)
	{
		foreach (ThingComp comp in compList)
		{
			if (comp.setup == setup)
			{
				return comp;
			}
		}
		return null;
	}

	public void SetupComponents()
	{
		foreach (CompSetup compSetup in def.compSetupList)
		{
			AddComp(ThingCompMaker.MakeThingComp(compSetup));
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			SetupComponents();
		}
		foreach (ThingComp comp in compList)
		{
			comp.CompExposeData();
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		foreach (ThingComp comp in compList)
		{
			comp.CompSpawnSetup();
		}
	}

	public override void DeSpawn()
	{
		base.DeSpawn();
		foreach (ThingComp comp in compList)
		{
			comp.CompDeSpawn();
		}
	}

	public override void Killed(DamageInfo dam)
	{
		base.Killed(dam);
		foreach (ThingComp comp in compList)
		{
			comp.CompKilled(dam);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		foreach (ThingComp comp in compList)
		{
			comp.CompDestroy();
		}
	}

	public override void DestroyFinalize()
	{
		base.DestroyFinalize();
		foreach (ThingComp comp in compList)
		{
			comp.CompDestroyFinalize();
		}
	}

	public override void Tick()
	{
		foreach (ThingComp comp in compList)
		{
			comp.CompTick();
		}
	}

	protected override void ApplyDamage(DamageInfo dinfo)
	{
		base.ApplyDamage(dinfo);
		foreach (ThingComp comp in compList)
		{
			comp.CompApplyDamage(dinfo);
		}
	}

	public override void Draw()
	{
		base.Draw();
		Comps_Draw();
	}

	protected void Comps_Draw()
	{
		foreach (ThingComp comp in compList)
		{
			comp.CompDraw();
		}
	}

	public override IEnumerable<MapMeshPiece> EmitMapMeshPieces()
	{
		foreach (ThingComp c in compList)
		{
			foreach (MapMeshPiece item in c.CompEmitMapMeshPieces())
			{
				yield return item;
			}
		}
		foreach (MapMeshPiece item2 in base.EmitMapMeshPieces())
		{
			yield return item2;
		}
	}

	public override IEnumerable<Command> GetCommandOptions()
	{
		foreach (ThingComp c in compList)
		{
			foreach (Command item in c.CompCommands())
			{
				yield return item;
			}
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		foreach (ThingComp comp in compList)
		{
			string text = comp.CompInspectString();
			if (text != string.Empty)
			{
				stringBuilder.AppendLine(text);
			}
		}
		return stringBuilder.ToString();
	}
}
