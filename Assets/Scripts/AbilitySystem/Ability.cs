using System;
using System.Collections.Generic;
using StormRend.Systems.Mapping;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities
{
	[Serializable, CreateAssetMenu(menuName = "StormRend/Ability", fileName = "Ability")]
	public class Ability : ScriptableObject
	{
		//Constants
		const int seven = 7;	//Cast Area Size Squared

		//Flags and Enums
		[Flags]
		public enum TargetableTileCategory
		{
			Empty = 1 << 0,
			Self = 1 << 1,
			Allies = 1 << 2,
			Enemies = 1 << 3,
		}

		//Inspector
		[SerializeField] Sprite _icon = null;
		[Tooltip("Animation number for this ability in order to send to a corresponding animator")]
		[SerializeField] int _animNumber;
		[TextArea, SerializeField] string _description = "";

		[Header("Casting"), Space(1), Tooltip("Glory cost required to perform this ability")]
		[SerializeField] int _gloryCost = 1;

		[Tooltip("The required number of selected tiles this ability needs in order for it to be performed")]
		[SerializeField] int requiredTiles = 1;

		[Tooltip("The category of tiles this ability can target")]
		[SerializeField, EnumFlags, Space(5)] TargetableTileCategory _targetableTileCategories;

		//Members
		public bool[,] castArea = new bool[seven, seven];
		[HideInInspector] public List<Effect> effects = new List<Effect>();

		//Properties
		public int animNumber => _animNumber;
		public Sprite icon => _icon;
		public string description => _description;
		public int gloryCost => _gloryCost;
		public TargetableTileCategory targetableTileCategories => _targetableTileCategories;

		//Core
		public void Perform(Unit owner, Tile[] targets)
		{
			foreach (var e in effects)
				e.Perform(owner, targets);
		}

		//TRANSFERRED FROM OLD
		public void GetSelectableTiles(ref Unit _player)
		{
			//Q. WTF is this doing?
			//A. I think this populates the passed in ally unit's
			//tiles that this ability can be applied to

			int center = (castArea.GetLength(0) / 2) + (castArea.GetLength(0) % 2);
			int endPoint = castArea.GetLength(0) / 2;

			// int center = (castArea.Length / 2) + (castArea.Length % 2);
			// int endPoint = castArea.Length / 2;

			List<Tile> tiles = new List<Tile>();
			Vector2Int coords = new Vector2Int();

			// List<Tile> nodes = new List<Tile>();
			// Vector2Int coords = Vector2Int.zero;


			// for (int x = 0; x < castArea.GetLength(0); x++)
			// {
			// 	for (int y = 0; y < castArea.GetLength(1); y++)
			// 	{
			// 		if (castArea[x, y] == true)
			// 		{
			// 			int _x = -endPoint + x;
			// 			int _y = endPoint - y;

			// 			coords.x = _player.coords.x + _x;
			// 			coords.y = _player.coords.y + _y;

			// 			Tile tile =
			// 			if (tile != null)
			// 				tiles.Add(tile);

			// 			xGrid.CoordToTile(coords);
			// 			if (node != null)
			// 				nodes.Add(node);
			// 		}
			// 	}
			// }

			// _player.SetAttackNodes(nodes);
		}
	}
}