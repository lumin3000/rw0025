using System.Collections.Generic;
using UnityEngine;

public class Room : Saveable
{
	private const float AirShowThreshold = 0.9f;

	public List<IntVec3> squaresList = new List<IntVec3>();

	public float airInt;

	public bool roomDeleted;

	public bool airTight;

	private int randomDebugIndex;

	private static readonly Color AirNumberColor = new Color(1f, 0.3f, 0.3f, 0.6f);

	private static readonly Vector2 AirNumberSize = new Vector2(40f, 19f);

	public float Air
	{
		get
		{
			return airInt;
		}
		set
		{
			if (Debug.isDebugBuild && !airTight)
			{
				Debug.LogError("Trying to set air on non-enclosed room at " + squaresList[0]);
				return;
			}
			airInt = value;
			if (airInt < 0f)
			{
				airInt = 0f;
			}
			if (Debug.isDebugBuild && airInt > MaxAir * 1.1f)
			{
				Debug.LogWarning("Pushing much more air into room than it can take. Pressure=" + AirPressure);
			}
		}
	}

	public float MaxAir => squaresList.Count;

	public float AirPressure => Air / MaxAir;

	public Pawn RoomOwner
	{
		get
		{
			Pawn pawn = null;
			foreach (Building_Bed containedBed in ContainedBeds)
			{
				if (containedBed.owner != null)
				{
					if (pawn != null)
					{
						return null;
					}
					pawn = containedBed.owner;
				}
			}
			return pawn;
		}
	}

	public bool IsPrisonCell
	{
		get
		{
			foreach (IntVec3 squares in squaresList)
			{
				Building_Bed building_Bed = Find.Grids.ThingAt<Building_Bed>(squares);
				if (building_Bed != null && building_Bed.forPrisoners)
				{
					return true;
				}
			}
			return false;
		}
	}

	public IEnumerable<Building_Bed> ContainedBeds
	{
		get
		{
			HashSet<Building_Bed> hashSet = new HashSet<Building_Bed>();
			foreach (IntVec3 squares in squaresList)
			{
				foreach (Thing item in Find.Grids.ThingsAt(squares))
				{
					Building_Bed building_Bed = item as Building_Bed;
					if (building_Bed != null && !hashSet.Contains(building_Bed))
					{
						hashSet.Add(building_Bed);
					}
				}
			}
			return hashSet;
		}
	}

	private bool ShouldDrawAir => !DebugSettings.worldBreathable && airTight && AirPressure < 0.9f;

	public IEnumerable<Thing> AllContainedThings
	{
		get
		{
			Grids grids = Find.Grids;
			foreach (IntVec3 sq in squaresList)
			{
				foreach (Thing item in grids.ThingsAt(sq))
				{
					yield return item;
				}
			}
		}
	}

	public Room()
	{
		randomDebugIndex = Random.Range(0, 100000);
	}

	public Room(List<IntVec3> squaresList)
		: this()
	{
		this.squaresList = squaresList;
		RoofMaker.RoofGenerationRequest(this);
		airTight = true;
		foreach (IntVec3 squares in squaresList)
		{
			if (!Find.RoofGrid.SquareIsRoofed(squares))
			{
				airTight = false;
			}
		}
		Find.Grids.RegisterInRoomMap(this);
		if (airTight)
		{
			AirGrid.FillFromAirGrid(this);
		}
		bool isPrisonCell = IsPrisonCell;
		foreach (Building_Bed containedBed in ContainedBeds)
		{
			containedBed.forPrisoners = isPrisonCell;
		}
	}

	public IEnumerator<IntVec3> GetEnumerator()
	{
		foreach (IntVec3 squares in squaresList)
		{
			yield return squares;
		}
	}

	public void ExposeData()
	{
		Scribe.LookField(ref airTight, "AirTight", forceSave: true);
		Scribe.LookField(ref airInt, "Air", forceSave: true);
		Scribe.LookList(ref squaresList, "Squares");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			Find.Grids.RegisterInRoomMap(this);
		}
	}

	public void RoomDraw()
	{
		if (DebugSettings.drawRooms)
		{
			foreach (IntVec3 squares in squaresList)
			{
				DebugRender.RenderSquare(squares, randomDebugIndex);
			}
		}
		else if (ShouldDrawAir)
		{
			GenRender.RenderFieldSolid(squaresList, GenMap.GetAirMaterial(AirPressure), Altitudes.AltitudeFor(AltitudeLayer.Room));
		}
	}

	public void RoomOnGUI()
	{
		if (!ShouldDrawAir)
		{
			return;
		}
		Vector3 vector = default(Vector3);
		foreach (IntVec3 squares in squaresList)
		{
			vector.x += squares.x;
			vector.z += squares.z;
		}
		vector /= (float)squaresList.Count;
		vector += new Vector3(0.5f, 0f, 0.5f);
		Vector2 vector2;
		if (squaresList.Contains(vector.ToIntVec3()))
		{
			vector2 = vector.ToScreenPosition();
		}
		else
		{
			int num = 0;
			num = squaresList.Count / 2;
			vector2 = squaresList[num].ToVector3Shifted().ToScreenPosition();
		}
		GUI.color = AirNumberColor;
		GenUI.SetFontSmall();
		float x = vector2.x;
		Vector2 airNumberSize = AirNumberSize;
		float left = x - airNumberSize.x / 2f;
		float y = vector2.y;
		Vector2 airNumberSize2 = AirNumberSize;
		float top = y - airNumberSize2.y / 2f;
		Vector2 airNumberSize3 = AirNumberSize;
		float x2 = airNumberSize3.x;
		Vector2 airNumberSize4 = AirNumberSize;
		Rect position = new Rect(left, top, x2, airNumberSize4.y);
		GUI.DrawTexture(position, GenUI.GrayTextBG);
		position.y += 2f;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(position, (AirPressure * 100f).ToString("##0") + "%");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
	}

	public void Delete()
	{
		roomDeleted = true;
		Find.Grids.DeRegisterInRoomMap(this);
		Find.RoomManager.allRooms.Remove(this);
		AirGrid.AddToAirGrid(this);
	}

	public bool ContainsSquare(IntVec3 Sq)
	{
		foreach (IntVec3 squares in squaresList)
		{
			if (Sq == squares)
			{
				return true;
			}
		}
		return false;
	}

	public bool ContainsThingOfType(EntityType ttype)
	{
		foreach (IntVec3 squares in squaresList)
		{
			foreach (Thing item in Find.Grids.ThingsAt(squares))
			{
				if (item.def.eType == ttype)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string ToString()
	{
		return string.Concat("[Room First=", squaresList[0], " DebugIndex=", randomDebugIndex, " Count=", squaresList.Count, "]");
	}
}
