using System.Collections.Generic;
using UnityEngine;
using StormRend.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Sequencing
{
	/// <summary>
	/// NOTE!: Maybe cinemachine already has somethinglike this	
	/// Attach to objects that will be used to detect objects 
	/// and then trigger it's sequence controller
	/// </summary>
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Rigidbody))]
	public class TriggerZone : MonoBehaviour
	{
		[SerializeField] SequenceController sequenceController;

		[Tooltip("If empty will detect ANY object with collider, otherwise detect only these objects")]
		[SerializeField] LayerMask layerMask = ~0;
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

		void OnTriggerEnter(Collider col)
		{
			//Detect ANY collider as long as it's in the selected layermask
			if (triggerObjects.Count <= 0 && layerMask.Contains(col.gameObject.layer))
			{
				sequenceController.Play();
			}
			//Detect only Trigger Objects
			else
			{
				var triggerObjectHit = col.GetComponent<TriggerObject>();
				if (triggerObjects.Contains(triggerObjectHit) && layerMask.Contains(triggerObjectHit.gameObject.layer))
					sequenceController.Play();	
			}
		}

		void OnDrawGizmos()
		{
			//Draw Red Wireframe Box
			var oldColor = Gizmos.color;
			Gizmos.color = Color.red;
			if (col) Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
			Gizmos.color = oldColor;
#if UNITY_EDITOR
			//Draw Trigger Zone Text
			GUIStyle centeredLabel = new GUIStyle();
			centeredLabel.alignment = TextAnchor.MiddleCenter;
			centeredLabel.fontSize = 15;
			centeredLabel.normal.textColor = Color.white;
			using (new Handles.DrawingScope())
				Handles.Label(transform.position, "TriggerZone: " + name, centeredLabel);
#endif
		}
	}
}