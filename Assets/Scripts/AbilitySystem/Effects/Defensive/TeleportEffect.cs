using StormRend.Defunct;

namespace StormRend.Abilities.Effects
{
    public class TeleportEffect : Effect
    {
        public override bool PerformEffect(oTile _effectedNode, Unit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            if (_effectedNode.GetUnitOnTop() != null
                || _effectedNode.m_nodeType == NodeType.BLOCKED
                || _effectedNode.m_nodeType == NodeType.EMPTY)
            {
                _thisUnit.SetHasAttacked(false);
                return false;
            }
            _thisUnit.MoveTo(_effectedNode);

            return true;
        }
    }
}