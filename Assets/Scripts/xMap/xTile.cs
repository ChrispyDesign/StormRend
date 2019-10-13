﻿using System.Collections.Generic;
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

	public class xTile : MonoBehaviour, xIHoverable, xISelectable
	{
		[SerializeField] private xUnit m_unitOnTop;      //Dumb
		[SerializeField] private xTile[] m_neighbours;
		[SerializeField] private Vector3 m_position;       //MB already has a Transform
		[SerializeField] private Vector2Int m_coordinate;
		[SerializeField] public GameObject m_attackCover;
		[SerializeField] public GameObject m_moveCover;
		[SerializeField] public GameObject m_onHoverCover;
		[SerializeField] public GameObject m_onDeactivate;

		private Color m_origMaterial;

		public bool m_selected = false;

		public NodeType m_nodeType;
		public xTile m_parent;
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

		public xTile SetNodeVariables(Vector3 _pos, Vector2Int _coordinate, NodeType _nodeType)
		{
			m_neighbours = new xTile[4];
			m_position = _pos;
			m_coordinate = _coordinate;
			m_nodeType = _nodeType;
			return this;
		}

		#region GettersAndSetters

		public xUnit GetUnitOnTop() { return m_unitOnTop; }
		public Vector2Int GetCoordinates() { return m_coordinate; }
		public Vector3 GetNodePosition() { return m_position; }

		public void SetUnitOnTop(xUnit _unit) { m_unitOnTop = _unit; }
		public void SetNeighbours(xTile[] _neighbours) { m_neighbours = _neighbours; }

		#endregion

		public List<xTile> GetNeighbours()
		{
			List<xTile> neighbours = new List<xTile>();

			foreach (xTile node in m_neighbours)
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
			xPlayerUnit currentSelectedUnit = xGameManager.singleton.GetPlayerController().GetCurrentPlayer();
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
			xPlayerUnit currentSelectedUnit = xGameManager.singleton.GetPlayerController().GetCurrentPlayer();

			if (currentSelectedUnit == null)
				return;

			if (currentSelectedUnit.GetAttackTiles().Count > 0)
				currentSelectedUnit.UnShowAttackTiles();

			if (xGameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE)
			{
				if (currentSelectedUnit && currentSelectedUnit.GetIsSelected())
				{
					List<xTile> nodes = currentSelectedUnit.GetAvailableTiles();

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

							xGameManager.singleton.GetCommandManager().commands.Add(temp);
						}

						FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 0.5f);
					}
					currentSelectedUnit.SetDuplicateMeshVisibilty(false);
					currentSelectedUnit.SetIsSelected(false);

					currentSelectedUnit.SetHasMoved(true);
				}
			}

			if (xGameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
			{
				xPlayerUnit player = xGameManager.singleton.GetPlayerController().GetCurrentPlayer();

				xAbility ability = player.GetSelectedAbility();
				Animator anim = player.GetComponentInChildren<Animator>();
				if (ability != null && player.GetAttackTiles().Contains(this))
				{
					bool continueAbility = true;
					foreach (xEffect effect in ability.GetEffects())
					{
						if (continueAbility)
						{
							continueAbility = effect.PerformEffect(this, currentSelectedUnit);
							if (anim != null)
								anim.SetInteger("AttackAnim", ability.GetAnimNumber());
						}
					}
					currentSelectedUnit.SetSelectedAbility(null);

					UndoSystem commandManager = xGameManager.singleton.GetCommandManager();

					//UndoController.
					foreach (MoveCommand move in commandManager.commands)
					{
						xUnit unit = move.m_unit;
						unit.m_afterClear = true;
					}

					commandManager.ClearCommands();     //UndoSystem.Clear()
					xUIAbilitySelector abilitySelector = xUIManager.GetInstance().GetAbilitySelector();
					abilitySelector.GetInfoPanel().SetActive(false);
					abilitySelector.GetButtonPanel().SetActive(false);
				}
			}

			if (currentSelectedUnit.GetHasAttacked())
			{
				xUIAbilitySelector selector = xUIManager.GetInstance().GetAbilitySelector();
				selector.SelectPlayerUnit(null);
				selector.GetInfoPanel().SetActive(false);
			}
			xGameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.IDLE);
		}

		public void OnDeselect()
		{
			m_selected = false;

			xUnit unitOnTop = this.GetUnitOnTop();
			if (unitOnTop)
			{
				List<xTile> nodes = unitOnTop.GetAvailableTiles();

				if (nodes != null)
				{
					foreach (xTile node in nodes)
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