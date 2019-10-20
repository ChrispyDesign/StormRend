using StormRend;
using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] int m_damageAmount;

    public override bool PerformEffect(Tile targetTile, Unit effectPerformer)
    {
        base.PerformEffect(targetTile, effectPerformer);

		if (!m_isTileAllowed)
			return false;

		Unit unit = targetTile.GetUnitOnTop();

		//If an opponent unit is found
        if (unit)
		{
            unit.TakeDamage(m_damageAmount);

			//Handle provoke
			if (unit.isProvoking)
				effectPerformer.TakeDamage(unit.provokeDamage);
		}
		return true;
	}
}
