using System.Collections;
using System.Collections.Generic;
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
            m_origMaterial = transform.GetComponent<MeshRenderer>().material.color;
            transform.GetComponent<MeshRenderer>().material.color = Color.red;

            PlayerUnit currentSelectedUnit = GameManager.singleton.GetPlayerController().GetCurrentPlayer();
            if (currentSelectedUnit && !m_unitOnTop && currentSelectedUnit.GetAvailableNodes().Contains(this))
            {
                currentSelectedUnit.MoveDuplicateTo(this);
            }
        }

        public void OnUnhover()
        {
            if (!m_selected)
                transform.GetComponent<MeshRenderer>().material.color = Color.white;

            transform.GetComponent<MeshRenderer>().material.color = m_origMaterial;
        }

        public void OnSelect()
        {
            PlayerUnit currentSelectedUnit = GameManager.singleton.GetPlayerController().GetCurrentPlayer();

            if (currentSelectedUnit == null)
                return;

            if (currentSelectedUnit.GetAttackNodes().Count > 0)
                currentSelectedUnit.UnShowAttackTiles();

            if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE)
            {
                if (currentSelectedUnit && currentSelectedUnit.GetIsFocused())
                {
                    List<Tile> nodes = currentSelectedUnit.GetAvailableNodes();

                    if (nodes.Contains(this) && !m_unitOnTop)
                    {
                        if (currentSelectedUnit.GetAlreadyMoved() &&
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
                    currentSelectedUnit.SetIsFocused(false);

                    currentSelectedUnit.SetAlreadyMoved(true);
                }
            }

            if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
            {
                Ability ability = currentSelectedUnit.GetLockedAbility();
                if (ability != null)
                {
					bool continueAbility = true;
					foreach (Effect effect in ability.GetEffects())
					{
						if (continueAbility)
							continueAbility = effect.PerformEffect(this, currentSelectedUnit);
					}
					currentSelectedUnit.SetLockedAbility(null);

					CommandManager commandManager = GameManager.singleton.GetCommandManager();

					foreach (MoveCommand move in commandManager.m_moves)
					{
						Unit unit = move.m_unit;
						unit.m_afterClear = true;
					}

					commandManager.m_moves.Clear();
					UIAbilitySelector abilitySelector = UIManager.GetInstance().GetAbilitySelector();
					abilitySelector.GetInfoPanel().SetActive(false);
					abilitySelector.GetButtonPanel().SetActive(false);					
                }
            }

            if (currentSelectedUnit.GetAlreadyAttacked())
            {
                UIAbilitySelector selector = UIManager.GetInstance().GetAbilitySelector();
                selector.SelectPlayerUnit(null);
                selector.GetInfoPanel().SetActive(false);
            }
            GameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.IDLE);
        }

        public void OnDeselect()
        {
            m_selected = false;

            Unit unitOnTop = GetUnitOnTop();
            if (unitOnTop)
            {
                List<Tile> nodes = unitOnTop.GetAvailableNodes();

                if (nodes != null)
                {
                    foreach (Tile node in nodes)
					{
						if (node.m_nodeType == NodeType.EMPTY)
							continue;

						//node.transform.GetComponent<MeshRenderer>().material.color = Color.white;
						node.m_attackCover.SetActive(false);
						node.m_moveCover.SetActive(false);
                        node.m_selected = false;
                    }
                }
            }

        }
    }
}