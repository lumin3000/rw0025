using System;
using UnityEngine;

public abstract class Designator
{
	public string buttonLabel = "No label";

	protected string defaultDesc = "No description.";

	public Texture2D buttonTexture;

	public Vector2 buttonTextureProps = Vector2.one;

	public bool buttonTextureOverdraw;

	public AudioClip dragStartClip = UISounds.Click;

	public AudioClip dragProgressClip = UISounds.WhiteNoiseLoop;

	public float dragVolumeMax = 0.1f;

	protected bool useMouseIcon;

	protected readonly Texture2D ButtonBGTex = Res.LoadTexture("UI/Widgets/DesButBG");

	public virtual string DescText => defaultDesc;

	public virtual bool Visible => true;

	public virtual int DraggableDimensions => 2;

	public virtual ButtonState DrawOptButton(Vector2 Loc)
	{
		Rect rect = new Rect(Loc.x, Loc.y, 75f, 75f);
		bool flag = false;
		if (rect.Contains(Event.current.mousePosition))
		{
			flag = true;
			GUI.color = GenUI.MouseoverColor;
		}
		Texture2D missingContentTex = buttonTexture;
		if (missingContentTex == null)
		{
			missingContentTex = GenUI.MissingContentTex;
		}
		bool flag2 = GUI.Button(rect, string.Empty);
		GUI.DrawTexture(rect, ButtonBGTex);
		float num = 0.85f;
		Vector2 texProportions = buttonTextureProps;
		if (buttonTextureOverdraw)
		{
			float num2 = (buttonTextureProps.x + 2f) / buttonTextureProps.x;
			float num3 = (buttonTextureProps.y + 2f) / buttonTextureProps.y;
			texProportions.x *= num2;
			texProportions.y *= num3;
			num *= Math.Min(num2, num3);
		}
		UIWidgets.DrawTextureFitted(new Rect(rect), missingContentTex, num, texProportions);
		float num4 = GUI.skin.label.CalcHeight(new GUIContent(buttonLabel), rect.width);
		num4 -= 2f;
		Rect position = new Rect(rect.x, rect.yMax - num4 + 12f, rect.width, num4);
		GUI.DrawTexture(position, GenUI.GrayTextBG);
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(position, buttonLabel);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		if (flag2)
		{
			GenSound.PlaySoundOnCamera(UISounds.TickTiny, 0.1f);
			return ButtonState.Clicked;
		}
		if (flag)
		{
			return ButtonState.Mouseover;
		}
		return ButtonState.Clear;
	}

	public virtual AcceptanceReport CanDesignateAt(IntVec3 loc)
	{
		if (!loc.InBounds())
		{
			return "Out of bounds.";
		}
		return true;
	}

	public virtual void DesOptionOnGUI()
	{
		if (useMouseIcon)
		{
			GenUI.DrawMouseIcon(buttonTexture);
		}
	}

	public abstract void DesignateAt(IntVec3 sq);

	public virtual void FinalizeDesignationSucceeded()
	{
	}

	public virtual void FinalizeDesignationFailed()
	{
	}

	public virtual void DesignatorUpdate()
	{
	}

	public virtual void Rotate(RotationDirection RotDir)
	{
	}
}
