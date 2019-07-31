using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private int m_damageAmount;

    public override void PerformEffect(Node _effectedNode)
    {
        base.PerformEffect(_effectedNode);

        Unit unit = _effectedNode.GetUnitOnTop();

        if (unit != null)
        {
            unit.SetHP(unit.GetHP() - m_damageAmount);
        }
    }
}
