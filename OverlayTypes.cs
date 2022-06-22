using System;

[Flags]
public enum OverlayTypes
{
	NeedsO2 = 0x1,
	NeedsPower = 0x2,
	PowerOff = 0x4,
	Damaged = 0x8,
	BurningWick = 0x10,
	Forbidden = 0x20
}
