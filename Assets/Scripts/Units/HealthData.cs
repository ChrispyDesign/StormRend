using System;

namespace StormRend.Units
{
	[Serializable]
	public struct HealthData
	{
		public Unit vendor;
		public int amount;

		public HealthData(Unit vendor, int amount)
		{
			this.vendor = vendor;
			this.amount = amount;
		}
	}
}
