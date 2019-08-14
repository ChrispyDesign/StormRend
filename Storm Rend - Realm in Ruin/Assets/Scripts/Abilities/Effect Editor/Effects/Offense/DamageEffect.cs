using StormRend;
using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private int m_damageAmount;

    public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
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
