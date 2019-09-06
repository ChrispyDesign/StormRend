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

	[RequireComponent(typeof(oGridImporter))]
	public class oMap : MonoBehaviour
	{
		[SerializeField] private oGrid m_grid;
		[SerializeField] private int m_nodeSize;
		[SerializeField] private Transform m_tilePrefab;

		private TileData m_gridData;

		void Start()
		{
			Transform parent = new GameObject("Tiles").transform;
			parent.parent = this.transform;
			oGridImporter import = GetComponent<oGridImporter>();
			m_grid = new oGrid(m_tilePrefab, m_nodeSize, parent, import.ImportGrid(import.m_path));
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