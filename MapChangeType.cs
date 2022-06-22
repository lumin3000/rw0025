using System;

[Flags]
public enum MapChangeType
{
	None = 0x1,
	Things = 0x2,
	FogOfWar = 0x4,
	Blockers = 0x8,
	GroundGlow = 0x10,
	Terrain = 0x20,
	Roofs = 0x40,
	HomeZone = 0x80,
	PowerGrid = 0x100
}
