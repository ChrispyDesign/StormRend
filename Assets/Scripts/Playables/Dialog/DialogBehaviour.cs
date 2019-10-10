using System;
using UnityEngine;
using UnityEngine.Playables;

namespace StormRend.Playables 
{ 
	[Serializable]
	public class DialogBehaviour : PlayableBehaviour
	{
		public float fontSize = 20f;
		public Color color = Color.red;
		[TextArea(3, 9)] public string text = "Insert Custom Dialogue Text";
   	}
}