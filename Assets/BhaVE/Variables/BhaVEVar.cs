using UnityEngine;
namespace BhaVE.Variables
{
	public abstract class BhaveVar<T> : ScriptableObject, IBHVar
	{
		public T value;

		[TextArea]
		public string description = "";
	}
}