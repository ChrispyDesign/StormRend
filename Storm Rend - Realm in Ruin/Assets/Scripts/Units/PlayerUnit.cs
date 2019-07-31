using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    BERSERKER = 0,
    VALKYRIE,
    SAGE,

    COUNT
}

public class PlayerUnit : Unit
{
    [Header("Player Relevant Variables")]
    [SerializeField] private PlayerClass m_unitType = PlayerClass.BERSERKER;

    private MoveCommand movePlayer;
    private bool alreadyMoved;

    #region gettersAndSetters

    public PlayerClass GetUnitType() { return m_unitType; }
    public bool GetAlreadyMoved() { return alreadyMoved; }
    public MoveCommand GetMoveCommand() { return movePlayer; }

    public void SetMoveCommand(MoveCommand _move) { movePlayer = _move; }

    #endregion

    public override void OnSelect()
    {
        PlayerController.SetCurrentMode(PlayerMode.MOVE);
        m_isFocused = true;
        foreach (ICommand command in CommandManager.m_moves)
        {
            MoveCommand move = command as MoveCommand;

            if (move.m_unit == this)
            {
                Node previousNode = Grid.GetNodeFromCoords(move.GetOrigCoordinates());

                if (previousNode.GetUnitOnTop() != this && previousNode.GetUnitOnTop() != null)
                    return;

                move.Undo();
                alreadyMoved = true;
            }
        }

        SetDuplicateMeshVisibilty(true);

        Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(EnemyUnit));

        Unit player = PlayerController.GetCurrentPlayer();
        if (player != null && player != this)
        {
            if (player.GetAttackNodes() != null &&
                player.GetAttackNodes().Count > 0)
                player.UnShowAttackTiles();
        }

        PlayerController.SetCurrentPlayer(this);
        UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
        UIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(this);
       
        base.OnSelect();
    }

    public override void OnDeselect()
    {        
        base.OnDeselect();

        SetDuplicateMeshVisibilty(false);
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