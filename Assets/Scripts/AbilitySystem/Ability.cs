using System;
using System.Collections.Generic;
using StormRend.Systems.Mapping;
using UnityEngine;

namespace StormRend.Abilities
{
	public class Ability : ScriptableObject
	{
		[Flags]
		public enum TargetableTileType
		{
			Empty = 1 << 0,
			Self = 1 << 1,
			Allies = 1 << 2,
			Enemies = 1 << 3,
		} 

		//Inspectors
		[TextArea, SerializeField] string _description = "";

		[SerializeField] int _animNumber;
		[SerializeField] Sprite _icon = null;
		[SerializeField] int _gloryCost = 0;
		[SerializeField] int requiredNumberOfTiles = 1;

		public bool[,] castArea = new bool[7, 7];
		[SerializeField] TargetableTileType _targetableTileTypes;

		[HideInInspector] public List<Effect> effects = new List<Effect>();

		//Properties
		public int animNumber => _animNumber;
		public Sprite icon => _icon;
		public string description => _description;
		public int gloryCost => _gloryCost;
		public TargetableTileType targetableTileTypes => _targetableTileTypes;

		//Core
		public void Execute(Unit owner, Tile[] targets)
		{
			foreach (var e in effects)
			{
				e.Execute(owner, targets);
			}
		}
	}
}