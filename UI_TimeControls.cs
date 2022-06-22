using System;
using UnityEngine;

public static class UI_TimeControls
{
	private const float ClockSoundVolume = 0.15f;

	public static readonly Vector2 TimeButSize = new Vector2(32f, 24f);

	private static readonly AudioClip[] SpeedSounds = new AudioClip[5]
	{
		Res.LoadSound("Interface/Clock/ClockStops"),
		Res.LoadSound("Interface/Clock/ClockTickingNormal"),
		Res.LoadSound("Interface/Clock/ClockTickingFast"),
		Res.LoadSound("Interface/Clock/ClockTickingSuperfast"),
		Res.LoadSound("Interface/Clock/ClockTickingSuperfast")
	};

	private static readonly Texture2D[] SpeedButtonTextures = new Texture2D[5]
	{
		Res.LoadTexture("UI/TimeControls/TimeSpeedButton_Pause"),
		Res.LoadTexture("UI/TimeControls/TimeSpeedButton_Normal"),
		Res.LoadTexture("UI/TimeControls/TimeSpeedButton_Fast"),
		Res.LoadTexture("UI/TimeControls/TimeSpeedButton_Superfast"),
		Res.LoadTexture("UI/TimeControls/TimeSpeedButton_Superfast")
	};

	private static void PlaySoundOf(TimeSpeed Speed)
	{
		GenSound.PlaySoundOnCamera(SpeedSounds[(int)Speed], 0.15f);
	}

	public static void DoTimeControlsGUI(Rect timerRect)
	{
		TickManager tickManager = Find.TickManager;
		GUI.BeginGroup(timerRect);
		Vector2 timeButSize = TimeButSize;
		float x = timeButSize.x;
		Vector2 timeButSize2 = TimeButSize;
		Rect rect = new Rect(0f, 0f, x, timeButSize2.y);
		foreach (int value in Enum.GetValues(typeof(TimeSpeed)))
		{
			if (value == 4)
			{
				continue;
			}
			if (UIWidgets.ImageButton(rect, SpeedButtonTextures[value]))
			{
				if (value == 0)
				{
					tickManager.TogglePaused();
				}
				else
				{
					tickManager.curTimeSpeed = (TimeSpeed)value;
				}
				PlaySoundOf(tickManager.curTimeSpeed);
			}
			if (tickManager.curTimeSpeed == (TimeSpeed)value)
			{
				GUI.DrawTexture(rect, GenUI.HighlightTex);
			}
			rect.x += rect.width;
		}
		if (Find.TickManager.slower.ForcedNormalSpeed)
		{
			GenUI.DrawLineHorizontal(new Vector2(timerRect.width / 2f, timerRect.height / 2f), timerRect.width * 0.45f);
		}
		GUI.EndGroup();
		if (Event.current.type == EventType.KeyDown)
		{
			if (Event.current.keyCode == KeyCode.Space)
			{
				Find.TickManager.TogglePaused();
				PlaySoundOf(Find.TickManager.curTimeSpeed);
				Event.current.Use();
			}
			if (Event.current.keyCode == KeyCode.Alpha1)
			{
				Find.TickManager.curTimeSpeed = TimeSpeed.Normal;
				PlaySoundOf(Find.TickManager.curTimeSpeed);
				Event.current.Use();
			}
			if (Event.current.keyCode == KeyCode.Alpha2)
			{
				Find.TickManager.curTimeSpeed = TimeSpeed.Fast;
				PlaySoundOf(Find.TickManager.curTimeSpeed);
				Event.current.Use();
			}
			if (Event.current.keyCode == KeyCode.Alpha3)
			{
				Find.TickManager.curTimeSpeed = TimeSpeed.Superfast;
				PlaySoundOf(Find.TickManager.curTimeSpeed);
				Event.current.Use();
			}
		}
		if (Debug.isDebugBuild)
		{
			if (tickManager.curTimeSpeed == TimeSpeed.Paused && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.KeypadPlus)
			{
				tickManager.DoSingleTick();
				GenSound.PlaySoundOnCamera(SpeedSounds[0], 0.075f);
			}
			if (Debug.isDebugBuild && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha4)
			{
				Find.TickManager.curTimeSpeed = TimeSpeed.DebugUltrafast;
			}
		}
		Vector2 point = Input.mousePosition;
		point.y = (float)Screen.height - point.y;
		if (timerRect.Contains(point) && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp))
		{
			Event.current.Use();
		}
	}
}
