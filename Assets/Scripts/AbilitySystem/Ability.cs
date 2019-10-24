using System;
using System.Collections.Generic;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Abilities
{
	public enum AbilityType
	{
		Primary,
		Secondary
	}

	[Serializable, CreateAssetMenu(menuName = "StormRend/Ability", fileName = "Ability")]
	public class Ability : ScriptableObject
	{
		//Constants
		public const int kCastAreaSqrLen = 7;    //Cast Area Size Squared. Should this be some kind of global?

		//Flags and Enums
		[Flags]
		public enum TargetTileMask
		{
			// Empty = 1 << 0,		//Empty is when no bits are selected!
			Self = 1 << 0,
			Allies = 1 << 1,
			Enemies = 1 << 2,
			InAnimates = 1 << 3,	//Such as crystals
		}

		//Inspector
		[SerializeField] Sprite _icon = null;
		[Tooltip("Animation number for this ability in order to send to a corresponding animator")]
		[SerializeField] int _animNumber = 0;
		[SerializeField] int _level = 1;
		[SerializeField] AbilityType _type = AbilityType.Primary;
		[TextArea(0, 2), SerializeField] string _description = "";

		[Header("Casting"), Space(1), Tooltip("Glory cost required to perform this ability")]
		[SerializeField] int _gloryCost = 1;

		[Tooltip("The required number of selected tiles this ability needs in order for it to be performed")]
		[SerializeField] int _requiredTiles = 1;

		[Tooltip("The type of tiles this ability can target")]
		//This will be used to determine which tiles the UserInputHandler can pick
		[EnumFlags, SerializeField] TargetTileMask _targetTileTypes = (TargetTileMask)~0;

		//Members
		[HideInInspector]
		public List<Effect> effects = new List<Effect>();
		public bool[,] castArea { get; set; } = new bool[kCastAreaSqrLen, kCastAreaSqrLen];

		//Properties
		public Sprite icon => _icon;
		public int animNumber => _animNumber;
		public AbilityType type => _type;
		public string description => _description;
		public int gloryCost => _gloryCost;
		public int requiredTiles => _requiredTiles;
		public TargetTileMask targetTileTypes => _targetTileTypes;

		//Core
		public void Perform(Unit owner, params Tile[] targets)
		{
			foreach (var e in effects)
				e.Perform(owner, targets);
		}


		public Tile[] GetTargetableTiles(AnimateUnit au)
		{
			List<Tile> result = new List<Tile>();

			//Find the center of the cast area
			Vector2Int center = 
				new Vector2Int((castArea.GetLength(0) / 2) + (castArea.GetLength(0) % 2),
								(castArea.GetLength(1) / 2) + (castArea.GetLength(1) % 2));

			//Go through castArea
			for (int row = 0; row < 0; castArea.GetLength(0))	//rows
			{
				for (int col = 0; col < 0; castArea.GetLength(1))	//columns
				{
					//If 
					if (castArea[row, col])
					{
						Vector2Int temp = new Vector2Int(row, col);
						var offset = temp - center;

						if (au.currentTile.TryGetConnectedTile(offset, out Tile t))
						{
							result.Add(t);
						}
					}
				}
			}
			return au.possibleTargetTiles = result.ToArray();
		}

		public bool CanAcceptTileType(Unit user, Tile t)
		{
			//Only one of the masks have to pass for the whole thing to pass
			//Empty == No bitmask

			//Self: Return true if the user is standing on this tile
			if ((targetTileTypes & TargetTileMask.Self) == TargetTileMask.Self)
				if (user.currentTile == t) return true;

			var aliveUnits = UnitRegistry.current.aliveUnits;

			foreach (var unit in aliveUnits)
			{
				switch (unit)
				{
					case AllyUnit ally:
						//Allies: Return true if any allies are standing on this tile
						if ((targetTileTypes & TargetTileMask.Allies) == TargetTileMask.Allies)
							if (ally.currentTile == t) return true;
						break;
					case EnemyUnit enemy:
						//Enemies: Return true if any enemies are standing on this tile
						if ((targetTileTypes & TargetTileMask.Enemies) == TargetTileMask.Enemies)
							if (enemy.currentTile == t) return true;
						break;
					case InAnimateUnit inAnimate:
						//Inanimates: Return true if any inanimate units are on this tile
						if ((targetTileTypes & TargetTileMask.InAnimates) == TargetTileMask.InAnimates)
							if (inAnimate.currentTile == t) return true;
						break;
				}
			}
			return false;
		}

		//Add an effect to this ability (Has editor code)
		public void AddEffect<T>(bool hideInHierarchy = true) where T : Effect
		{
			//Add and set owner
			var newEffect = Effect.CreateInstance<T>();
			newEffect.SetOwner(this);
			newEffect.name = newEffect.GetType().Name;
			this.effects.Add(newEffect);

			//Hide flags
			if (hideInHierarchy) newEffect.hideFlags = HideFlags.HideInHierarchy;

#if UNITY_EDITOR
			//Save
			AssetDatabase.AddObjectToAsset(newEffect, this);
			AssetDatabase.SaveAssets();
#endif
		}

		//Removes an effect from this ability (Has Editor code)
		public void RemoveEffect(Effect e)
		{
			this.effects.Remove(e);
#if UNITY_EDITOR
			DestroyImmediate(e, true);
			AssetDatabase.SaveAssets();
#endif
		}


		//-------------------------------------------------------------
		//TRANSFERRED FROM OLD
		public void GetSelectableTiles(ref Unit unit)
		{
			//Q. WTF is this doing?
			//A. I think this populates the passed in ally unit's
			//tiles that this ability can be applied to

			int center = (castArea.GetLength(0) / 2) + (castArea.GetLength(0) % 2);
			int endPoint = castArea.GetLength(0) / 2;
			// int center = (castArea.Length / 2) + (castArea.Length % 2);
			// int endPoint = castArea.Length / 2;

			List<Tile> tiles = new List<Tile>();
			// Vector2Int coords = new Vector2Int();
			// List<Tile> nodes = new List<Tile>();
			// Vector2Int coords = Vector2Int.zero;

			for (int x = 0; x < castArea.GetLength(0); x++)
			{
				for (int y = 0; y < castArea.GetLength(1); y++)
				{
					if (castArea[x, y] == true)
					{
						int tx = -endPoint + x;
						int ty = endPoint - y;

						// coords.x = unit.currentTile.FindConnectedTile()
					}
				}
			}

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