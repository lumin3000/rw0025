using UnityEngine;

public class Building_AirNetworkable : Building
{
	protected AirNetwork airNet;

	public float StoredAir;

	public float StoredAirMax;

	private static readonly Material AirBarFilledMat = GenRender.SolidColorMaterial(new Color(0.85f, 0.85f, 0.85f));

	private static readonly Material AirBarUnfilledMat = GenRender.SolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

	public void SetAirNet(AirNetwork NewNet)
	{
		airNet = NewNet;
		airNet.airBuildings.Add(this);
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (StoredAirMax > 0f)
		{
			text += "\n\n";
			if (airNet != null)
			{
				string text2 = text;
				text = text2 + "Stored air: " + StoredAir.ToString("###0.00") + " / " + StoredAirMax.ToString("###0.00");
			}
			else
			{
				text += "Not connected to air network.";
			}
		}
		return text;
	}

	public virtual void Tick_ConsumeAir()
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref StoredAir, "StoredAir");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		SetAirNet(AirNets.onlyNet);
	}

	public override void Destroy()
	{
		base.Destroy();
		if (airNet != null)
		{
			airNet.airBuildings.Remove(this);
		}
	}

	protected void DrawAirBar(Vector2 Offset, float BarLength)
	{
		GenRender.FillableBarRequest fillableBarRequest = new GenRender.FillableBarRequest();
		fillableBarRequest.Center = DrawPos + Vector3.up * 0.1f;
		fillableBarRequest.BarSize = new Vector2(BarLength, 0.8f);
		fillableBarRequest.FillPercent = StoredAir / StoredAirMax;
		fillableBarRequest.FilledMat = AirBarFilledMat;
		fillableBarRequest.UnfilledMat = AirBarUnfilledMat;
		fillableBarRequest.Margin = 0.15f;
		fillableBarRequest.Rotation = rotation;
		fillableBarRequest.PreRotationOffset = Offset;
		GenRender.RenderFillableBar(fillableBarRequest);
	}
}
