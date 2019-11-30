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

		[Header("Dissolve")]
		[SerializeField] string dissolveShaderParam = "_DissolveValue";
		[SerializeField] float startDissolveValue = 1f;
		[SerializeField] float endDissolveValue = 0f;

		[Header("Shader swap")]
		[SerializeField] Shader noOutlinedDissolvable = null;

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

			//Swap shaders
			SwapShader(noOutlinedDissolvable);

			//Dissolve
			float time = 0f;
			float rate = 1f / duration;
			while (time < 1f)
			{
				time += rate * Time.deltaTime;

				foreach (var m in materials)
					SetDissolveLerp(m, time);
				yield return null;
			}

			//Make sure it is zeroed out properly
			foreach (var m in materials)
				SetDissolveLerp(m, 1f);

			//Finally kill unit
			au.Die();

			//------------------------------------------
			void SwapShader(Shader shader)
			{
				foreach (var m in materials)
				{
					m.shader = noOutlinedDissolvable;
				}
			}
		}

		void SetDissolveLerp(Material m, float t) 
			=> m.SetFloat(dissolveShaderParam, Mathf.Lerp(startDissolveValue, endDissolveValue, t));
	}
}