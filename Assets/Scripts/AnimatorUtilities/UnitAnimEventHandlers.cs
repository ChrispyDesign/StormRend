using StormRend.Abilities;
using StormRend.Units;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Anim.EventHandlers
{
	public class UnitAnimEventHandlers : MonoBehaviour
	{
		//Inspector
		[SerializeField] ParticleSystem[] particles;

		public UnityEvent onExecuteAbility;
		public UnityEvent onDeathDissolve;
		public UnityEvent onFinaliseDeath;

		//Members
		protected Unit u;
		protected AnimateUnit au;

		void Awake()
		{
			u = GetComponentInParent<Unit>() as Unit;
			au = u as AnimateUnit;
		}

		public void PerformAbility()
		{
			au.Act();
			onExecuteAbility.Invoke();
		}

		public void DeathDissolve()
		{
			Debug.Log("Death Dissolve");
			onDeathDissolve.Invoke();
		}

		public void FinaliseDeath()
		{
			au.Die();
			onFinaliseDeath.Invoke();
		}
		public void PlayPFX(string particleName)
		{
			foreach (var p in particles)
			{
				if (p.name == particleName)
				{
					p.Play();	//Preliminary
				}
			}
		}
   	}
}