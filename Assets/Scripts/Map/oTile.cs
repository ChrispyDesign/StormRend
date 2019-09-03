using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Abilities.Effects;
using StormRend.UI;
using UnityEditor;
using UnityEngine;

namespace StormRend.Defunct
{
	public enum Neighbour
	{
		UP = 0,
		RIGHT,
		DOWN,
		LEFT
	}

	public class oTile : MonoBehaviour, IHoverable, ISelectable
	{
		[SerializeField] private Unit m_unitOnTop;      //Dumb
		[SerializeField] private oTile[] m_neighbours;
		[SerializeField] private Vector3 m_position;       //MB already has a Transform
		[SerializeField] private Vector2Int m_coordinate;
		[SerializeField] public GameObject m_attackCover;
		[SerializeField] public GameObject m_moveCover;
		[SerializeField] public GameObject m_onHoverCover;
		[SerializeField] public GameObject m_onDeactivate;

		private Color m_origMaterial;

		public bool m_selected = false;

		public NodeType m_nodeType;
		public oTile m_parent;
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
			Handles.color = Color.white;
			Handles.BeginGUI();
			Handles.Label(transform.position, this.name, EditorStyles.whiteLargeLabel);
			Handles.EndGUI();
		}
#endif

		public oTile SetNodeVariables(Vector3 _pos, Vector2Int _coordinate, NodeType _nodeType)
		{
			m_neighbours = new oTile[4];
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
		public void SetNeighbours(oTile[] _neighbours) { m_neighbours = _neighbours; }

		#endregion

		public List<oTile> GetNeighbours()
		{
			List<oTile> neighbours = new List<oTile>();

			foreach (oTile node in m_neighbours)
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

			//m_origMaterial = transform.GetComponent<MeshRenderer>().material.color;
			//transform.GetComponent<MeshRenderer>().material.color = Color.red;

			//PlayerUnit currentSelectedUnit = GameManager.singleton.GetPlayerController().GetCurrentPlayer();
			//if (currentSelectedUnit && !m_unitOnTop && currentSelectedUnit.GetAvailableTiles().Contains(this) && !currentSelectedUnit.GetHasMoved() && !currentSelectedUnit.GetHasAttacked())
			//{
			//    currentSelectedUnit.MoveDuplicateTo(this);
			//    m_onHoverCover.SetActive(true);
			//    m_moveCover.SetActive(false);
			//}
		}

		public void OnUnhover()
		{
			m_onHoverCover.SetActive(false);

			//if (!m_selected)
			//    transform.GetComponent<MeshRenderer>().material.color = Color.white;

			//transform.GetComponent<MeshRenderer>().material.color = m_origMaterial;

			//if (currentSelectedUnit && !m_unitOnTop && currentSelectedUnit.GetAvailableTiles().Contains(this) && !currentSelectedUnit.GetHasMoved())
			//{
			//    m_onHoverCover.SetActive(false);
			//    m_moveCover.SetActive(true);
			//}
			//else
			//{
			//    m_onHoverCover.SetActive(false);
			//    m_moveCover.SetActive(false);
			//}

		}

		public void OnSelect()
		{
			PlayerUnit currentSelectedUnit = GameManager.singleton.GetPlayerController().GetCurrentPlayer();

			if (currentSelectedUnit == null)
				return;

			if (currentSelectedUnit.GetAttackTiles().Count > 0)
				currentSelectedUnit.UnShowAttackTiles();

			if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE)
			{
				if (currentSelectedUnit && currentSelectedUnit.GetIsSelected())
				{
					List<oTile> nodes = currentSelectedUnit.GetAvailableTiles();

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

			if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
			{
				PlayerUnit player = GameManager.singleton.GetPlayerController().GetCurrentPlayer();

				Ability ability = player.GetSelectedAbility();
				Animator anim = player.GetComponentInChildren<Animator>();
				if (ability != null && player.GetAttackTiles().Contains(this))
				{
					bool continueAbility = true;
					foreach (Effect effect in ability.GetEffects())
					{
						if (continueAbility)
						{
							continueAbility = effect.PerformEffect(this, currentSelectedUnit);
							if (anim != null)
								anim.SetInteger("AttackAnim", ability.GetAnimNumber());
						}
					}
					currentSelectedUnit.SetSelectedAbility(null);

					CommandManager commandManager = GameManager.singleton.GetCommandManager();

					//UndoController.
					foreach (MoveCommand move in commandManager.m_moves)
					{
						Unit unit = move.m_unit;
						unit.m_afterClear = true;
					}

					commandManager.m_moves.Clear();     //UndoController.Clear()
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
			}
			GameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.IDLE);
		}

		public void OnDeselect()
		{
			m_selected = false;

			Unit unitOnTop = this.GetUnitOnTop();
			if (unitOnTop)
			{
				List<oTile> nodes = unitOnTop.GetAvailableTiles();

				if (nodes != null)
				{
					foreach (oTile node in nodes)
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