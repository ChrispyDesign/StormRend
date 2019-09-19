using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Neighbour
{
    UP = 0,
    RIGHT,
    DOWN,
    LEFT
}

namespace StormRend
{
    public class Tile : MonoBehaviour, IHoverable, ISelectable
    {
        [SerializeField] private Unit m_unitOnTop;
        [SerializeField] private Tile[] m_neighbours;
        [SerializeField] private Vector3 m_position;
        [SerializeField] private Vector2Int m_coordinate;
		[SerializeField] public GameObject m_attackCover;
		[SerializeField] public GameObject m_moveCover;
		[SerializeField] public GameObject m_onHoverCover;
		[SerializeField] public GameObject m_onDeactivate;

        private Color m_origMaterial;

        public bool m_selected = false;

        public NodeType m_nodeType;
        public Tile m_parent;
        public int m_nGCost, m_nHCost;

        public int m_nFCost
        {
            get
            {
                return m_nGCost + m_nHCost;
            }
        }

        private void Update()
        {
            if (m_nodeType == NodeType.EMPTY && m_unitOnTop != null)
            {
                m_unitOnTop.Die();
            }
        }

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			float offsetY = 0;
			Handles.color = Color.white;
			Handles.BeginGUI();
			Handles.Label(transform.position + Vector3.up * offsetY, this.name, EditorStyles.whiteMiniLabel);
			Handles.EndGUI();
		}
#endif

        public Tile SetNodeVariables(Vector3 _pos, Vector2Int _coordinate, NodeType _nodeType)
        {
            m_neighbours = new Tile[4];
            m_position = _pos;
            m_coordinate = _coordinate;
            m_nodeType = _nodeType;
            return this;
        }

        #region GettersAndSetters

        public Unit GetUnitOnTop() { return m_unitOnTop; }
        public Vector2Int GetCoordinates() { return m_coordinate; }
        public Vector3 GetNodePosition() { return m_position; }

        public void SetUnitOnTop(Unit _unit) { m_unitOnTop = _unit; }
        public void SetNeighbours(Tile[] _neighbours) { m_neighbours = _neighbours; }

        #endregion

        public List<Tile> GetNeighbours()
        {
            List<Tile> neighbours = new List<Tile>();

            foreach (Tile node in m_neighbours)
            {
                if (node == null)
                    continue;

                neighbours.Add(node);
            }
            return neighbours;
        }

        public void OnHover()
        {
			if (m_nodeType == NodeType.WALKABLE && m_unitOnTop != null)
			{
				m_onHoverCover.SetActive(true);
			}
			PlayerUnit currentSelectedUnit = GameManager.singleton.GetPlayerController().GetCurrentPlayer();
			if (currentSelectedUnit && !m_unitOnTop && currentSelectedUnit.GetAvailableTiles().Contains(this))
			{
				currentSelectedUnit.MoveDuplicateTo(this);
			}
		}

        public void OnUnhover()
        {
			m_onHoverCover.SetActive(false);
		}

        public void OnSelect()
        {
			PlayerController controller = GameManager.singleton.GetPlayerController();
            PlayerUnit currentSelectedUnit = controller.GetCurrentPlayer();

			if (currentSelectedUnit == null)
                return;

            if (currentSelectedUnit.GetAttackTiles().Count > 0 && controller.GetCurrentMode() != PlayerMode.ATTACK)
                currentSelectedUnit.UnShowAttackTiles();

            if (controller.GetCurrentMode() == PlayerMode.MOVE)
            {
                if (currentSelectedUnit && currentSelectedUnit.GetIsSelected())
                {
                    List<Tile> nodes = currentSelectedUnit.GetAvailableTiles();

                    if (nodes.Contains(this) && !m_unitOnTop)
                    {
                        if (currentSelectedUnit.GetHasMoved() &&
                            currentSelectedUnit.GetMoveCommand() != null)
                        {
                            MoveCommand move = currentSelectedUnit.GetMoveCommand();
                            move.SetCoordinates(m_coordinate);
                            currentSelectedUnit.GetMoveCommand().Execute();
                        }
                        else
                        {
                            currentSelectedUnit.SetMoveCommand(new MoveCommand(
                                                                   currentSelectedUnit,
                                                                   m_coordinate));
                            MoveCommand temp = currentSelectedUnit.GetMoveCommand();
                            temp.Execute();

                            GameManager.singleton.GetCommandManager().m_moves.Add(temp);
                        }

                        FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 0.5f);
                    }
                    currentSelectedUnit.SetDuplicateMeshVisibilty(false);
                    currentSelectedUnit.SetIsSelected(false);

                    currentSelectedUnit.SetHasMoved(true);
                }
            }

            if (controller.GetCurrentMode() == PlayerMode.ATTACK)
            {
				PlayerUnit player = controller.GetCurrentPlayer();

				Ability ability = player.GetSelectedAbility();
				Animator anim = player.GetComponentInChildren<Animator>();
				if (ability != null && player.GetAttackTiles().Contains(this))
                {
					ability.AddToList(this);

					if (ability.GetTilesToSelect() > ability.GetTiles().Count)
					{
						this.m_selected = false;
						this.m_attackCover.SetActive(false);
						return;
					}

					bool continueAbility = true;
					foreach (Effect effect in ability.GetEffects())
					{
						if (continueAbility)
						{
							continueAbility = effect.PerformEffect(ability.GetTiles(), currentSelectedUnit);
							if (anim != null)
								anim.SetInteger("AttackAnim", ability.GetAnimNumber());
						}
					}

					//TODO Temporary Fix
					if (continueAbility)
					{
						currentSelectedUnit.SetHasMoved(true);
						currentSelectedUnit.SetHasAttacked(true);
					}

					currentSelectedUnit.SetSelectedAbility(null);

					CommandManager commandManager = GameManager.singleton.GetCommandManager();

					foreach (MoveCommand move in commandManager.m_moves)
					{
						Unit unit = move.m_unit;
						unit.isChained = true;
					}

					commandManager.m_moves.Clear();
					UIAbilitySelector abilitySelector = UIManager.GetInstance().GetAbilitySelector();
					abilitySelector.GetInfoPanel().SetActive(false);
					abilitySelector.GetButtonPanel().SetActive(false);
				}
            }

            if (currentSelectedUnit.GetHasAttacked())
            {
                UIAbilitySelector selector = UIManager.GetInstance().GetAbilitySelector();
                selector.SelectPlayerUnit(null);
                selector.GetInfoPanel().SetActive(false);
				currentSelectedUnit.UnShowAttackTiles();
            }
			controller.SetCurrentMode(PlayerMode.IDLE);
		}

        public void OnDeselect()
        {
			m_selected = false;

			Unit unitOnTop = this.GetUnitOnTop();
			if (unitOnTop)
			{
				PlayerController controller = GameManager.singleton.GetPlayerController();
				Ability ability = controller.GetCurrentPlayer()?.GetSelectedAbility();
				List<Tile> nodes = new List<Tile>();

				if (controller?.GetPrevMode() == PlayerMode.ATTACK && ability?.GetTiles().Count == ability?.GetTilesToSelect())
				{
					nodes = unitOnTop?.GetAttackTiles();
				}
				else if (controller?.GetPrevMode() == PlayerMode.MOVE)
				{
					nodes = unitOnTop?.GetAvailableTiles();
				}

				if (nodes != null)
				{
					foreach (Tile node in nodes)
					{
						if (node.m_nodeType == NodeType.EMPTY)
							continue;

						node.m_attackCover.SetActive(false);
						node.m_moveCover.SetActive(false);
						node.m_selected = false;
					}
				}
			}

		}
    }
}