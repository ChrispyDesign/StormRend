using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Assists.Designer
{
	public class LongShipMover : MonoBehaviour
	{
		[SerializeField] Vector2 speedLimit;
		[SerializeField] float speed;

		[SerializeField] Transform target;
		[SerializeField] Vector3 direction;
		[SerializeField] float checkRange = 0.15f;

		[SerializeField] List<Transform> LSTargets;


		void Start()
		{
			target = LSTargets[0];
			GetNewTarget();
		}

		void FixedUpdate()      //Max ~60fps; Slight optimization
		{
			if ((transform.position - target.position).sqrMagnitude > (checkRange * checkRange))
			{
				direction = target.position - transform.position;

				transform.LookAt(target.position);
				transform.position += direction.normalized * speed * Time.deltaTime;
			}
			else
			{
				GetNewTarget();
			}
		}


		void GetNewTarget()
		{
			int rand = Random.Range(0, LSTargets.Count);

			if (LSTargets[rand] == target)
			{
				GetNewTarget();
			}
			else
			{
				target = LSTargets[rand];
				speed = Random.Range(speedLimit.x, speedLimit.y);
				transform.LookAt(target.position);
			}
		}
	}
}