using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Transform", fileName = "BhaveTranform")]
	public sealed class BhaveTransform : BhaveVar<Transform>
	{
		//This doesn't seem to work properly. Safer to set .value directly
		// public static implicit operator BhaveTransform(Transform rhs)
		// {
		// 	var ret = CreateInstance<BhaveTransform>();
		// 	ret.value = rhs;
		// 	return ret;
		// }

		public static implicit operator Transform(BhaveTransform self)
		{
			return self.value;
		}
	}
}
