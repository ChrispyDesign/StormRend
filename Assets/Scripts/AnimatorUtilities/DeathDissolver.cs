using System.Collections;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.VisualFX;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Handles death sequence of the unit
	/// </summary>
	public class DeathDissolver : MonoBehaviour
	{
		//Inspector
		[SerializeField] VFX deathVFX = null;

		[Header("Designer to tune these values")]
		[SerializeField] float startDelay = 1.5f;
		[SerializeField] float duration = 2.5f;
		[SerializeField] string shaderParam = "_DissolveValue";

		//Members
		List<Material> materials = new List<Material>();
		Unit u;

		void Awake()
		{
			u = GetComponentInParent<Unit>();

			//Get materials from each child renderers
			var renderers = GetComponentsInChildren<Renderer>();
			foreach (var r in renderers)
				materials.AddRange(r.materials);
		}

		public void Execute()
		{
			//Dissolve animate units. Instantly kill anything else
			switch (u)
			{
				case AnimateUnit au:
					StartCoroutine(RunDeathDissolve(au));
					break;
				case InAnimateUnit iu:
					iu.Die();
					break;
				default:
					u.Die();
					break;
			}
		}

		IEnumerator RunDeathDissolve(AnimateUnit au)
		{
			//Create VFX
			deathVFX?.Play(au.transform.position, au.transform.rotation);

			//Initial delay
			yield return new WaitForSeconds(startDelay);

			//Dissolve
			float value = 1f;
			float rate = 1f / duration;
			while (value > 0f)
			{
				value -= rate * Time.deltaTime;
				SetDissolve(value);
				yield return null;
			}

			//Zero out dissolve
			SetDissolve(0f);

			//Finally kill unit
			au.Die();
		}

		void SetDissolve(float dissolve)
		{
			foreach (var m in materials)
				m.SetFloat(shaderParam, dissolve);
		}
	}
}