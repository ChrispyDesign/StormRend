using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Berserker,
    Valkyrie,
    Sage
}

public class PlayerUnit : Unit
{
    [SerializeField] private UnitType m_unitType;

    #region getters

    public UnitType GetUnitType() { return m_unitType; }

    #endregion

    public override void OnSelect()
    {
        base.OnSelect();

        UIManager.GetInstance().SelectPlayerUnit(this);
    }

    public override void OnDeselect()
    {
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
