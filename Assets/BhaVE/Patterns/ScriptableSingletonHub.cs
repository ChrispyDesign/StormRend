using UnityEngine;

namespace BhaVE.Patterns
{
	/// <summary>
	/// Place custom scriptable singletons into this hub in the scene to avoid dereferencing in build
	/// This hub will also call each scriptable singleton's OnFixedUpdate() and OnUpdate() methods
	/// </summary>
	public class ScriptableSingletonHub : MonoBehaviour
	{
		public ScriptableSingletonSeed[] scriptableSingletons;

		void Awake()
		{
			//Find all scriptable objects
			scriptableSingletons = Resources.FindObjectsOfTypeAll<ScriptableSingletonSeed>();
		}

		void FixedUpdate()
		{
			foreach (var ss in scriptableSingletons)
				ss.OnFixedUpdate();
		}

		void Update()
		{
			foreach (var ss in scriptableSingletons)
				ss.OnUpdate();
		}
	}
}