using System.Text;
using UnityEngine;

public class DialogBox_Credits : DialogBox
{
	public DialogBox_Credits()
	{
		SetWinCentered(900f, 700f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(20f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Design/programming/art - Tynan Sylvester");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Title logo - Rhopunzel");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("RimWorld uses sounds generously uploaded to freesound.org by these generous individuals:\n\n    Research bubbling: Glaneur de sons\n    Click: TicTacShutUp\n    Shovel hits: shall555\n    Pick hits: cameronmusic\n    Building placement: joedeshon and HazMatt\n    Flesh impacts: harri\n    Food drop: JustinBW\n    Nutrient paste dispenser: raywilson\n    Weapon handling: S_Dij, KNO_SFX\n    Corpse drop: CosmicEmbers and Sauron974\n    Growing done pop: yottasounds\n    Flame burst: JoelAudio (Joel Azzopardi)\n    Interface pops: Volterock, patchen, broumbroum\n    Melee miss: yuval\n    Construction drill A: cmusounddesign\n    Construction drill B: AGFX\n    Construction ratchet: gelo_papas\n    Construction rummaging: D W\n    Eating: zebraphone\n    Grenade pin: ryanconway");
		GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), stringBuilder.ToString());
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}
}
