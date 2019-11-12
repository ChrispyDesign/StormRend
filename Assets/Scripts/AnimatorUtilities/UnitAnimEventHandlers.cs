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
		[SerializeField] ParticleSystem[] onboardParticles = null;
		public UnityEvent onDeath = null;

		//Members
		protected Unit u = null;
		protected AnimateUnit au = null;
		protected DeathDissolver dd = null;

		void Awake()
		{
			u = GetComponentInParent<Unit>() as Unit;
			au = u as AnimateUnit;
			dd = GetComponent<DeathDissolver>();
		}

		public virtual void PerformAbility()
		{
			au.Act();
		}

		public virtual void Die()
		{
			dd.Execute();
			onDeath.Invoke();   //Run death dissolve and die etc
		}

		/// <summary>
		/// Takes in a PFX and plays it at the object's transform
		/// </summary>
		/// <param name="particle"></param>
		public virtual void PlayPFX(object particle)
		{
			//Instantiates the particle
			GameObject go = particle as GameObject;
			Instantiate(go, this.transform.position, this.transform.rotation);
		}

		public void PlayOnboardPFX(string name)
		{
			foreach (var p in onboardParticles)
			{
				if (p.name == name)
				{
					StartCoroutine(PlayPFXOnce(p));
				}
			}
		}

		IEnumerator PlayPFXOnce(ParticleSystem p)
		{
			//Activate particle
			p.gameObject.SetActive(true);

			//Deactivate once it finishes playing
			//TODO will have to consult with Dale here
			if (p.isPlaying) yield return null;
			p.gameObject.SetActive(false);
		}
	}
}