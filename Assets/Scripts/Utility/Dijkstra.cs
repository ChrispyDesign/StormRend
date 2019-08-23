using System;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class Dijkstra : MonoBehaviour
	{
		//Make this A* because why not

		public static Dijkstra Instance; //bingleton
		public List<Tile> m_validMoves = new List<Tile>(); // The path we will calculated
		public List<Tile> m_checkedNodes = new List<Tile>();
		public bool m_DebugPath; // If we want to debug out path

		private void Awake()
		{
			// Initialising the instance if its not yet been initialised
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
		}

		public void FindValidMoves(Tile _startNode, int allowedTiles, System.Type blockedUnits)
		{
			//This could just return the valid moves directly
			// If there is already a path clear it
			if (m_validMoves.Count > 0)
				m_validMoves.Clear();

			// Creating a binary tree for paths that can be taken from the currentnode
			Queue<Tile> queue = new Queue<Tile>();

			// Add our starting point on the openlist
			queue.Enqueue(_startNode);

			while (queue.Count > 0)
			{
				// Initialise the current node to openlist's first node and remove it from the openlist
				// add it to the closedlist cuz its being searched
				Tile currentNode = queue.Dequeue();

				if (!m_checkedNodes.Contains(currentNode))
					m_checkedNodes.Add(currentNode);

				List<Tile> Neighbours = currentNode.GetNeighbours();

				// We search thru the neighbours until we find the best travel cost
				// to another node
				foreach (Tile neighbour in Neighbours)
				{
					if (neighbour.m_nodeType == NodeType.BLOCKED || neighbour.m_nodeType == NodeType.EMPTY)
						continue;

					Unit neighbourOnTop = neighbour.GetUnitOnTop();

					if (neighbourOnTop)
						if (neighbourOnTop.GetType() == blockedUnits)
							continue;

					if (!m_checkedNodes.Contains(neighbour))
						m_checkedNodes.Add(neighbour);

					int newMovementCostToNeighbour = currentNode.m_nGCost + 1;
					if (newMovementCostToNeighbour < neighbour.m_nGCost || !queue.Contains(neighbour))
					{
						neighbour.m_nGCost = newMovementCostToNeighbour;
						neighbour.m_nHCost = 1;
						neighbour.m_parent = currentNode;

						if (neighbour.m_nGCost <= allowedTiles)
							queue.Enqueue(neighbour);
					}
				}

				if (currentNode.m_nGCost > 0 && currentNode.m_nGCost <= allowedTiles && !m_validMoves.Contains(currentNode))
					m_validMoves.Add(currentNode);

			}

			foreach (Tile node in m_checkedNodes)
			{
				node.m_nHCost = 0;
				node.m_nGCost = 0;
			}
		}
	}
}