using UnityEditor;
using BhaVE.Core;
using UnityEngine;

namespace BhaVE.Editor
{
    [CustomEditor(typeof(BhaveAgent))]
    public class BhaveAgentEditor : UnityEditor.Editor
    {
        BhaveAgent t;
        void OnEnable()
        {
            t = target as BhaveAgent;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal();
            {
                //Tick button
                if (GUILayout.Button("Tick"))
                {
                    BhaveDirector.singleton.Tick(t);
                }

                //Open Bhave Editor
                if (GUILayout.Button("Edit Tree"))
                {
                    //TODO
                    // BhaVEditor.SetActiveAgent(t);
                    //Selection.activeObject = t;
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
