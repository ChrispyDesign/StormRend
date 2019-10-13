using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Defunct
{
	public enum NodeType
	{
		WALKABLE,
		EMPTY,
		BLOCKED,
		PLAYER,
		ENEMY,
		NOT_ASSIGNED,

		COUNT
	}

	[RequireComponent(typeof(xGridImporter))]
	public class xMap : MonoBehaviour
	{
		[SerializeField] private xGrid m_grid;
		[SerializeField] private int m_nodeSize;
		[SerializeField] private Transform m_tilePrefab;

		private TileData m_gridData;

		void Start()
		{
			Transform parent = new GameObject("Tiles").transform;
			parent.parent = this.transform;
			xGridImporter import = GetComponent<xGridImporter>();
			m_grid = new xGrid(m_tilePrefab, m_nodeSize, parent, import.ImportGrid(import.m_path));
		}
	}

	[System.Serializable]
	public class TileData
	{
		[System.Serializable]
		public struct rowData
		{
			public NodeType[] row;
		}

		public rowData[] rows = new rowData[17];
	}
}