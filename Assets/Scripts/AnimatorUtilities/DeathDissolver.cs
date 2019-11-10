using System.Collections;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Assists
{
	public class DeathDissolver : MonoBehaviour
	{
		//Inspector
		[SerializeField] float initialDelay = 1f;
		[SerializeField] float dissolveDuration = 2f;
		[SerializeField] string paramName = "_DissolveValue";

		//Members
		// [ReadOnlyField, SerializeField] 
		List<Material> materials = new List<Material>();
		Unit u;
		AnimateUnit au;

		void Awake()
		{
			u = GetComponentInParent<Unit>();
			au = u as AnimateUnit;

			//Get materials from each child renderers
			var renderers = GetComponentsInChildren<Renderer>();
			foreach (var r in renderers)
				materials.Add(r.material);
		}

		public void Run()
		{
			//Dissolve animate units. Instantly kill anything else
			if (au)
				StartCoroutine(RunDeathDissolve());
			else
				u.Die();    //ie. Crystals
		}

		IEnumerator RunDeathDissolve()
		{
			//Initial delay
			yield return new WaitForSeconds(initialDelay);

			//Dissolve
			float value = 1f;
			float rate = 1f / dissolveDuration;
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
				m.SetFloat(paramName, dissolve);
		}
	}
}