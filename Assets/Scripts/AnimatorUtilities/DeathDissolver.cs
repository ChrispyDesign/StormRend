using System.Collections;
using System.Collections.Generic;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Handles death sequence of the unit
	/// </summary>
	public class DeathDissolver : MonoBehaviour
	{
		//Inspector
		[Header("Designer to tune these values")]
		[SerializeField] float startDelay = 1.5f;
		[SerializeField] float duration = 2.5f;
		[SerializeField] string shaderParam = "_DissolveValue";

		//Members
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
				materials.AddRange(r.materials);
		}

		public void Execute()
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