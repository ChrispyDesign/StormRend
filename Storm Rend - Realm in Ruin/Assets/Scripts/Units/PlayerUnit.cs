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

    private MoveCommand movePlayer;
    private bool alreadyMoved;

    #region gettersAndSetters

    public UnitType GetUnitType() { return m_unitType; }
    public Ability[] GetAbilities() { return m_abilities; }
    public bool GetAlreadyMoved() { return alreadyMoved; }
    public MoveCommand GetMoveCommand() { return movePlayer; }

    public void SetMoveCommand(MoveCommand _move) { movePlayer = _move; }

    #endregion

    public override void OnSelect()
    {
        foreach (ICommand command in CommandManager.m_moves)
        {
            MoveCommand move = command as MoveCommand;
            
            if (move.m_unit == this)
            {
                if (Grid.GetNodeFromCoords(move.GetOrigCoordinates()).GetUnitOnTop() != null)
                    return;

                move.Undo();
                alreadyMoved = true;
            }
        }

        Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(EnemyUnit));

        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
        UIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(this);
        Player.SetCurrentPlayer(this);
        base.OnSelect();
    }

    public override void OnDeselect()
    {
        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(null);
        UIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(null);
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