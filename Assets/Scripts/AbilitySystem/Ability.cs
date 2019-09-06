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
	[CreateAssetMenu(fileName = "New Ability", menuName = "StormRend/Ability")]
	public class Ability : ScriptableObject
	{
		private List<oTile> m_tiles = new List<oTile>();

		// ability info (for UI display purposes)
		[SerializeField] string m_name;
		[SerializeField] int animNumber;
		[SerializeField] Sprite m_icon = null;
		[TextArea]
		[SerializeField] string m_description;

		[SerializeField] int m_gloryRequirement = 0;
		[SerializeField] int m_tilesToSelect = 1;

		public RowData[] m_castArea = new RowData[7];
		[SerializeField] TargetableTiles m_targetableTiles;
		[SerializeField] List<Effect> m_effects = new List<Effect>();

		#region getters
		//GET RID OF GETTERS AND SETTERS!!!
		public List<oTile> GetTiles() { return m_tiles; }
		public string GetName() { return m_name; }

		public int GetAnimNumber() { return animNumber; }
		public Sprite GetIcon() { return m_icon; }
		public string GetDescription() { return m_description; }

		/// <summary> the amount of glory required to perform this ability </summary>
		public int GetGloryRequirement() { return m_gloryRequirement; }

		/// <summary> the amount of tiles to select before this ability is cast </summary>
		public int GetTilesToSelect() { return m_tilesToSelect; }
		public TargetableTiles GetTargetableTiles() { return m_targetableTiles; }
		public List<Effect> GetEffects() { return m_effects; }

		#endregion

		public void AddToList(oTile _tile) { m_tiles.Add(_tile); }

		public void GetSelectableTiles(ref Unit _player)
		{
			int center = (m_castArea.Length / 2) + (m_castArea.Length % 2);
			int endPoint = m_castArea.Length / 2;
			List<oTile> nodes = new List<oTile>();
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

						oTile node = oGrid.CoordToTile(coords);
						if (node != null)
							nodes.Add(node);
					}
				}
			}

			_player.SetAttackNodes(nodes);
		}
	}
}