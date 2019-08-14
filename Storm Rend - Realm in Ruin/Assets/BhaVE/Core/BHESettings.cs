using System;
using UnityEngine;
using BhaVE.Editor.Enums;

namespace BhaVE.Editor.Settings
{
	[Serializable, CreateAssetMenu(menuName = "BhaVE/Settings")]
	public class BHESettings : ScriptableObject
	{
		const float nodeAlpha = 0.9f;
		public Vector2 nodeSize = new Vector2(100, 75);

		[Header("Colors")]
		public Color editorBGColour = Color.white;
		public Color rootColour = new Color(1, 0.73f, 0.35f, nodeAlpha);  			//#FFBA59
		public Color selectorColour = new Color(1, 0.48f, 0.45f, nodeAlpha); 		//#FF7A73
		public Color sequenceColour = new Color(0.55f, 0.9f, 1, nodeAlpha);     	//#8CE6FF
		public Color decoratorColour = new Color(0.83f, 0.63f, 0.97f, nodeAlpha);	//#D5A1F8
		public Color conditionColour = new Color(1, 0.95f, 0.45f, nodeAlpha);       //#FFF373
		public Color actionColor = new Color(0.73f, 1, 0.5f, nodeAlpha); 			//#BBFF80

		[Header("Connections")]
		public BHEConnectionStyle connectionStyle = BHEConnectionStyle.Bezier;
		public float lineThickness = 3.5f;
		public Color defaultLineColour = Color.black;
		public Color runningLineColor = Color.white;
		public float bezierTangent = 20f;	

		[Header("Real-time Visualization")]
		public Color pendingColor = new Color(1, 0.92f, 0.2f, nodeAlpha);      //#FFEB33
		public Color failureColor = new Color(1, 0.3f, 0, nodeAlpha);       //#FF3D33
		public Color successColour = new Color(0.6f, 1, 0, nodeAlpha);      //#98FF33

	}
}


/* GUISkin nuances
window:
Normal = selected = false, mouseover = false
OnNormal: selected = true, mouseover = false
Hover: selected = false, mouseover = true
OnHover: selected = true, Mouseover = true

Active and Focuses not used (I think)
 */