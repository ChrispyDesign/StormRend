using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private int m_damageAmount;

    public override void PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return;

		Unit unit = _effectedNode.GetUnitOnTop();

        if (unit != null)
        {
            unit.TakeDamage(m_damageAmount);
        }
	}
}
