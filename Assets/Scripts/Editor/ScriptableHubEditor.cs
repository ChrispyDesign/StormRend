using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StormRend.Prototype;

namespace StormRend.Editors
{
    [CustomEditor(typeof(ScriptableHub))]
    public class ScriptableHubEditor : Editor
    {
        ScriptableHub sh;

        void OnEnable()
        {
            sh = target as ScriptableHub;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Load All Scriptable Singletons"))
            {
                sh.LoadAllScriptableSingletons();
            }
            if (GUILayout.Button("Load All Scriptable Objects (Danger!)"))
            {
                sh.LoadAllScriptableObjects();
            }
            if (GUILayout.Button("Clear List"))
            {
                sh.scriptablesObjects.Clear();
            }
        }
    }
}