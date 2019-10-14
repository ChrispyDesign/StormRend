using StormRend.Systems.Mapping;
using UnityEngine;

namespace StormRend.Abilities
{
	public abstract class Effect : ScriptableObject
	{
		protected bool isTileAllowed;	//WTF does this mean?
		public bool isFoldOut { get; set; } = true;

		public abstract bool Execute(Unit owner, Tile[] targets);
	}
}