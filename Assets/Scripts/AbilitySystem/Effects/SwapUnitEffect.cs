using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class SwapUnitEffect : Effect
	{
		[Tooltip("The particle to be instantiated")]
		[SerializeField] GameObject VFX = null;

		/// <summary>
		/// Swap or teleport units
		/// </summary>
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			//Make sure there is atleast 2 tiles passed in
			if (!(targetTiles.Length >= 2)) { Debug.LogWarning("Not enough target tiles! Exiting..."); return; }

			//Try getting animate units if they exist
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[0], out Unit u0);
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[1], out Unit u1);
			var au0 = u0 as AnimateUnit;
			var au1 = u1 as AnimateUnit;

			//Swap/teleport units if they exist
			au0?.Move(targetTiles[1], false, false, true);
			au1?.Move(targetTiles[0], false, false, true);

			//Reset begin tile
			if (au0) au0.beginTurnTile = au0.currentTile;
			if (au1) au1.beginTurnTile = au1.currentTile;

			//Recalculate move tiles
			au0?.CalculateMoveTiles();
			au1?.CalculateMoveTiles();

			//Instantiate VFXs
			var vfx0 = Instantiate(VFX, au0.transform.position, au0.transform.rotation);
			var vfx1 = Instantiate(VFX, au1.transform.position, au1.transform.rotation);

			//Get duration of particle effect
			var ps0 = vfx0.GetComponentInChildren<ParticleSystem>();
			var ps1 = vfx1.GetComponentInChildren<ParticleSystem>();
			var vfxDuration0 = ps0.main.duration + ps0.main.startLifetime.constant;
			var vfxDuration1 = ps1.main.duration + ps1.main.startLifetime.constant;

			//Destroy VFXs based on time
			Destroy(vfx0, vfxDuration0);
			Destroy(vfx1, vfxDuration1);
		}
	}
}