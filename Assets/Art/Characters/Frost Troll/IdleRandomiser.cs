using System.Collections;
using UnityEngine;

namespace StormRend.Other
{
	[RequireComponent(typeof(Animator))]
	public class IdleRandomiser : MonoBehaviour
	{
		[SerializeField] float minIdleTime = 5f;
		[SerializeField] float maxIdleTime = 10f;
		[SerializeField] string paramName = "Done_Waiting";

		Animator anim;
		float nextRandomTime;

		void Awake()
		{
			anim = GetComponent<Animator>();
		}

		void OnEnable()
		{
			nextRandomTime = Random.Range(minIdleTime, maxIdleTime);
			StartCoroutine(RandomIdles());
		}

		IEnumerator RandomIdles()
		{
			while (true)
			{
				yield return new WaitForSeconds(nextRandomTime);
				nextRandomTime = Random.Range(minIdleTime, maxIdleTime);
				anim.SetBool(paramName, true);
			}
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}
	}
}
