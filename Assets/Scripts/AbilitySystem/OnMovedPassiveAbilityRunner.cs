using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Utilities
{
	[RequireComponent(typeof(AnimateUnit))]
	public class OnMovedPassiveAbilityRunner : MonoBehaviour
	{
		AnimateUnit au = null;

		void Awake() => au = GetComponent<AnimateUnit>();

		void OnEnable() => au.onMoved.AddListener(OnMoved);
		void OnDisable() => au.onMoved.RemoveListener(OnMoved);

		void OnMoved(Tile tile)
		{
			
		}
	}
}