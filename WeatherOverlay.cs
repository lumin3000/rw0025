using UnityEngine;

public abstract class WeatherOverlay
{
	public Material worldOverlayMat;

	public Material screenOverlayMat;

	protected float worldOverlayPanSpeed1;

	protected float worldOverlayPanSpeed2;

	protected Vector2 worldPanDir1;

	protected Vector2 worldPanDir2;

	public Color OverlayColor
	{
		set
		{
			if (worldOverlayMat != null)
			{
				worldOverlayMat.color = value;
			}
			if (screenOverlayMat != null)
			{
				screenOverlayMat.color = value;
			}
		}
	}

	public virtual void WeatherOverlayTick()
	{
		if (worldOverlayMat != null)
		{
			worldOverlayMat.SetTextureOffset("_MainTex", Find.TickManager.tickCount * worldPanDir1 * -1f * worldOverlayPanSpeed1 * worldOverlayMat.GetTextureScale("_MainTex").x);
			if (worldOverlayMat.HasProperty("_MainTex2"))
			{
				worldOverlayMat.SetTextureOffset("_MainTex2", Find.TickManager.tickCount * worldPanDir2 * -1f * worldOverlayPanSpeed2 * worldOverlayMat.GetTextureScale("_MainTex2").x);
			}
		}
	}

	public void DrawWeatherOverlay()
	{
		if (worldOverlayMat != null)
		{
			Vector3 position = Find.Map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
			Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, worldOverlayMat, 0);
		}
		if (screenOverlayMat != null)
		{
			float num = Find.CameraMap.camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * Find.CameraMap.camera.aspect, 1f, num);
			Vector3 position2 = Find.CameraMap.transform.position;
			position2.y = Altitudes.AltitudeFor(AltitudeLayer.Weather);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(position2, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, screenOverlayMat, 0);
		}
	}

	public override string ToString()
	{
		if (worldOverlayMat != null)
		{
			return worldOverlayMat.name;
		}
		if (screenOverlayMat != null)
		{
			return screenOverlayMat.name;
		}
		return "NoOverlayOverlay";
	}
}
