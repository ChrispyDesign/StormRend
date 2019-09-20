using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BhaVE.Patterns
{
	public abstract class ScriptableSingleton<T> : ScriptableSingletonSeed where T : ScriptableObject
	{
		#region Singleton
		readonly static string defaultPath = "Assets/BhaVE/Config/";

		static T _singleton = null;
		public static T singleton
		{
			get
			{
				if (!_singleton)
				{
					LocateFirstInstanceAndDeleteRest();
				}
				if (!_singleton)
				{
					InstantiateAndCreateAsset();
				}
				return _singleton;
			}
		}

		static void LocateFirstInstanceAndDeleteRest()
		{
			//Get all instances of this type
			T[] these = Resources.FindObjectsOfTypeAll<T>();

			if (these.Length > 0)
			{
				//Set the first instance to singleton
				_singleton = these[0];

#if UNITY_EDITOR
				//Delete the rest
				//NOTE: It is assumed that these objects will always be assets and never in memory
				for (int i = 1; i < these.Length; i++)
				{
					AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(these[i]));
				}
#endif
			}
		}

		static void InstantiateAndCreateAsset()
		{
			_singleton = CreateInstance<T>();
#if UNITY_EDITOR
			AssetDatabase.CreateAsset(_singleton, defaultPath + _singleton.GetType().Name + ".asset");
#endif
		}
		#endregion
	}

	/// <summary>
	/// Singleton Scriptable Object Base class that can be referenced by SS hub
	/// </summary>
	public abstract class ScriptableSingletonSeed : ScriptableObject
	{
		#region Hub Calls
		/// <summary>
		/// Called by hub every fixed update
		/// </summary>
		public virtual void OnFixedUpdate() { }

		/// <summary>
		/// Called by hub every update
		/// </summary>
		public virtual void OnUpdate() { }
		#endregion
	}
}