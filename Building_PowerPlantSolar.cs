using UnityEngine;

public class Building_PowerPlantSolar : Building_PowerPlant
{
	private const float FullSunPower = 1700f;

	private const float NightPower = 0f;

	private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

	private static readonly Material BarFilledMat = GenRender.SolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));

	private static readonly Material BarUnfilledMat = GenRender.SolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

	public override void Tick()
	{
		base.Tick();
		if (Find.RoofGrid.SquareIsRoofed(base.Position))
		{
			powerComp.powerOutput = 0f;
		}
		else
		{
			powerComp.powerOutput = Mathf.Lerp(0f, 1700f, SkyManager.curSkyGlowPercent);
		}
	}

	public override void Draw()
	{
		base.Draw();
		GenRender.FillableBarRequest fillableBarRequest = new GenRender.FillableBarRequest();
		fillableBarRequest.Center = DrawPos + Vector3.up * 0.1f;
		fillableBarRequest.BarSize = BarSize;
		fillableBarRequest.FillPercent = powerComp.powerOutput / 1700f;
		fillableBarRequest.FilledMat = BarFilledMat;
		fillableBarRequest.UnfilledMat = BarUnfilledMat;
		fillableBarRequest.Margin = 0.15f;
		IntRot intRot = rotation;
		intRot.Rotate(RotationDirection.Clockwise);
		fillableBarRequest.Rotation = intRot;
		GenRender.RenderFillableBar(fillableBarRequest);
	}
}
