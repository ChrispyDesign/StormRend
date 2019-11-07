using pokoro.BhaVE.Core.Variables;
using UnityEngine;

namespace BhaVE.Test
{
	public class PlayerController : MonoBehaviour
	{
		public BhaveTransform playerTransform;
		public float speed = 7.5f;
		public GameObject bulletPrefab;
		public float bulletForce = 20f;
		public ForceMode bulletForceMode = ForceMode.Impulse;
		public float bulletLifetime = 3f;

		void Awake()
		{
			//Inits
			playerTransform.value = transform;
		}

		void Update()
		{
			//Move input
			var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			//Move
			transform.position += move * speed * Time.deltaTime;
			//Rotate
			if (move.sqrMagnitude != 0f)
				transform.rotation = Quaternion.LookRotation(move, Vector3.up);
			//Shoot
			if (Input.GetKey(KeyCode.Space))
			{
				var bullet = Instantiate(bulletPrefab, transform.position + Vector3.up, transform.rotation);
				bullet.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(move) * bulletForce, bulletForceMode);
				Destroy(bullet, bulletLifetime);
			}
		}
	}
}