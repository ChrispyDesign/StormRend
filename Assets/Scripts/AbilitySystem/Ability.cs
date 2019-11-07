using System;
using System.Collections.Generic;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;
using StormRend.Enums;

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
	public partial class Ability : ScriptableObject
	{
		//Constants
		public const int caSize = 7;    //Cast Area Size Squared

		//Inspector
		[SerializeField] Sprite _icon = null;
		[Tooltip("Animation trigger for this ability that will be sent to animator")]
		[SerializeField] string _animationTrigger = "";
		[SerializeField] AbilityType _type = AbilityType.Primary;
		[SerializeField] int _level = 0;
		[TextArea(0, 2), SerializeField] string _description = "";

		[Header("Casting"), Space(1), Tooltip("Glory cost required to perform this ability")]
		[SerializeField] int _gloryCost = 1;

		[Tooltip("The required number of selected tiles this ability needs in order for it to be performed. NOTE: Setting this to zero will instantly perform ability on self upon clicking on ability")]
		[SerializeField] int _requiredTiles = 1;

		[Tooltip("The type of tiles this ability can target")]
		//This will be used to determine which tiles the UserInputHandler can pick
		[EnumFlags, SerializeField] TargetType _targetTileTypes = (TargetType)~0;

		//Members
		[HideInInspector]
		public List<Effect> effects = new List<Effect>();
		[HideInInspector] public bool[] castArea = new bool[caSize * caSize];		//this sometimes resets

		//Properties
		public Sprite icon => _icon;
		public string animationTrigger => _animationTrigger;
		public AbilityType type => _type;
		public int level => _level;
		public string description => _description;
		public int gloryCost => _gloryCost;
		public int requiredTiles => _requiredTiles;
		public TargetType targetTileTypes => _targetTileTypes;

		//Core
		public void Perform(Unit owner, params Tile[] targets)
		{
			Debug.Log("Performing Ability: " + this.name);
			foreach (var e in effects)
			{
				Debug.Log("Performing Effect: " + e.name);
				e.Perform(this, owner, targets);
			}
		}

		public bool IsAcceptableTileType(AnimateUnit owner, Tile tile)
		{
			//NOTE: Only one of the masks have to pass for the whole thing to pass
			//Empty: Return true if no units standing on the tile
			if ((targetTileTypes & TargetType.Empty) == TargetType.Empty)
				if (!UnitRegistry.IsAnyUnitOnTile(tile)) return true;

			//Self: Return true if the user is standing on this tile
			if ((targetTileTypes & TargetType.Self) == TargetType.Self)
				if (owner.currentTile == tile) return true;

			var aliveUnits = UnitRegistry.current.aliveUnits;

			foreach (var unit in aliveUnits)
			{
				bool isAnimate = false, isInAnimate = false;
				switch (unit)
				{
					case AllyUnit ally:
						isAnimate = true;
						//Allies: Return true if any allies are standing on this tile but not self
						if ((targetTileTypes & TargetType.Allies) == TargetType.Allies)
							if (ally.currentTile == tile && owner.currentTile != tile) return true;
						break;
					case EnemyUnit enemy:
						isAnimate = true;
						//Enemies
						if ((targetTileTypes & TargetType.Enemies) == TargetType.Enemies)
							if (enemy.currentTile == tile && owner.currentTile != tile) return true;
						break;
					case CrystalUnit crystal:
						isInAnimate = true;
						//Enemies
						if ((targetTileTypes & TargetType.Crystals) == TargetType.Crystals)
							if (crystal.currentTile == tile && owner.currentTile != tile) return true;
						break;
				}
				//Catch the abstracts
				//Animates
				if (isAnimate && (targetTileTypes & TargetType.Animates) == TargetType.Animates)
					if (unit.currentTile == tile && owner.currentTile != tile) return true;
				//InAnimates
				if (isInAnimate && (targetTileTypes & TargetType.InAnimates) == TargetType.InAnimates)
					if (unit.currentTile == tile && owner.currentTile != tile) return true;
			}
			return false;
		}

		//Add an effect to this ability (Has editor code)
		public void AddEffect<T>(bool hideInHierarchy = true) where T : Effect
		{
			//Add and set owner
			var newEffect = Effect.CreateInstance<T>();
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