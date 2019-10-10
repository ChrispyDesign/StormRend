using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Sequencing
{
	/// <summary>
	/// Attach to objects that will be used to detect objects 
	/// and then trigger it's sequence controller
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class TriggerZone : MonoBehaviour
	{
		[SerializeField] SequenceController sequenceController;

		[Header("If empty will detect ANY object with collider")]
		[Tooltip("Detect only these objects in trigger zone")]
		[SerializeField] List<TriggerObject> triggerObjects = new List<TriggerObject>();

		void Awake()
		{
			Debug.Assert(sequenceController, "Sequence controller not found!");
		}

		void OnTriggerEnter(Collider collider)
		{
			//If set to filter out trigger objects...
			if (triggerObjects.Count > 0)
			{
				//Exit if no trigger objects detected
				var triggerObjectHit = collider.GetComponent<TriggerObject>();
				if (!triggerObjectHit) return;
			}

			//Object detected in zone
			sequenceController.Play();
		}

		void OnTriggerExit(Collider collider)
		{
			sequenceController.Pause();
		}
	}
}