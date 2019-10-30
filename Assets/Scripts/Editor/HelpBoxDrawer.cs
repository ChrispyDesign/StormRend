using UnityEditor;
using UnityEngine;
using StormRend.Utility.Attributes;

namespace StormRend.Editors.PropDrawers
{
	[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
			//Exit if it's not a string
			if (property.type != "string") return;

			//Display help box
			string helpMessage = property.stringValue;
			EditorGUILayout.HelpBox(helpMessage, MessageType.Info);
        }
    }
}
