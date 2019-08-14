using StormRend;
using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] int m_damageAmount;

    public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		Unit unit = _effectedNode.GetUnitOnTop();

        if (unit != null)
        {
            unit.TakeDamage(m_damageAmount);
		}

		return true;
	}
}
