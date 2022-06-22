using UnityEngine;

public class CameraMap : MonoBehaviour
{
	private const float MaxDeltaTime = 0.025f;

	private const float ScreenEdgeWidth = 20f;

	private const float MapEdgeMargin = -2f;

	private const float StartingSize = 24f;

	private const float MinSize = 12f;

	private const float MaxSize = 60f;

	private const float ZoomSpeed = 2.6f;

	private const float ZoomTightness = 0.4f;

	private const float ZoomScaleFromAltDenominator = 35f;

	private const float PageKeyZoomRate = 4f;

	private const float MinAltitude = 15f;

	private const float MaxAltitude = 65f;

	public CameraShaker shaker;

	public CameraMapConfig config = new CameraMapConfig_Normal();

	protected Vector3 camVelocity;

	protected Vector3 camRootPos;

	protected float camRootSize;

	protected float desiredSize;

	protected Vector2 mouseDragVect = Vector2.zero;

	public CameraZoomRange CurrentZoom
	{
		get
		{
			if (camRootSize < 13f)
			{
				return CameraZoomRange.Closest;
			}
			if (camRootSize < 13.8f)
			{
				return CameraZoomRange.Close;
			}
			if (camRootSize < 42f)
			{
				return CameraZoomRange.Middle;
			}
			if (camRootSize < 57f)
			{
				return CameraZoomRange.Far;
			}
			return CameraZoomRange.Furthest;
		}
	}

	public IntRect CurrentViewRect
	{
		get
		{
			IntRect result = default(IntRect);
			result.minX = Mathf.FloorToInt(base.transform.position.x - base.camera.orthographicSize * base.camera.aspect - 1f);
			result.maxX = Mathf.CeilToInt(base.transform.position.x + base.camera.orthographicSize * base.camera.aspect);
			result.minZ = Mathf.FloorToInt(base.transform.position.z - base.camera.orthographicSize - 1f);
			result.maxZ = Mathf.CeilToInt(base.transform.position.z + base.camera.orthographicSize);
			return result;
		}
	}

	private float HitchReduceFactor
	{
		get
		{
			float result = 1f;
			if (Time.deltaTime > 0.025f)
			{
				result = 0.025f / Time.deltaTime;
			}
			return result;
		}
	}

