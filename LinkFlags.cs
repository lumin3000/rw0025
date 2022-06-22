using System;

[Flags]
public enum LinkFlags
{
	None = 0x0,
	Wall = 0x1,
	Sandbags = 0x2,
	Rock = 0x4,
	Minerals = 0x8,
	PowerConduit = 0x10,
	MapEdge = 0x20
}
