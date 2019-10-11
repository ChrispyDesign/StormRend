using UnityEngine;

namespace StormRend.Sequencing
{
	/// <summary>
	/// Attach to objects that will be used to set off sequence trigger zones
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class TriggerObject : MonoBehaviour
	{
		void Awake()
		{
			var col = GetComponent<Collider>();
			Debug.Assert(col, "No collider attached to trigger object!");
		}
	}
}