using UnityEngine;
namespace BhaVE.Variables
{
	public abstract class BhaveVar<T> : BhaveVarSeed
	{
		public T value;

		[TextArea]
		public string description = "";
	}

	public abstract class BhaveVarSeed : ScriptableObject {}
}