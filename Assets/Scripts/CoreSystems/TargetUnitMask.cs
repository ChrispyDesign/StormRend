using System;

namespace StormRend.Enums
{
	[Flags]
	public enum TargetUnitMask
	{
		Allies = 1 << 0,
		Enemies = 1 << 1,
		InAnimates = 1 << 2,
		Crystals = 1 << 3,
	}
}