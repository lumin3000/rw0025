using System;
using UnityEngine;

public static class TradeSliders
{
	private const int DragTimeGrowMinimumOffset = 300;

	private const float DragVolumeMax = 0.1f;

	private const float DragLooperFalloffRate = 8f;

	public static EntityType dragEntityType;

	public static int dragBaseAmount;

	public static bool dragLimitWarningGiven = false;

	private static SoundLooper dragLooper;

	private static readonly AudioClip DragStartClip = UISounds.TickHigh;

	private static readonly AudioClip DragProgressClip = UISounds.DragLoopMeta;

	private static readonly AudioClip DragEndClip = UISounds.TickLow;

	public static void TradeSliderDraggingUpdate(float mouseOffX)
	{
		int num = dragBaseAmount;
		if (Math.Abs(mouseOffX) > 300f)
		{
			if (mouseOffX > 0f)
			{
				dragBaseAmount--;
			}
			else
			{
				dragBaseAmount++;
			}
		}
		int num2 = dragBaseAmount - (int)(mouseOffX / 4f);
		int num3 = TradeSession.curDeal.AmountPlayerBuying(dragEntityType);
		AcceptanceReport acceptanceReport = null;
		while (num3 != num2)
		{
			if (num2 > num3)
			{
				acceptanceReport = TradeSession.curDeal.TryIncrementAmount(dragEntityType);
			}
			if (num2 < num3)
			{
				acceptanceReport = TradeSession.curDeal.TryDecrementAmount(dragEntityType);
			}
			if (!acceptanceReport.accepted)
			{
				if (!dragLimitWarningGiven)
				{
					UI_Messages.Message(acceptanceReport.reasonText, UIMessageSound.Reject);
					dragLimitWarningGiven = true;
				}
				dragBaseAmount = num;
				break;
			}
			num3 = ((num2 <= num3) ? (num3 - 1) : (num3 + 1));
			dragLimitWarningGiven = false;
		}
		if (acceptanceReport != null && acceptanceReport.accepted)
		{
			if (dragLooper == null)
			{
				GenSound.PlaySoundOnCamera(DragStartClip, 0.1f);
				dragLooper = new SoundLooperCamera(DragProgressClip, 0.1f, SoundLooperMaintenanceType.PerFrame);
			}
			else
			{
				float num4 = 0f - mouseOffX;
				if (num4 > 300f)
				{
					num4 = 300f;
				}
				if (num4 < -300f)
				{
					num4 = -300f;
				}
				float num5 = num4 / 40f;
				float pitch = (float)Math.Pow(1.05946, num5);
				dragLooper.looperSource.pitch = pitch;
			}
			dragLooper.Volume = 0.1f;
		}
		if (dragLooper != null)
		{
			dragLooper.Maintain();
			dragLooper.Volume -= Time.deltaTime * 8f * 0.1f;
		}
	}

	public static void TradeSliderDraggingCompleted(float mouseOffX)
	{
		dragLooper = null;
		GenSound.PlaySoundOnCamera(DragEndClip, 0.1f);
	}
}
