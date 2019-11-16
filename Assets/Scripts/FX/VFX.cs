using UnityEngine;

namespace StormRend.VisualFX
{
    [CreateAssetMenu(menuName = "StormRend/VFX", fileName = "VFX")]
    public class VFX : ScriptableObject
    {
		//Inspector
		[Tooltip("The life time of the VFX in seconds. Set to 0 for infinite")]
		public bool autoDuration = false;
       	public float lifetime = 5f;
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

		public void Create(Vector3 position, Quaternion rotation)
		{
			// Debug.Log("Total duration: " + totalDuration);
			Destroy(Instantiate(prefab, position, rotation), totalDuration);
		}

		public void Create(Vector3 position, Quaternion rotation, float duration)
		{
			Destroy(Instantiate(prefab, position, rotation), duration);
		}
    }
}