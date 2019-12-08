/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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