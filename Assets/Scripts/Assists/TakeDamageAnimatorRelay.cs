using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	[RequireComponent(typeof(Animator))]
	public class TakeDamageAnimatorRelay : AnimatorRelay
	{
		[SerializeField] string animParam = "Damage";
		private Animator anim;

		void Awake()
		{
			anim = GetComponent<Animator>();
		}

		public void SetIntFromHealthData(HealthData data)
		{
			anim.SetInteger(animParam, data.amount);
		}
	}
}