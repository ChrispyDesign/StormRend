using System.Collections;
using StormRend.Assists;
using StormRend.Units;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Anim.EventHandlers
{
	[RequireComponent(typeof(DeathDissolver))]      //All units pretty much require this
	public class UnitAnimEventHandlers : MonoBehaviour
	{
		//Inspector
		[SerializeField] GameObject[] onboardParticles = null;
		public UnityEvent onDeath = null;

		//Members
		protected Unit unit = null;
		protected AnimateUnit animateUnit = null;
		protected DeathDissolver deathDissolver = null;

		void Awake()
		{
			unit = GetComponentInParent<Unit>() as Unit;
			animateUnit = unit as AnimateUnit;
			deathDissolver = GetComponent<DeathDissolver>();
		}

		public virtual void PerformAbility() => animateUnit.Act();

		public virtual void Die()
		{
			deathDissolver.Execute();
			onDeath.Invoke();   //Run death dissolve and die etc
		}

		/// <summary>
		/// Takes in a PFX and plays it at the object's transform
		/// </summary>
		/// <param name="particle"></param>
		public void PlayVFX(Object particle)
		{
			//Instantiates the particle
			GameObject go = particle as GameObject;
			Instantiate(go, this.transform.position, this.transform.rotation);
		}

		public void MountVFX(Object particle)
		{
			//Instantiates the particle
			GameObject particleGO = particle as GameObject;
			var pfx = Instantiate(particleGO, this.transform.position, this.transform.rotation);
			pfx.transform.SetParent(unit.transform);
		}

		/// <summary>
		/// Plays a particle effect that is loaded on this event handler
		/// </summary>
		/// <param name="name">The particle name to play</param>
		public void PlayOnboardVFX(string name)
		{
			// foreach (var p in onboardParticles)
			// {
			// 	if (p.name == name)
			// 	{
			// 		StartCoroutine(PlayVFXOnce(p));
			// 		return;
			// 	}
			// }
			// Debug.LogWarningFormat("Particle {0} not found!", name);
		}

		public void MountOnboardVFX(string name)
		{
			foreach (var po in onboardParticles)
			{
				if (po.name == name)
				{
					//Instantiates and mounts the particle
					var pfx = Instantiate(po.gameObject, this.transform.position, this.transform.rotation);
					pfx.transform.SetParent(unit.transform);
					return;
				}
			}
			Debug.LogWarningFormat("Particle {0} not found!", name);
		}

		//Not sure if this would work
		// IEnumerator PlayVFXOnce(ParticleSystem p)
		// {
		// 	//Activate particle
		// 	p.gameObject.SetActive(true);

		// 	//Deactivate once it finishes playing.
		// 	//TODO will have to consult with Dale here
		// 	if (p.isPlaying) yield return null;

		// 	p.gameObject.SetActive(false);
		// }
	}
}