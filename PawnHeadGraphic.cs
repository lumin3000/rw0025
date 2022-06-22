using System.IO;
using UnityEngine;

public class PawnHeadGraphic
{
	public string headName = string.Empty;

	public ObjectGraphic head;

	public Material matSleeping;

	public Material matIncap;

	public Material matDead;

	public PawnHeadGraphic(string graphPath)
	{
		headName = Path.GetFileNameWithoutExtension(graphPath);
		head = new ObjectGraphic(graphPath);
		matSleeping = head.MatAt(IntRot.south);
		matIncap = head.MatAt(IntRot.south);
		matDead = head.MatAt(IntRot.south);
	}
}
