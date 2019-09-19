using StormRend;
using System.Collections.Generic;
using UnityEngine;


public class SwapUnitEffect : Effect
{
	public bool removeCurses;
	public bool restoreHealth;

	public int restoreAmount;

	public override bool PerformEffect(List<Tile> targetTile, Unit effectPerformer)
	{
		Unit unit1 = targetTile[0].GetUnitOnTop();
		Unit unit2 = targetTile[1].GetUnitOnTop();

		if (removeCurses)
		{
			unit1.isBlind = false;
			unit1.isCrippled = false;

			unit2.isBlind = false;
			unit2.isCrippled = false;
		}

		if (restoreHealth)
		{
			if (unit1 != null)
				unit1.TakeDamage(-restoreAmount);

			if (unit2 != null)
				unit2.TakeDamage(-restoreAmount);
		}

		if(unit1 != null)
			unit1.MoveTo(targetTile[1]);

		if (unit2 != null)
			unit2.MoveTo(targetTile[0]);

		return true;
	}
}
