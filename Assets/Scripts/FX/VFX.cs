using UnityEngine;

namespace StormRend.VisualFX
{
	/// <summary>
	/// Stores particle prefabs as well as have useful functions that auto instantiates and destroys the particle
	/// </summary>
    [CreateAssetMenu(menuName = "StormRend/VFX", fileName = "VFX")]
    public class VFX : ScriptableObject
    {
		//Inspector
		[Tooltip("The life time of the VFX in seconds. Set to 0 for infinite")]
       	public float lifetime = 5f;
		public bool autoDuration = false;
        public GameObject prefab;

		public float totalDuration		//Of the attached particle
		{
			get
			{
				//This might not work
				var ps = prefab.GetComponentInChildren<ParticleSystem>();
				return ps.main.startLifetime.constant + ps.main.startDelay.constant + ps.main.duration;
			}
		}

		public GameObject Play(Vector3 position, Quaternion rotation)
		{
			var go = Instantiate(prefab, position, rotation);
			Destroy(go, totalDuration);
			return go;
		}

		public GameObject Play(Vector3 position, Quaternion rotation, float duration)
		{
			var go = Instantiate(prefab, position, rotation);
			Destroy(go, duration);
			return go;
		}
    }
}