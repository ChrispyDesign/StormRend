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
            isSelected = true;

            Unit player = GameManager.singleton.GetPlayerController().GetCurrentPlayer();
            if (player != null && player != this)
            {
                if (player.GetAttackTiles() != null &&
                    player.GetAttackTiles().Count > 0)
                    player.UnShowAttackTiles();
            }

            GameManager.singleton.GetPlayerController().SetCurrentPlayer(this);
           // UIManager.GetInstance().GetAvatarSelector().SelectPlayerUnit(this);
            UIManager.GetInstance().GetAbilitySelector().SelectPlayerUnit(this);

            if (hasMoved && hasAttacked)
                return;

            GameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.MOVE);

            if (!isLocked) 
            {
                foreach (ICommand command in GameManager.singleton.GetCommandManager().m_moves)
                {
                    MoveCommand move = command as MoveCommand;

                    if (move.m_unit == this)
                    {
                        Tile previousNode = Grid.CoordToTile(move.GetOrigCoordinates());

                        if (previousNode.GetUnitOnTop() != this && previousNode.GetUnitOnTop() != null)
                            return;

                        move.Undo();
                        hasMoved = true;
                    }
                }

                SetDuplicateMeshVisibilty(true);

                Dijkstra.Instance.GetValidMoves(GetTile(), GetMoveRange(), typeof(EnemyUnit));
            }

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            base.OnDeselect();

            SetDuplicateMeshVisibilty(false);
        }

		public void CheckSoulCommune(Unit _deadUnit)
		{
			Dijkstra dijkstra = Dijkstra.Instance;
			dijkstra.GetValidMoves(Grid.CoordToTile(coords), GetMoveRange(), typeof(EnemyUnit));

			foreach(Tile tile in dijkstra.validMoves)
			{
				if(tile.GetUnitOnTop() != null && tile.GetUnitOnTop() == _deadUnit)
				{
					UIManager.GetInstance().GetGloryManager().GainGlory(1);

				}
			}
		}
    }
}