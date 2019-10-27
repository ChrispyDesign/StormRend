using System;

namespace StormRend.Enums
{
	//Flags and Enums
	[Flags]
	public enum TargetTileMask
	{
		Empty = 1 << 0,     //Empty is when no bits are selected!
		Self = 1 << 1,
		AllyUnits = 1 << 2,
		EnemyUnits = 1 << 3,
		InAnimates = 1 << 4,    //Such as crystals
		Animates = 1 << 5,
	}
}