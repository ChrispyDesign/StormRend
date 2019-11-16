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