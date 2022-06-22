using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPanel_Designation : UIPanel
{
	public const float InfoRectHeight = 230f;

	private const int NumButsPerColumn = 3;

	private const float ButtonsStartX = 210f;

	protected List<Designator> desOptions = new List<Designator>();

	public DesignationDragger dragger = new DesignationDragger();

	public Designator selectedDesOption;

	private readonly Vector2 ButSpacing = new Vector2(6f, 14f);

	public UIPanel_Designation(IEnumerable<Designator> newDesList)
	{
		desOptions = newDesList.ToList();
	}

	public override void PanelClosing()
	{
		selectedDesOption = null;
	}

	private void SelectDesignator(Designator des)
	{
		selectedDesOption = des;
	}

	public override void PanelOnGUI(Rect fillRect)
	{
		if (selectedDesOption != null)
		{
			selectedDesOption.DesOptionOnGUI();
		}
		Designator designator = DrawButtonGrid();
		Rect infoRect = new Rect(0f, (float)(Screen.height - 35) - 152f - 230f, 200f, 230f);
		if (designator == null && selectedDesOption != null)
		{
			designator = selectedDesOption;
		}
		DrawMouseoverInfoBox(infoRect, designator);
		dragger.DraggerOnGUI();
	}

	public override void PanelUpdate()
	{
		dragger.DraggerUpdate();
		if (selectedDesOption != null)
		{
			selectedDesOption.DesignatorUpdate();
		}
	}

	public void AddModDesOptions(EntityCategory CatToUse)
	{
		foreach (ThingDefinition allThingDefinition in ThingDefDatabase.AllThingDefinitions)
		{
			if (allThingDefinition.category == CatToUse)
			{
				desOptions.Add(new Designator_Place(allThingDefinition));
			}
		}
	}

	protected Designator DrawButtonGrid()
	{
		GenUI.SetFontTiny();
		float num = Screen.height;
		Vector2 butSpacing = ButSpacing;
		float y = num - butSpacing.y - 75f;
		Vector2 vector = new Vector2(210f, y);
		int num2 = 0;
		Designator result = null;
		foreach (Designator desOption in desOptions)
		{
			if (desOption.Visible)
			{
				ButtonState buttonState = desOption.DrawOptButton(new Vector2(vector.x, vector.y));
				if (buttonState == ButtonState.Clicked)
				{
					SelectDesignator(desOption);
				}
				if (buttonState >= ButtonState.Mouseover)
				{
					result = desOption;
				}
				float x = vector.x;
				float y2 = vector.y;
				Vector2 butSpacing2 = ButSpacing;
				Rect rect = new Rect(x, y2, 75f, 75f + butSpacing2.y);
				rect = rect.GetInnerRect(-12f);
				GenUI.AbsorbClicksInRect(rect);
				float y3 = vector.y;
				Vector2 butSpacing3 = ButSpacing;
				vector.y = y3 - (75f + butSpacing3.y);
				num2++;
				if (num2 >= 3)
				{
					float x2 = vector.x;
					Vector2 butSpacing4 = ButSpacing;
					vector.x = x2 + (75f + butSpacing4.x);
					vector.y = y;
					num2 = 0;
				}
			}
		}
		return result;
	}

	protected void DrawMouseoverInfoBox(Rect InfoRect, Designator MouseoverOpt)
	{
		UIWidgets.DrawWindow(InfoRect);
		if (MouseoverOpt != null)
		{
			GUI.BeginGroup(InfoRect);
			Rect position = new Rect(10f, 10f, 999f, 999f);
			GenUI.SetFontSmall();
			GUI.Label(position, MouseoverOpt.buttonLabel);
			Designator_Place designator_Place = MouseoverOpt as Designator_Place;
			if (designator_Place != null)
			{
				int num = 35;
				foreach (ResourceCost cost in designator_Place.entDefToPlace.costList)
				{
					ThingDefinition thingDefinition = cost.rType.DefinitionOfType();
					GUI.DrawTexture(new Rect(10f, num, 20f, 20f), thingDefinition.uiIcon);
					if (Find.ResourceManager.TotalAmountOf(cost.rType) < cost.Amount)
					{
						GUI.color = Color.red;
					}
					GUI.Label(new Rect(36f, num + 2, 50f, 100f), cost.Amount.ToString());
					GUI.color = Color.white;
					num += 18;
				}
				num += 6;
			}
			Rect position2 = new Rect(10f, 90f, InfoRect.width - 20f, 999f);
			GUI.Label(position2, MouseoverOpt.DescText);
			GUI.EndGroup();
		}
		GenUI.AbsorbClicksInRect(InfoRect.GetInnerRect(-8f));
	}
}