	private Vector2 CurInputDollyVect
	{
		get
		{
			Vector2 result = Vector3.zero;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				result.x = 0f - config.scrollRateKeys;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				result.x = config.scrollRateKeys;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				result.y = config.scrollRateKeys;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				result.y = 0f - config.scrollRateKeys;
			}
			if (mouseDragVect != Vector2.zero)
			{
				mouseDragVect *= HitchReduceFactor;
				mouseDragVect.x *= -1f;
				result += mouseDragVect * config.scrollRateMouseDrag;
				mouseDragVect = Vector2.zero;
			}
			Vector2 vector = Input.mousePosition;
			Vector2 point = vector;
			point.y = (float)Screen.height - point.y;
			if (Screen.fullScreen)
			{
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect(Screen.width - 250, 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, Screen.height - 250, 205f, 255f);
				Rect rect4 = new Rect(Screen.width - 250, Screen.height - 250, 255f, 255f);
				if (!rect.Contains(point) && !rect3.Contains(point) && !rect2.Contains(point) && !rect4.Contains(point))
				{
					Vector2 vector2 = new Vector2(0f, 0f);
					if (vector.x >= 0f && vector.x < 20f)
					{
						vector2.x -= config.scrollRateScreenEdge;
					}
					if (vector.x <= (float)Screen.width && vector.x > (float)Screen.width - 20f && vector.y < (float)(Screen.height - 200))
					{
						vector2.x += config.scrollRateScreenEdge;
					}
					if (vector.y <= (float)Screen.height && vector.y > (float)Screen.height - 20f)
					{
						vector2.y += config.scrollRateScreenEdge;
					}
					if (vector.y >= 0f && vector.y < 20f && vector.x > 300f)
					{
						vector2.y -= config.scrollRateScreenEdge;
					}
					result += vector2;
				}
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				result *= 2.4f;
			}
			return result;
		}
	}

	private void Awake()
	{
		shaker = new CameraShaker();
		ResetCamera();
	}

	private void OnPreCull()
	{
		Find.WeatherManager.DrawAllWeather();
	}

	public void ResetCamera()
	{
		desiredSize = 24f;
		camRootSize = desiredSize;
		base.camera.orthographicSize = desiredSize;
	}

	public void Expose()
	{
		Scribe.EnterNode("CameraMap");
		Scribe.LookField(ref camRootPos, "CamRootPos");
		Scribe.LookField(ref desiredSize, "DesiredSize");
		camRootSize = desiredSize;
		base.camera.orthographicSize = desiredSize;
		Scribe.ExitNode();
	}

	private void OnGUI()
	{
		if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
		{
			mouseDragVect = Event.current.delta;
			Event.current.Use();
		}
	}

	private void Update()
	{
		Vector2 curInputDollyVect = CurInputDollyVect;
		if (curInputDollyVect != Vector2.zero)
		{
			float num = (camRootSize - 12f) / 48f * 0.7f + 0.3f;
			camVelocity = new Vector3(curInputDollyVect.x, 0f, curInputDollyVect.y) * num;
		}
		if (Find.UIRoot.dialogs.TopDialog == null)
		{
			float num2 = Time.deltaTime * HitchReduceFactor;
			camRootPos += camVelocity * num2 * config.moveSpeedScale;
			if (camRootPos.x > (float)Find.Map.Size.x + -2f)
			{
				camRootPos.x = (float)Find.Map.Size.x + -2f;
			}
			if (camRootPos.z > (float)Find.Map.Size.z + -2f)
			{
				camRootPos.z = (float)Find.Map.Size.z + -2f;
			}
			if (camRootPos.x < 2f)
			{
				camRootPos.x = 2f;
			}
			if (camRootPos.z < 2f)
			{
				camRootPos.z = 2f;
			}
		}
		if (camVelocity != Vector3.zero)
		{
			camVelocity *= config.camSpeedDecayFactor;
			if (camVelocity.magnitude < 0.1f)
			{
				camVelocity = Vector3.zero;
			}
		}
		float num3 = 0f;
		if (Find.UIRoot.dialogs.TopDialog == null)
		{
			num3 = Input.GetAxis("Mouse ScrollWheel");
			if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.T))
			{
				num3 -= 4f;
			}
			if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.G))
			{
				num3 += 4f;
			}
		}
		desiredSize -= num3 * 2.6f * camRootSize / 35f;
		if (desiredSize < 12f)
		{
			desiredSize = 12f;
		}
		if (desiredSize > 60f)
		{
			desiredSize = 60f;
		}
		float num4 = desiredSize - camRootSize;
		camRootSize += num4 * 0.4f;
		base.camera.orthographicSize = camRootSize;
		camRootPos.y = 15f + (camRootSize - 12f) / 48f * 50f;
		shaker.Update();
		base.transform.position = camRootPos + shaker.ShakeOffset;
	}

	public void JumpTo(Vector3 newLookAt)
	{
		camRootPos = new Vector3(newLookAt.x, camRootPos.y, newLookAt.z);
	}

	public void JumpTo(IntVec3 IntLoc)
	{
		JumpTo(IntLoc.ToVector3Shifted());
	}

	public float SquareSize()
	{
		return (float)Screen.height / (base.camera.orthographicSize * 2f);
	}

	public Vector2 InvertedWorldToScreenPoint(Vector3 worldLoc)
	{
		Vector3 vector = base.camera.WorldToScreenPoint(worldLoc);
		vector.y = (float)Screen.height - vector.y;
		return new Vector2(vector.x, vector.y);
	}
}
