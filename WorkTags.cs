using System;

[Flags]
public enum WorkTags
{
	None = 0x0,
	Intellectual = 0x1,
	ManualDumb = 0x2,
	ManualSkilled = 0x4,
	Violent = 0x8,
	Caring = 0x10,
	Social = 0x20,
	Scary = 0x40,
	Artistic = 0x80,
	Crafting = 0x100,
	Cooking = 0x200,
	Firefighting = 0x400
}
