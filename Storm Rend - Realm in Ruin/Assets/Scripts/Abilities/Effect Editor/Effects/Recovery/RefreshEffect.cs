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

		if (!m_isTileAllowed)
			return;

		_thisUnit.SetAlreadyMoved(false);
		_thisUnit.SetAlreadyAttacked(false);
	}
}