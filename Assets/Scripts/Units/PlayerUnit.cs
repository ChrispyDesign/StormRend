using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend
{
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
        [SerializeField] PlayerClass m_unitType = PlayerClass.BERSERKER;

        MoveCommand movePlayer;

        #region gettersAndSetters
		public PlayerClass unitType => m_unitType;
        public MoveCommand GetMoveCommand() { return movePlayer; }
        public void SetMoveCommand(MoveCommand _move) { movePlayer = _move; }
        #endregion

        public override void OnSelect()
        {
            m_isFocused = true;

            Unit player = GameManager.GetInstance().GetPlayerController().GetCurrentPlayer();
            if (player != null && player != this)
            {
                if (player.GetAttackNodes() != null &&
                    player.GetAttackNodes().Count > 0)
                    player.UnShowAttackTiles();
            }

            GameManager.GetInstance().GetPlayerController().SetCurrentPlayer(this);
            UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
            UIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(this);

            if (m_alreadyMoved && m_alreadyAttacked)
                return;

            GameManager.GetInstance().GetPlayerController().SetCurrentMode(PlayerMode.MOVE);

            if (!m_afterClear)
            {
                foreach (ICommand command in GameManager.GetInstance().GetCommandManager().m_moves)
                {
                    MoveCommand move = command as MoveCommand;

                    if (move.m_unit == this)
                    {
                        Node previousNode = Grid.GetNodeFromCoords(move.GetOrigCoordinates());

                        if (previousNode.GetUnitOnTop() != this && previousNode.GetUnitOnTop() != null)
                            return;

                        move.Undo();
                        m_alreadyMoved = true;
                    }
                }

                SetDuplicateMeshVisibilty(true);

                Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(EnemyUnit));
            }

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            base.OnDeselect();

            SetDuplicateMeshVisibilty(false);
        }


		//Could be better implemented
        public override void Die()
        {
            base.Die();
            GameManager.GetInstance().m_playerCount--;
            GameManager.GetInstance().CheckEndCondition();
        }
    }
}