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
		[Flags]
		public enum TargetableTileCategory
		{
			Empty = 1 << 0,
			Self = 1 << 1,
			Allies = 1 << 2,
			Enemies = 1 << 3,
		} 

		//Inspectors
		[SerializeField] Sprite _icon = null;
		[TextArea, SerializeField] string _description = "";

		[Tooltip("Animation number for this ability in order to send to a corresponding animator")]
		[SerializeField] int _animNumber;
		
		[Tooltip("Glory cost required to perform this ability")]
		[SerializeField] int _gloryCost = 1;
		
		[Tooltip("The required number of selected tiles this ability needs in order for it to be performed")]
		[SerializeField] int requiredTiles = 1;

		[Tooltip("The category of tiles this ability can target")]
		[SerializeField, EnumFlags] TargetableTileCategory _targetableTileCategories;
		public bool[,] castArea = new bool[7, 7];

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
	}
}