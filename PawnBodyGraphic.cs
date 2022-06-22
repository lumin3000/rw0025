using System.IO;
using UnityEngine;

public class PawnBodyGraphic
{
	public string bodyName;

	public ObjectGraphic body;

	public Vector3[] headOffsets = new Vector3[4];

	public Material MatIncap => body.MatAt(IntRot.south);

	public Material MatDead => body.MatAt(IntRot.south);

	public Material MatSleeping => body.MatAt(IntRot.south);

	public PawnBodyGraphic(string graphPath)
	{
		bodyName = Path.GetFileNameWithoutExtension(graphPath);
		body = new ObjectGraphic(graphPath);
		ref Vector3 reference = ref headOffsets[0];
		reference = new Vector3(0f, 0f, 0.25f);
		ref Vector3 reference2 = ref headOffsets[1];
		reference2 = new Vector3(0.15f, 0f, 0.24f);
		ref Vector3 reference3 = ref headOffsets[2];
		reference3 = new Vector3(0f, 0f, 0.27f);
		ref Vector3 reference4 = ref headOffsets[3];
		reference4 = new Vector3(-0.15f, 0f, 0.24f);
	}
}
