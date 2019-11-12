using StormRend.Abilities;
using StormRend.Units;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Anim.EventHandlers
{
	public class UnitAnimEventHandlers : MonoBehaviour
	{
		//Inspector
		[SerializeField] ParticleSystem[] particles = null;
		public UnityEvent onDeath = null;

		//Members
		protected Unit u = null;
		protected AnimateUnit au = null;

		void Awake()
		{
			u = GetComponentInParent<Unit>() as Unit;
			au = u as AnimateUnit;
		}

		public virtual void PerformAbility()
		{
			au.Act();
		}

		public virtual void Die()
		{
			onDeath.Invoke();	//Run death dissolve and die etc
		}

		public virtual void PlayPFX(string particleName)
		{
			foreach (var p in particles)
			{
				if (p.name == particleName)
				{
					p.Play();	//Preliminary; Might not work
				}
			}
		}
   	}
}