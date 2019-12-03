using pokoro.Patterns.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public sealed class MasterCanvas : Singleton<MasterCanvas>
	{
		public GraphicRaycaster graphicRaycaster { get; private set; }
		public Canvas canvas { get; private set; }
		void Awake()
		{
			canvas = GetComponent<Canvas>();
			graphicRaycaster = GetComponent<GraphicRaycaster>();
		}
	}
}