using System;
using System.Collections.Generic;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;
using StormRend.Enums;
using pokoro.BhaVE.Core.Variables;
using StormRend.Abilities.Effects;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Abilities
{
	public enum AbilityType
	{
		Passive,
		Primary,
		Secondary
	}

	[Serializable, CreateAssetMenu(menuName = "StormRend/Ability", fileName = "Ability")]
	public partial class Ability : ScriptableObject
	{
		//Constants
		const int descriptionCount = 3;
		public const int caSize = 7;    //Cast Area Size Squared

		//Inspector
		[SerializeField] Sprite _icon = null;
		[Tooltip("Animation trigger for this ability that will be sent to animator")]
		[SerializeField] string _animationTrigger = "";
		[SerializeField] AbilityType _type = AbilityType.Primary;
		[Range(1, 3), SerializeField] int _level = 1;
		[SerializeField] string _title = null;
		[TextArea(0, 2), SerializeField] string[] _descriptions = new string[descriptionCount];

		[Header("Casting"), Tooltip("Glory cost required to perform this ability")]
		[SerializeField] int _gloryCost = 1;
		[Tooltip("Insert main Glory SO variable")]
		[SerializeField] BhaveInt glory = null;

		[Header("Targeting"), Tooltip("The required number of selected tiles this ability needs in order for it to be performed. NOTE: Setting this to zero will instantly perform ability on self upon clicking on ability")]
		[SerializeField] int _requiredTiles = 1;

		[Tooltip("The tiles this ability can target")]
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
		public string title => _title;
		public string[] descriptions => _descriptions;
		public int gloryCost => _gloryCost;
		public int requiredTiles => _requiredTiles;
		public TargetType targetTileTypes => _targetTileTypes;

	#region Core
		//CRUNCH!
		public bool Perform(Unit owner, params Unit[] units)
			=> Perform(owner, units.Select(x => x.currentTile).ToArray());

		/// <summary>
		/// Perform the entire ability
		/// </summary>
		public bool Perform(Unit owner, params Tile[] targets)
		{
			foreach (var e in effects)
			{
				e.Perform(this, owner, targets);
			}
			return true;	//Successful ability execution
		}

		/// <summary>
		/// Perform a certain effect contained in this ability
		/// </summary>
		/// <typeparam name="T">Effect type to specifically perform</typeparam>
		public bool Perform<T>(Unit owner, params Tile[] targets) where T : Effect
		{
			foreach (var e in effects)
			{
				if (e is T)
				{
					e.Perform(this, owner, targets);
				}
			}
			return true;
		}

		/// <summary>
		/// Perform passive effects contained in this ability when a unit is killed
		/// </summary>
		public void PerformOnUnitKilled(Unit owner, Unit killed)
		{
			foreach (var e in effects)
			{
				var pe = e as PassiveEffect;
				if (pe)
				{
					if (pe.OnUnitKilled(this, owner, killed))
					{
						//Passive effect successful; perform animation
						owner.animator.SetTrigger(animationTrigger);
					}
				}
			}
		}

		/// <summary>
		/// Perform passive effects contained in this ability when a unit is created or spawned in
		/// </summary>
		public void PerformOnUnitCreated(Unit owner, Unit created)
		{
			foreach (var e in effects)
			{
				var pe = e as PassiveEffect;
				if (pe)
				{
					if (pe.OnUnitCreated(this, owner, created))
					{
						//Passive effect successful; perform animation
						owner.animator.SetTrigger(animationTrigger);
					}
				}
			}
		}
	#endregion

		/// <summary>
		/// Return true if the tile is valid according to this ability's targeting settings
		/// </summary>
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