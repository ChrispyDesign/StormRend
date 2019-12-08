/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Systems;
using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class Transitioner : MonoBehaviour
	{
		//Maybe just create an audiosource, set it to play on awake and preload the start clip
		[SerializeField] AudioClip startClip;

		GameDirector gameDirector;

		private void Start()
		{
			if (startClip)
			{
				gameDirector = GameDirector.current;
				AudioSource src = gameDirector.sfxAudioSource;
				src.loop = false;

				src.clip = startClip;
				src.Play();
			}	
		}
	}
}