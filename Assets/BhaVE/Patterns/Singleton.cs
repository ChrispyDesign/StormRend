using UnityEngine;

namespace BhaVE.Patterns
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T _singleton;
		public static bool isInstanced => Singleton<T>._singleton != null;

		public static T singleton
		{
			get
			{
				if (_singleton == null)
				{
					_singleton = FindExistingSingleton() ?? CreateNewSingleton();
				}
				return _singleton;
			}
		}
		/// <summary>
		/// Finds an existing instance of this singleton in the scene.
		/// </summary>
		static T FindExistingSingleton()
		{
			T[] existingInstances = FindObjectsOfType<T>();

			if (existingInstances == null || existingInstances.Length == 0)
				return null;

			return existingInstances[0];
		}

		/// <summary>
		/// If no instance of the T MonoBehaviour exists, 
		/// creates a new GameObject in the scene and adds T to it.
		/// </summary>
		static T CreateNewSingleton()
		{
			GameObject container = new GameObject(typeof(T).Name + " (Singleton)");
			return container.AddComponent<T>();
		}

		/// <summary>
		/// All derived classes must run this base method
		/// </summary>
		protected virtual void Awake()
		{
			T thisSingleton = this.GetComponent<T>();

			if (_singleton == null)
			{
				_singleton = thisSingleton;
				// DontDestroyOnLoad(_singleton);
			}
			else if (thisSingleton != _singleton)
			{
				Debug.LogWarningFormat("Duplicate singleton found with type {0} in GameObject {1}",
					this.GetType(), this.gameObject.name);

				Component.Destroy(this.GetComponent<T>());
				return;
			}
		}
	}
}
