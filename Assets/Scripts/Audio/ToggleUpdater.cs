using pokoro.BhaVE.Core.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	[RequireComponent(typeof(Toggle))]
	public sealed class ToggleUpdater : UIUpdater
	{
		[SerializeField] BhaveBool sov = null;
		Toggle toggle = null;

		void Awake()
		{
			toggle = GetComponent<Toggle>();
			Debug.Assert(sov, "SOV not found!");
		}

		void OnEnable() => toggle.isOn = sov.value;
	}
}