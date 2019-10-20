using System.Collections.Generic;
using System.Linq;
using BhaVE.Patterns;
using UnityEditor;
using UnityEngine;

namespace StormRend.Prototype 
{ 
	/// <summary>
	/// Holds a references to all scriptable objects esp. scriptable singletons so that the project can be built properly
	/// </summary>
	public class ScriptableHub : MonoBehaviour
	{
		public List<ScriptableObject> scriptablesObjects;

		public void LoadAllScriptableSingletons()
		{
			scriptablesObjects.AddRange(Resources.FindObjectsOfTypeAll<ScriptableSingletonSeed>().ToList());
		}

		public void LoadAllScriptableObjects()
		{
			scriptablesObjects = Resources.FindObjectsOfTypeAll<ScriptableObject>().ToList();
		}
   	}
}