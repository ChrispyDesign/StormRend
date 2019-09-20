using BhaVE.Core;
using UnityEngine;
namespace BhaVE.Variables
{
	public abstract class BhaveVar<T> : BhaveVarSeed
	{
		public T value;

		[TextArea]
		public string description = "";
	}

	public abstract class BhaveVarSeed : ScriptableObject 
	{
		//IDEA: Use ScriptableObject.name to access this variable through the agent?
		public BhaveAgent owner;
		public int ID = -1;
	}
}