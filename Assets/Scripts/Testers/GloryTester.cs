using pokoro.BhaVE.Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Variables.Utils
{
	/// <summary>
	/// Custom variable listener and limiter
	/// </summary>
	public class GloryTester : MonoBehaviour
	{
		[SerializeField] BhaveInt glory = null;

		[TextArea(0, 2), SerializeField, Space(5)] string description = null;

		[Space]
		[SerializeField] bool debug = false;
		[SerializeField] KeyCode increaseKey = KeyCode.RightBracket;
		[SerializeField] KeyCode decreaseKey = KeyCode.LeftBracket;

		void Start() =>	glory.value = 0;	//Reset at startup

		void Update()
		{
			if (!debug) return;

			if (Input.GetKeyDown(increaseKey)) ++glory.value;
			if (Input.GetKeyDown(decreaseKey)) --glory.value;
		}
		void OnGUI()
		{
			if (!debug) return;

			GUILayout.Label("GLORY: " + glory?.value);
		}
	}
}