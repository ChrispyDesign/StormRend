using System;
using UnityEngine;
using BhaVE.Core;
using BhaVE.Nodes;
using BhaVE.Variables;

namespace BhaVE.Delegates.Examples
{
	[Serializable]
	[CreateAssetMenu(menuName = "BhaVE/Delegates/Actions/Shoot")]
	public class ShootAction : BhaveAction
	{
		public BhaveTransform target;
		public GameObject bulletPrefab;
		public float bulletForce = 10;
		public float bulletRate = 1f;
		public ForceMode bulletForceMode = ForceMode.Impulse;
		public float bulletLifetime = 3f;

		float lastShotTime;

        public override void Initiate(BhaveAgent agent) => lastShotTime = Time.time;

        public override NodeState Execute(BhaveAgent agent)
		{
			//If the time between last bullet shot
			if (Time.time - lastShotTime >= bulletRate)
			{
				// Debug.LogFormat("ShootAction.Execute(), target = {0}", target.value.position);
				
				//Start timer
				lastShotTime = Time.time;

				//Just shoot at target
				var bullet = Instantiate(bulletPrefab, agent.transform.position + Vector3.up, agent.transform.rotation);

				//Add force to the bullet
				var bulletVector = Vector3.Normalize(target.value.position - agent.transform.position);
				bullet.GetComponent<Rigidbody>().AddForce(bulletVector * bulletForce, bulletForceMode);

				
				//Clean it up
				Destroy(bullet, bulletLifetime);


				return NodeState.Success;
			}
			return NodeState.Pending;
		}
	}
}