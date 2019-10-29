using System;

namespace StormRend.Units
{
	[Serializable]
	public struct DamageData
	{
		public Unit attacker;
		public int amount;

		public DamageData(Unit attacker, int amount)
		{
			this.attacker = attacker;
			this.amount = amount;
		}
	}
}

