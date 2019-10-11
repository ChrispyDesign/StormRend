using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Sequencing
{
	/// <summary>
	/// Attach to objects that will be used to detect objects 
	/// and then trigger it's sequence controller
	/// </summary>
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Rigidbody))]
	public class TriggerZone : MonoBehaviour
	{
		[SerializeField] SequenceController sequenceController;

		[Header("If empty will detect ANY object with collider")]
		[Tooltip("Detect only these objects in trigger zone")]
		[SerializeField] List<TriggerObject> triggerObjects = new List<TriggerObject>();

		Collider col;
		Rigidbody rb;

		void Awake()
		{
			Debug.Assert(sequenceController, "Sequence controller not found!");
			col = GetComponent<Collider>();
			rb = GetComponent<Rigidbody>();
		}
		void Reset() => col = GetComponent<Collider>();
		void OnValidate() => col = GetComponent<Collider>();

		void Start()
		{
			//Set core settings for the trigger zone to work properly
			col.isTrigger = true;
			rb.useGravity = false;  //Not really essential, just a safety precaution
			rb.isKinematic = true;
		}

		void OnTriggerEnter(Collider collider)
		{
			//Detect ANY collider
			if (triggerObjects.Count <= 0)
			{
				sequenceController.Play();
			}
			//Detect only Trigger Objects
			else
			{
				var triggerObjectHit = collider.GetComponent<TriggerObject>();
				if (triggerObjects.Contains(triggerObjectHit))
					sequenceController.Play();	
			}
		}

		void OnDrawGizmos()
		{
			var oldColor = Gizmos.color;
			Gizmos.color = Color.red;
			if (col) Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
			Gizmos.color = oldColor;
		}
	}
}