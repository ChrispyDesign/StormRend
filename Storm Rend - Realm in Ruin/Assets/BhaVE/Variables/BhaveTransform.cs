using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Transform", fileName = "BhaveTranform")]
	public sealed class BhaveTransform : BhaveVar<Transform> 
	{ 
		public static implicit operator BhaveTransform(Transform rhs)
		{
			return new BhaveTransform { value = rhs };
		}
		public static implicit operator Transform(BhaveTransform self)
		{
			return self.value;
		}
	}
}
