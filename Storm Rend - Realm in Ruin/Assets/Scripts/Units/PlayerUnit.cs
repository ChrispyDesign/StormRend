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

    #region getters

    public UnitType GetUnitType() { return m_unitType; }

    #endregion

    public override void OnSelect()
    {
        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
        Player.SetCurrentPlayer(this);
        base.OnSelect();
    }

    public override void OnDeselect()
    {
        Grid.GetNodeFromCoords(m_coordinates).OnDeselect();
        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(null);
        Player.SetCurrentPlayer(null);
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
