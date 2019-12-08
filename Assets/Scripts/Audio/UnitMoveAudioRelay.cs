/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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