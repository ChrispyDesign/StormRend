using StormRend;
using UnityEngine;

public enum RefreshType
{
    AttackAgain,
    MoveAgain
}

public class RefreshEffect : Effect
{
    [SerializeField] RefreshType m_refreshType;

    public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
    {
        base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		_thisUnit.SetHasMoved(false);
		_thisUnit.SetHasAttacked(false);
		_thisUnit.isLocked = false;

		return true;
	}
}