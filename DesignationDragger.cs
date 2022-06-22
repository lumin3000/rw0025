using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesignationDragger
{
	private const float LooperFalloffRate = 8f;

	private const int MaxSquareWidth = 50;

	public bool dragging;

	private IntVec3 startDragSquare;

	private int lastFrameDragSquaresDrawn;

	private SoundLooper dragLooper;

	private static readonly Material DragHighlightSquareMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightSquare", MatBases.MetaOverlay);

	private Designator SelOption
	{
		get
		{
			return Find.UIMapRoot.modeControls.tabArchitect.selectedPanel.selectedDesOption;
		}
		set
		{
			Find.UIMapRoot.modeControls.tabArchitect.selectedPanel.selectedDesOption = value;
		}
	}

	public IEnumerable<IntVec3> DragSquares => DragSquaresBetween(startDragSquare, Gen.MouseWorldSquare(), SelOption.DraggableDimensions);

	public void DraggerOnGUI()
	{
		if (SelOption == null)
		{
			return;
		}
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
		{
			if (SelOption != null && SelOption.DraggableDimensions == 0)
			{
				AcceptanceReport acceptanceReport = SelOption.CanDesignateAt(Gen.MouseWorldSquare());
				if (acceptanceReport.accepted)
				{
					SelOption.DesignateAt(Gen.MouseWorldSquare());
					SelOption.FinalizeDesignationSucceeded();
				}
				else
				{
					UI_Messages.Message(acceptanceReport.reasonText, UIMessageSound.Reject);
					SelOption.FinalizeDesignationFailed();
				}
			}
			else
			{
				dragging = true;
				startDragSquare = Gen.MouseWorldSquare();
				GenSound.PlaySoundOnCamera(SelOption.dragStartClip, 0.2f);
				dragLooper = new SoundLooperCamera(SelOption.dragProgressClip, SelOption.dragVolumeMax, SoundLooperMaintenanceType.PerFrame);
			}
			Event.current.Use();
		}
		if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && SelOption != null)
		{
			GenSound.PlaySoundOnCamera("Interface/OptionDeselected", 0.2f);
			SelOption = null;
			dragging = false;
			Event.current.Use();
		}
		if (Event.current.type != EventType.MouseUp || Event.current.button != 0 || !dragging)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		dragging = false;
		foreach (IntVec3 dragSquare in DragSquares)
		{
			AcceptanceReport acceptanceReport2 = SelOption.CanDesignateAt(dragSquare);
			if (acceptanceReport2.accepted)
			{
				SelOption.DesignateAt(dragSquare);
				flag2 = true;
			}
			else if (!flag)
			{
				UI_Messages.Message(acceptanceReport2.reasonText, UIMessageSound.Reject);
				flag = true;
			}
		}
		if (flag2)
		{
			SelOption.FinalizeDesignationSucceeded();
		}
		else
		{
			SelOption.FinalizeDesignationFailed();
		}
		Event.current.Use();
	}

	public void DraggerUpdate()
	{
		if (dragging)
		{
			DrawDragHighlightSquares();
			if (dragLooper != null)
			{
				dragLooper.Maintain();
			}
		}
	}

	public void CancelDrag()
	{
		dragging = false;
	}

	private void DrawDragHighlightSquares()
	{
		using (IEnumerator<IntVec3> enumerator = DragSquares.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Graphics.DrawMesh(position: enumerator.Current.ToVector3Shifted() + 10f * Vector3.up, mesh: MeshPool.plane10, rotation: Quaternion.identity, material: DragHighlightSquareMat, layer: 0);
			}
		}
		if (DragSquares.Count() != lastFrameDragSquaresDrawn)
		{
			dragLooper.Volume = SelOption.dragVolumeMax;
			lastFrameDragSquaresDrawn = DragSquares.Count();
		}
		else
		{
			dragLooper.Volume -= Time.deltaTime * 8f * SelOption.dragVolumeMax;
		}
	}

	private IEnumerable<IntVec3> DragSquaresBetween(IntVec3 start, IntVec3 end, int numDimensions)
	{
		if (numDimensions == 1)
		{
			bool horizontal = true;
			if (Math.Abs(start.x - end.x) < Math.Abs(start.z - end.z))
			{
				horizontal = false;
			}
			if (horizontal)
			{
				int StartZ = start.z;
				if (start.x > end.x)
				{
					IntVec3 temp4 = start;
					start = end;
					end = temp4;
				}
				for (int k = start.x; k <= end.x; k++)
				{
					IntVec3 candidate3 = new IntVec3(k, start.y, StartZ);
					if (SelOption.CanDesignateAt(candidate3).accepted)
					{
						yield return candidate3;
					}
				}
			}
			else
			{
				int StartX = start.x;
				if (start.z > end.z)
				{
					IntVec3 temp3 = start;
					start = end;
					end = temp3;
				}
				for (int j = start.z; j <= end.z; j++)
				{
					IntVec3 candidate2 = new IntVec3(StartX, start.y, j);
					if (SelOption.CanDesignateAt(candidate2).accepted)
					{
						yield return candidate2;
					}
				}
			}
		}
		if (numDimensions != 2)
		{
			yield break;
		}
		IntVec3 locStart = start;
		IntVec3 locEnd = end;
		if (locEnd.x > locStart.x + 50)
		{
			locEnd.x = locStart.x + 50;
		}
		if (locEnd.z > locStart.z + 50)
		{
			locEnd.z = locStart.z + 50;
		}
		if (locEnd.x < locStart.x)
		{
			if (locEnd.x < locStart.x - 50)
			{
				locEnd.x = locStart.x - 50;
			}
			int temp2 = locStart.x;
			locStart = new IntVec3(locEnd.x, locStart.y, locStart.z);
			locEnd = new IntVec3(temp2, locEnd.y, locEnd.z);
		}
		if (locEnd.z < locStart.z)
		{
			if (locEnd.z < locStart.z - 50)
			{
				locEnd.z = locStart.z - 50;
			}
			int temp = locStart.z;
			locStart = new IntVec3(locStart.x, locStart.y, locEnd.z);
			locEnd = new IntVec3(locEnd.x, locEnd.y, temp);
		}
		for (int i = locStart.x; i <= locEnd.x; i++)
		{
			for (int l = locStart.z; l <= locEnd.z; l++)
			{
				IntVec3 candidate = new IntVec3(i, locStart.y, l);
				if (SelOption.CanDesignateAt(candidate).accepted)
				{
					yield return candidate;
				}
			}
		}
	}
}
