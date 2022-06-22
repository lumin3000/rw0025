using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ThingSelector
{
	private const float PawnSelectRadius = 1f;

	private const int MaxNumSelected = 80;

	protected DragBox dragBox = new DragBox();

	private List<Thing> selectedThings = new List<Thing>();

	private Dictionary<Thing, float> selectTimes = new Dictionary<Thing, float>();

	private readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", MatBases.MetaOverlay);

	private float SelJumpDuration = 0.07f;

	private float SelJumpDistance = 0.2f;

	private bool ShiftIsHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

	public IEnumerable<Thing> SelectedThings
	{
		get
		{
			foreach (Thing selectedThing in selectedThings)
			{
				yield return selectedThing;
			}
		}
	}

	public Thing SingleSelectedThing
	{
		get
		{
			if (selectedThings.Count != 1)
			{
				return null;
			}
			return selectedThings[0];
		}
	}

	public int NumSelected => selectedThings.Count;

	public void SelectorOnGUI()
	{
		SelectorHandleWorldClicks();
		dragBox.DragBoxOnGUI();
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && selectedThings.Count > 0)
		{
			ClearSelection();
			Event.current.Use();
		}
	}

	public void SelectorUpdate()
	{
		DrawSelectionBrackets();
		DrawSelectedPawnPaths();
		foreach (Thing selectedThing in selectedThings)
		{
			selectedThing.DrawSelectedExtras();
		}
	}

	public void SelectorHandleWorldClicks()
	{
		if (Event.current.type == EventType.MouseDown)
		{
			if (Event.current.button == 0)
			{
				if (Event.current.clickCount == 1)
				{
					dragBox.active = true;
					dragBox.start = Gen.MouseWorldPosVector3();
				}
				if (Event.current.clickCount == 2)
				{
					SelectAllOfTypeUnderMouseOnScreen();
				}
			}
			if (Event.current.button == 1)
			{
				if (selectedThings.Count == 1 && selectedThings[0] is Pawn)
				{
					Find.FloatMenu.MakeAtMouseFor((Pawn)selectedThings[0]);
				}
				else
				{
					foreach (Thing selectedThing in selectedThings)
					{
						Pawn pawn = selectedThing as Pawn;
						if (pawn != null)
						{
							AutoOrderToSquare(pawn, Gen.MouseWorldSquare());
						}
					}
				}
			}
			Event.current.Use();
		}
		if (Event.current.type != EventType.MouseUp)
		{
			return;
		}
		if (Event.current.button == 0 && dragBox.active)
		{
			dragBox.active = false;
			if (!dragBox.IsValid)
			{
				SelectUnderMouse();
			}
			else
			{
				SelectInsideDragBox();
			}
		}
		Event.current.Use();
	}

	private static void AutoOrderToSquare(Pawn pawn, IntVec3 dest)
	{
		foreach (FloatMenuChoice item in FloatMenuMaker.ChoicesAtFor(dest, pawn))
		{
			if (item.autoTakeable)
			{
				item.Chosen();
				break;
			}
		}
	}

	public IEnumerable<Thing> SelectableThingsUnderMouse()
	{
		if (!Gen.MouseWorldSquare().InBounds())
		{
			yield break;
		}
		List<Thing> selectableList = GenUI.ThingsUnderMouse(1f, new TargetingParameters
		{
			mustBeSelectable = true,
			targetTeams = TargetingParameters.AllTeams,
			canTargetPawns = true,
			canTargetBuildings = true,
			canTargetSmallObjects = true
		});
		if (selectableList.Count > 0 && selectableList[0] is Pawn && (selectableList[0].DrawPos - Gen.MouseWorldPosVector3()).MagnitudeHorizontal() < 0.4f)
		{
			foreach (Thing t in selectableList.ListFullCopy())
			{
				if (t.def.eType == EntityType.Pawn && (t.DrawPos - Gen.MouseWorldPosVector3()).MagnitudeHorizontal() > 0.4f)
				{
					selectableList.Remove(t);
				}
			}
		}
		foreach (Thing item in selectableList)
		{
			yield return item;
		}
	}

	private void SelectUnderMouse()
	{
		List<Thing> list = SelectableThingsUnderMouse().ToList();
		if (list.Count == 0)
		{
			if (!ShiftIsHeld)
			{
				ClearSelection();
			}
		}
		else if (list.Count == 1)
		{
			Thing thing = list[0];
			if (!ShiftIsHeld)
			{
				ClearSelection();
				Select(thing);
			}
			else if (!selectedThings.Contains(thing))
			{
				Select(thing);
			}
			else
			{
				Deselect(thing);
			}
		}
		else
		{
			if (list.Count <= 1)
			{
				return;
			}
			Thing thing2 = list.Where((Thing t) => selectedThings.Contains(t)).FirstOrDefault();
			if (thing2 != null)
			{
				if (!ShiftIsHeld)
				{
					int num = list.IndexOf(thing2) + 1;
					if (num >= list.Count)
					{
						num -= list.Count;
					}
					ClearSelection();
					Select(list[num]);
					return;
				}
				foreach (Thing item in list)
				{
					if (selectedThings.Contains(item))
					{
						Deselect(item);
					}
				}
			}
			else
			{
				if (!ShiftIsHeld)
				{
					ClearSelection();
				}
				Select(list[0]);
			}
		}
	}

	private void SelectAllOfTypeUnderMouseOnScreen()
	{
		List<Thing> underMouseThings = SelectableThingsUnderMouse().ToList();
		if (underMouseThings.Count > 0 && !underMouseThings[0].def.neverMultiSelect)
		{
			TrySelectAllWith((Thing t) => t.def == underMouseThings[0].def && !IsSelected(t), SelectionUtility.MultiSelectableThingsInRect(new Rect(0f, 0f, Screen.width, Screen.height)));
		}
	}

	private void SelectInsideDragBox()
	{
		if (!ShiftIsHeld)
		{
			ClearSelection();
		}
		if (!TrySelectAllWith((Thing t) => dragBox.Contains(t), dragBox.ContainedThings()))
		{
			SelectUnderMouse();
		}
	}

	private bool TrySelectAllWith(Func<Thing, bool> validator, IEnumerable extraThings)
	{
		Func<IEnumerable, bool> func = delegate(IEnumerable thingEnum)
		{
			bool result = false;
			foreach (Thing item in thingEnum)
			{
				if (item.Selectable && !item.def.neverMultiSelect && validator(item))
				{
					Select(item);
					result = true;
				}
			}
			return result;
		};
		if (func(Find.PawnManager.Colonists))
		{
			return true;
		}
		if (func(Find.PawnManager.Hostiles))
		{
			return true;
		}
		if (func(Find.PawnManager.AllPawns))
		{
			return true;
		}
		if (func(Find.BuildingManager.AllBuildingsColonist))
		{
			return true;
		}
		if (func(Find.BuildingManager.AllBuildingsArtificial.Where((Building b) => b.Team.IsHostileToTeam(TeamType.Colonist))))
		{
			return true;
		}
		if (func(Find.BuildingManager.AllBuildingsArtificial))
		{
			return true;
		}
		if (func(extraThings))
		{
			return true;
		}
		return false;
	}

	private int TeamSelectionPriority(TeamType Team)
	{
		if (Team == TeamType.Colonist)
		{
			return 99;
		}
		if (Team.IsHostileToTeam(TeamType.Colonist))
		{
			return 1;
		}
		return 0;
	}

	private void Select(Thing t)
	{
		if (selectedThings.Count < 80)
		{
			if (t.def.eType == EntityType.Pawn && t.Team == TeamType.Colonist)
			{
				GenSound.PlaySoundOnCamera("Radio/RadioFuzzOpen", 0.02f, SoundSlot.RadioFuzz);
			}
			else
			{
				GenSound.PlaySoundOnCamera("Interface/SelectThing", 0.1f);
			}
			selectedThings.Add(t);
			if (!selectTimes.ContainsKey(t))
			{
				selectTimes.Add(t, Time.realtimeSinceStartup);
			}
			else
			{
				selectTimes[t] = Time.realtimeSinceStartup;
			}
		}
	}

	public bool IsSelected(Thing t)
	{
		return selectedThings.Contains(t);
	}

	public void ClearSelection()
	{
		selectTimes.Clear();
		selectedThings.Clear();
	}

	public void Deselect(Thing t)
	{
		if (selectedThings.Contains(t))
		{
			selectedThings.Remove(t);
		}
	}

	private void DrawSelectionBrackets()
	{
		foreach (Thing selectedThing in selectedThings)
		{
			DrawSelectionBracketFor(selectedThing);
		}
	}

	private void DrawSelectionBracketFor(Thing thing)
	{
		Vector3[] array = new Vector3[4]
		{
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3)
		};
		Vector3 vector = thing.TrueCenter();
		Pawn pawn = thing as Pawn;
		if (pawn != null)
		{
			vector = pawn.drawer.DrawPos;
		}
		Vector2 vector2 = thing.RotatedSize.ToVector2() * 0.5f;
		Vector2 vector3 = vector2;
		vector3.x -= 0.5f;
		vector3.y -= 0.5f;
		float num = 1f - (Time.realtimeSinceStartup - selectTimes[thing]) / SelJumpDuration;
		if (num < 0f)
		{
			num = 0f;
		}
		float num2 = num * SelJumpDistance;
		vector3.x += num2;
		vector3.y += num2;
		ref Vector3 reference = ref array[0];
		reference = new Vector3(vector.x - vector3.x, 0f, vector.z - vector3.y);
		ref Vector3 reference2 = ref array[1];
		reference2 = new Vector3(vector.x + vector3.x, 0f, vector.z - vector3.y);
		ref Vector3 reference3 = ref array[2];
		reference3 = new Vector3(vector.x + vector3.x, 0f, vector.z + vector3.y);
		ref Vector3 reference4 = ref array[3];
		reference4 = new Vector3(vector.x - vector3.x, 0f, vector.z + vector3.y);
		int num3 = 0;
		for (int i = 0; i < 4; i++)
		{
			array[i].y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
			Quaternion rotation = Quaternion.AngleAxis(num3, Vector3.up);
			Graphics.DrawMesh(MeshPool.plane10, array[i], rotation, SelectionBracketMat, 0);
			num3 -= 90;
		}
	}

	private void DrawSelectedPawnPaths()
	{
		foreach (Thing selectedThing in selectedThings)
		{
			if (selectedThing.def.eType == EntityType.Pawn && selectedThing.Team == TeamType.Colonist)
			{
				Pawn pawn = (Pawn)selectedThing;
				if (pawn.pather.curPath != null)
				{
					pawn.pather.curPath.DrawPath();
				}
			}
		}
	}
}
