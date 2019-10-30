using System.Collections.Generic;
using StormRend.Abilities.Effects;
using UnityEngine;
using StormRend.Defunct;

namespace StormRend.Abilities
{
	[System.Serializable]
	public struct TargetableTiles
	{
		public bool m_empty;
		public bool m_enemies;
		public bool m_players;
		public bool m_self;
	}

	[System.Serializable]
	public class RowData
	{
		public bool[] elements = new bool[7];
	}

	[System.Serializable]
	[CreateAssetMenu(fileName = "xAbility", menuName = "StormRend/xAbility")]
	public class xAbility : ScriptableObject
	{
		private List<xTile> m_tiles = new List<xTile>();

		// ability info (for UI display purposes)
		[SerializeField] int animNumber;
		[SerializeField] Sprite m_icon = null;
		[TextArea]
		[SerializeField] string m_description;

		[SerializeField] int m_gloryRequirement = 0;
		[SerializeField] int m_tilesToSelect = 1;

		public RowData[] m_castArea = new RowData[7];
		[SerializeField] TargetableTiles m_targetableTiles;
		[SerializeField] List<xEffect> m_effects = new List<xEffect>();

		#region getters
		//GET RID OF GETTERS AND SETTERS!!!
		public int GetAnimNumber() { return animNumber; }
		public Sprite GetIcon() { return m_icon; }
		public string GetDescription() { return m_description; }

		/// <summary> the amount of glory required to perform this ability </summary>
		public int GetGloryRequirement() { return m_gloryRequirement; }

		/// <summary> the amount of tiles to select before this ability is cast </summary>
		// public int GetTilesToSelect() { return m_tilesToSelect; }
		public TargetableTiles GetTargetableTiles() { return m_targetableTiles; }
		public List<xEffect> GetEffects() { return m_effects; }

		#endregion

		// public void AddToList(xTile _tile) { m_tiles.Add(_tile); }

		public void GetSelectableTiles(ref xUnit _player)
		{
			//Q. WTF is this doing?
			//A. I think this populates the passed in ally unit's
			//tiles that this ability can be applied to

			int center = (m_castArea.Length / 2) + (m_castArea.Length % 2);
			int endPoint = m_castArea.Length / 2;
			List<xTile> nodes = new List<xTile>();
			Vector2Int coords = Vector2Int.zero;

			for (int y = 0; y < m_castArea.Length; y++)
			{
				for (int x = 0; x < m_castArea[y].elements.Length; x++)
				{
					if (m_castArea[y].elements[x])
					{
						int _x = -endPoint + x;
						int _y = endPoint - y;

						coords.x = _player.coords.x + _x;
						coords.y = _player.coords.y + _y;

						xTile node = xGrid.CoordToTile(coords);
						if (node != null)
							nodes.Add(node);
					}
				}
			}

			_player.SetAttackNodes(nodes);
		}
	}
}