using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Vector4", fileName = "BhaveVector4")]
	public sealed class BhaveVector4 : BhaveVar<Vector4>
	{
		public static implicit operator BhaveVector4(Vector4 rhs)
		{
			return new BhaveVector4{ value = rhs };
		}
		public static implicit operator Vector4(BhaveVector4 self)
		{
			return self.value;
		}
	}
}
