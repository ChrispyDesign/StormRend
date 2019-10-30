using System.Collections;
using System.Collections.Generic;
using StormRend;
using StormRend.Defunct;
using StormRend.Systems;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Defunct
{
    public enum PlayerClass
    {
        BERSERKER = 0,
        VALKYRIE,
        SAGE,

        COUNT
    }

    public class xPlayerUnit : xUnit
    {
        [Header("Player Relevant Variables")]
        [SerializeField] PlayerClass m_unitType = PlayerClass.BERSERKER;

        xMoveCommand movePlayer;

        #region gettersAndSetters
		public PlayerClass unitType => m_unitType;
        public xMoveCommand GetMoveCommand() { return movePlayer; }
        public void SetMoveCommand(xMoveCommand _move) { movePlayer = _move; }
        #endregion

        public override void OnSelect()
        {
            m_isSelected = true;

            xUnit player = xGameManager.current.GetPlayerController().GetCurrentPlayer();
            if (player != null && player != this)
            {
                if (player.GetAttackTiles() != null &&
                    player.GetAttackTiles().Count > 0)
                    player.UnShowAttackTiles();
            }

            xGameManager.current.GetPlayerController().SetCurrentPlayer(this);
            xUIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
            xUIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(this);

            if (m_hasMoved && m_hasAttacked)
                return;

            xGameManager.current.GetPlayerController().SetCurrentMode(SelectMode.Move);

            if (!m_afterClear)
            {
                foreach (xICommand command in xGameManager.current.GetCommandManager().commands)
                {
                    xMoveCommand move = command as xMoveCommand;

                    if (move.m_unit == this)
                    {
                        xTile previousNode = xGrid.CoordToTile(move.GetOrigCoordinates());

                        if (previousNode.GetUnitOnTop() != this && previousNode.GetUnitOnTop() != null)
                            return;

                        move.Undo();
                        m_hasMoved = true;
                    }
                }

                SetDuplicateMeshVisibilty(true);

                xDijkstra.Instance.FindValidMoves(GetTile(), GetMoveRange(), typeof(xEnemyUnit));
            }

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            base.OnDeselect();

            SetDuplicateMeshVisibilty(false);
        }
    }
}