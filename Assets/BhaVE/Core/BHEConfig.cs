using System;
using UnityEngine;
using BhaVE.Editor.Enums;

namespace BhaVE.Editor.Settings
{
	[Serializable, CreateAssetMenu(menuName = "BhaVE/Settings", fileName = "BHEConfig")]
	public class BHEConfig : ScriptableObject
	{
		const float nodeAlpha = 0.9f;
		public Vector2 nodeSize = new Vector2(95, 65);

		[Header("Workspace")]
		public Color workspaceColour = new Color(0.9f, 0.9f, 0.9f, 1);          //HSVA:0,0,90,100
		[Range(1, 200)]
		public int gridMinor = 7;
		public Color gridMinorColour = new Color(0.5f, 0.9f, 1, 0.6f);			//HSVA:194,40,100,100
		[Range(1, 200)]
		public int gridMajor = 56;
		public Color gridMajorColour = new Color(0.5f, 0.9f, 1, 0.8f);			//HSVA:194,100,100,100

		[Header("Real-time Visualization")]
		public Color pendingColor = new Color(1, 0.92f, 0.2f, nodeAlpha);		//#FFEB33
		public Color failureColor = new Color(1, 0.3f, 0, nodeAlpha);       	//#FF3D33
		public Color successColour = new Color(0.6f, 1, 0, nodeAlpha);      	//#98FF33
		public Color abortColour = new Color(0.25f, 0.25f, 0.25f, nodeAlpha);	//HSVA:0,0,35,90
		public Color pauseColour = new Color(0.4f, 0.23f, 0.20f, nodeAlpha);	//HSVA:18,100,60,90

		[Header("Nodes")]
		public Color rootColour = new Color(1, 0.73f, 0.35f, nodeAlpha);  			//#FFBA59, HSVA:30,70,100,90
		public Color selectorColour = new Color(1, 0.48f, 0.45f, nodeAlpha); 		//#FF7A73, HSVA:3,55,100,90
		public Color sequenceColour = new Color(0.55f, 0.9f, 1, nodeAlpha);         //#8CE6FF, HSVA:193,45,100,90
		public Color decoratorColour = new Color(0.83f, 0.63f, 0.97f, nodeAlpha);   //#D5A1F8, HSVA:276,35,97,90
		public Color conditionColour = new Color(1, 0.95f, 0.45f, nodeAlpha);       //#FFF373, HSVA:50,55,100,90
		public Color actionColor = new Color(0.73f, 1, 0.5f, nodeAlpha);            //#BBFF80, HSVA:93,50,100,90
		public Color deactivateColor = new Color(0.35f, 0.35f, 0.35f, nodeAlpha);	//HSVA:0,0,70,90
        public Color suspendColor = new Color(0.5f, 0.272f, 0.25f, nodeAlpha);		//HSVA:5,58,80,95

		[Header("Connections")]
		public BHEConnectionStyle connectionStyle = BHEConnectionStyle.Bezier;
		public float lineThickness = 3.5f;
		public Color defaultLineColour = Color.black;
		public Color runningLineColor = Color.white;
		public float bezierTangent = 20f;
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