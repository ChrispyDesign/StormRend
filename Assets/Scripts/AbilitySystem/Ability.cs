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
		public const int castAreaSqrLen = 7;    //Cast Area Size Squared. Should this be some kind of global?

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
		public bool[] castArea { get; set; } = new bool[castAreaSqrLen * castAreaSqrLen];

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
			Debug.Log("Peforming Ability: " + this.name);
			foreach (var e in effects)
				e.Perform(owner, targets);
		}

		public bool CanAcceptTileType(Unit u, Tile t)
		{
			//Only one of the masks have to pass for the whole thing to pass
			//Empty == No bitmask

			//Self: Return true if the user is standing on this tile
			if ((targetTileTypes & TargetTileMask.Self) == TargetTileMask.Self)
				if (u.currentTile == t) return true;

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
	}
}