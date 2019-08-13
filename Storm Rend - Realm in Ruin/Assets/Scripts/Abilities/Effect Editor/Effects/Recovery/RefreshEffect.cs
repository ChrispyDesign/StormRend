using UnityEngine;

public enum RefreshType
{
    AttackAgain,
    MoveAgain
}

public class RefreshEffect : Effect
{
    [SerializeField] private RefreshType m_refreshType;

    public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		_thisUnit.SetAlreadyMoved(false);
		_thisUnit.SetAlreadyAttacked(false);
		_thisUnit.m_afterClear = false;

		return true;
	}
}