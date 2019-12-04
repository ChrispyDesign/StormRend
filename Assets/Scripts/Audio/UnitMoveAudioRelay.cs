using StormRend.MapSystems.Tiles;
using StormRend.Tags;
using UnityEngine;

namespace StormRend.Audio
{
	public sealed class UnitMoveAudioRelay : AudioRelay
	{
		[SerializeField] AudioMagazine grassWalkSounds = null;
		[SerializeField] AudioMagazine paveWalkSounds = null;

		public void OnMove(Tile tile)
		{
			switch (tile.tag)
			{
				case GrassTileTag g:
					audioSystem.ChancePlayMagazineByAudioSource(grassWalkSounds, audioSource);
					break;
				case PaveTileTag p:
					audioSystem.ChancePlayMagazineByAudioSource(paveWalkSounds, audioSource);
					break;
			}
		}
	}
}