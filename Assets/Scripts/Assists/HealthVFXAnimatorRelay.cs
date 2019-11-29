using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	[RequireComponent(typeof(Animator))]
	public class HealthVFXAnimatorRelay : AnimatorRelay
	{
		[SerializeField] string animDamageParam = "Damage";
		[SerializeField] string animHealParam = "Heal";
		
		Animator anim;
		void Awake() => anim = GetComponent<Animator>();

		public void OnTakeDamage(HealthData data) => anim.SetInteger(animDamageParam, data.amount);

		public void OnHeal(HealthData data) => anim.SetInteger(animHealParam, data.amount);
	}
}