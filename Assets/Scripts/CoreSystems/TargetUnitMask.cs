using System;

namespace StormRend.Enums
{
	[Flags]
	public enum TargetUnitMask
	{
		Animates = 1 << 0,
		InAnimates = 1 << 1,
		Allies = 1 << 2,
		Enemies = 1 << 3,
		Crystals = 1 << 4,
	}
}