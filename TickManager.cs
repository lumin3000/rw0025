using UnityEngine;

public class TickManager
{
	public const int TickRareInterval = 250;

	public const float BaseTicksPerSecond = 60f;

	public int tickCount;

	private float realTimeToTickThrough;

	public TimeSpeed curTimeSpeed = TimeSpeed.Normal;

	public TimeSpeed prePauseTimeSpeed;

	private TickList tickListNormal = new TickList(TickerType.Normal);

	private TickList tickListRare = new TickList(TickerType.Rare);

	public TimeSlower slower = new TimeSlower();

	public float TickRateMultiplier
	{
		get
		{
			if (slower.ForcedNormalSpeed)
			{
				if (curTimeSpeed == TimeSpeed.Paused)
				{
					return 0f;
				}
				return 1f;
			}
			return curTimeSpeed switch
			{
				TimeSpeed.Paused => 0f, 
				TimeSpeed.Normal => 1f, 
				TimeSpeed.Fast => 3f, 
				TimeSpeed.Superfast => 5f, 
				TimeSpeed.DebugUltrafast => 20f, 
				_ => -1f, 
			};
		}
	}

	private float CurTimePerTick
	{
		get
		{
			if (TickRateMultiplier == 0f)
			{
				return 0f;
			}
			return 1f / (60f * TickRateMultiplier);
		}
	}

	public bool Paused
	{
		get
		{
			if (curTimeSpeed == TimeSpeed.Paused || Find.UIMapRoot.dialogs.TopDialog != null || Find.UIMapRoot.modeControls.PausedTab())
			{
				return true;
			}
			return false;
		}
	}

	public void TogglePaused()
	{
		if (curTimeSpeed != 0)
		{
			prePauseTimeSpeed = curTimeSpeed;
			curTimeSpeed = TimeSpeed.Paused;
		}
		else
		{
			curTimeSpeed = prePauseTimeSpeed;
		}
	}

	public void Expose()
	{
		Scribe.EnterNode("TickManager");
		int value = tickCount;
		Scribe.LookField(ref value, "TickCount");
		tickCount = value;
		Scribe.ExitNode();
	}

	public void RegisterAllTickabilityFor(Thing t)
	{
		TickListFor(t)?.RegisterThing(t);
	}

	public void DeRegisterAllTickabilityFor(Thing t)
	{
		TickListFor(t)?.DeregisterThing(t);
	}

	private TickList TickListFor(Thing t)
	{
		return t.def.tickerType switch
		{
			TickerType.Never => null, 
			TickerType.Normal => tickListNormal, 
			TickerType.Rare => tickListRare, 
			_ => null, 
		};
	}

	public void TickManagerUpdate()
	{
		if (!Paused)
		{
			realTimeToTickThrough += Time.deltaTime;
			int num = 0;
			while (realTimeToTickThrough > 0f && (float)num < TickRateMultiplier * 2f)
			{
				DoSingleTick();
				realTimeToTickThrough -= CurTimePerTick;
				num++;
			}
			if (realTimeToTickThrough > 0f)
			{
				realTimeToTickThrough = 0f;
			}
		}
	}

	public void DoSingleTick()
	{
		if (!DebugSettings.fastEcology)
		{
			tickCount++;
		}
		else
		{
			tickCount += 250;
		}
		tickListNormal.SingleTick();
		tickListRare.SingleTick();
		Find.DestroyManager.DestroyTick();
		Find.GameEnder.GameEndTick();
		DateHandler.DateHandlerTick();
		Find.Storyteller.StorytellerTick();
		AirNets.onlyNet.AirNetTick();
		PowerNetManager.PowerNetsTick();
		if (Find.Map.breakdownManager != null)
		{
			Find.Map.breakdownManager.BreakdownManagerTick();
		}
		Find.AIKingManager.AIKingManagerTick();
		Find.VisitorManager.VisitorManagerTick();
		Find.DebugDrawer.DebugDrawerTick();
		Find.Map.autosaver.AutosaverTick();
		Find.ReachabilityRegions.ReachabilityRegionsTick();
		Find.Map.colonyInfo.ColonyInfoTick();
		RoofMaker.RoofMakerTick();
		Find.MapConditionManager.MapConditionManagerTick();
		Find.WeatherManager.WeatherManagerTick();
	}

	public void DebugSetTickCount(int newTickCount)
	{
		tickCount = newTickCount;
	}
}
