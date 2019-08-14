using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Quaternion", fileName = "BhaveQuaternion")]
	public sealed class BhaveQuaternion : BhaveVar<Quaternion>
	{
		public static implicit operator BhaveQuaternion(Quaternion rhs)
		{
			return new BhaveQuaternion{ value = rhs };
		}
		public static implicit operator Quaternion(BhaveQuaternion self)
		{
			return self.value;
		}
	}
}
