using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	private void Update()
	{
		base.transform.position = Find.CameraMap.transform.position;
	}
}
