using System.Collections.Generic;
using UnityEngine;

public class CompForbiddable : ThingComp
{
	public bool forbidden;

	private static readonly Texture2D ButtonIconForbidden = Res.LoadTexture("UI/Commands/Forbidden");

	public override void CompExposeData()
	{
		Scribe.LookField(ref forbidden, "Forbidden", forceSave: false);
	}

	public override void CompDraw()
	{
		if (forbidden)
		{
			OverlayDrawer.DrawOverlay(parent, OverlayTypes.Forbidden);
		}
	}

	public override IEnumerable<Command> CompCommands()
	{
		Command_Toggle opt = new Command_Toggle
		{
			hotKey = KeyCode.F,
			icon = ButtonIconForbidden,
			isActive = () => !forbidden,
			action = delegate
			{
				forbidden = !forbidden;
			}
		};
		if (forbidden)
		{
			opt.tipDef = new TooltipDef("Forbidden - colonists will not interact with this object.");
		}
		else
		{
			opt.tipDef = new TooltipDef("Not forbidden - colonists may interact with this object.");
		}
		yield return opt;
	}
}
