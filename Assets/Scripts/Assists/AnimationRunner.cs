/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Abilities;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Animation runner to be hooked up to AnimateUnit.OnAct(Ability)
	/// </summary>
	public class AnimationRunner : MonoBehaviour
	{
		[Header("Dynamically hook up to AnimateUnit.OnAct(Ability)")]
		[ReadOnlyField, SerializeField] Animator anim;
		void Awake() => anim = GetComponentInChildren<Animator>();
		public void Animate(Ability ability) => anim.SetTrigger(ability.animationTrigger);
	}
}