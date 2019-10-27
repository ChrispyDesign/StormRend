using System;

namespace StormRend.Enums
{
	//Flags and Enums
	[Flags]
	public enum TargetMask
	{
		Empty = 1 << 0,     //Empty is when no bits are selected!
		Self = 1 << 1,
		Allies = 1 << 2,
		Enemies = 1 << 3,
		Crystals = 1 << 4,
		InAnimates = 1 << 5,    //Such as crystals
		Animates = 1 << 6,
	}
}