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

		public void Run()
		{
			StartCoroutine(RunBlizzardSequence());
		}

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