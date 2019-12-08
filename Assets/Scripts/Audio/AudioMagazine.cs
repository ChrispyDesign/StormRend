/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    [CreateAssetMenu(menuName = "StormRend/AudioMagazine")]
    public class AudioMagazine : ScriptableObject
    {
		[Range(0f, 1f)] public float volume = 1f;
        [Range(0, 100)] public int chance = 50;
		public bool avoidRepetitions = true;

		//Members
		internal AudioClip lastPlayed = null;

        public List<AudioClip> clips;
	}
}