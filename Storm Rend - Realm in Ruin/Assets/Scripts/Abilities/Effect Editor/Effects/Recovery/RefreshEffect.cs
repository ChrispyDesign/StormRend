using UnityEngine;

public enum RefreshType
{
    AttackAgain,
    MoveAgain
}

public class RefreshEffect : Effect
{
    [SerializeField] private RefreshType m_refreshType;

    public override void PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);
        
        if(m_refreshType == RefreshType.AttackAgain)
            _thisUnit.SetAlreadyMoved(false);

        if(m_refreshType == RefreshType.MoveAgain)
            _thisUnit.SetAlreadyAttacked(false);
    }
}