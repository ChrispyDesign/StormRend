using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    BERSERKER = 0,
    VALKYRIE,
    SAGE,

    COUNT
}

public class PlayerUnit : Unit
{
    [SerializeField] private UnitType m_unitType = UnitType.BERSERKER;
    [SerializeField] private Ability[] m_abilities;

    #region getters

    public UnitType GetUnitType() { return m_unitType; }
    public Ability[] GetAbilities() { return m_abilities; }

    #endregion

    public override void OnSelect()
    {
        Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(EnemyUnit));

        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
        UIManager.GetInstance().GetAbilityManager().SelectPlayerUnit(this);
        Player.SetCurrentPlayer(this);
        base.OnSelect();
    }

    public override void OnDeselect()
    {
        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(null);
        UIManager.GetInstance().GetAbilityManager().SelectPlayerUnit(null);
        base.OnDeselect();
    }

    public override void OnHover()
    {
        base.OnHover();
    }

    public override void OnUnhover()
    {
        base.OnUnhover();
    }
}
