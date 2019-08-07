using UnityEngine;

public class HealEffect : Effect
{
    [SerializeField] private int m_healAmount;

    public override void PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);

        Unit unit = _effectedNode.GetUnitOnTop();

        if (unit != null)
        {
            unit.SetHP(unit.GetHP() + m_healAmount);
        }
    }
}