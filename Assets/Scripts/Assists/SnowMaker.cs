/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Plays snowing/blizzard sequence according to duration and transition curve
	/// </summary>
	public class SnowMaker : MonoBehaviour
	{
		[SerializeField] List<Material> materials = new List<Material>();
		[SerializeField] float duration = 5;
		[SerializeField] AnimationCurve transition = AnimationCurve.Linear(0, 0, 1, 1);

		//Reset the snow shader on start
		void Start() =>	SetSnowOpacity(0);

		public void Run() => StartCoroutine(RunBlizzardSequence());

		IEnumerator RunBlizzardSequence()
		{
			float time = 0;
			float rate = 1f / duration;
			while (time < 1f)
			{
				time += rate * Time.deltaTime;
				SetSnowOpacity(transition.Evaluate(time));
				yield return null;
			}
			
			//Reset snow opacity on finish to avoid annoying unstaged files in git 
			SetSnowOpacity(0);
		}

		void SetSnowOpacity(float value)
		{
			foreach (var m in materials)
				m.SetFloat("_SnowOpacity", value);
		}
	}
}