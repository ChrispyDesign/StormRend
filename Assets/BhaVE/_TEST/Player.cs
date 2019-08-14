using BhaVE.Variables;
using UnityEngine;

namespace BhaVE.Test
{
	public class Player : MonoBehaviour
	{
		public BhaveTransform playerTransform;
		public float speed = 7.5f;
		public GameObject bulletPrefab;
		public float bulletForce = 20f;
		public ForceMode bulletForceMode = ForceMode.Impulse;
		public float bulletLifetime = 3f;

		void Update()
		{
			var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			transform.position += move * speed * Time.deltaTime;
			if (move.sqrMagnitude != 0f)
				transform.rotation.SetLookRotation(move);
			playerTransform.value = transform;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				var bullet = Instantiate(bulletPrefab, transform.position + Vector3.up, transform.rotation);
				bullet.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(move) * bulletForce, bulletForceMode);
				Destroy(bullet, bulletLifetime);
			}
		}
	}
}