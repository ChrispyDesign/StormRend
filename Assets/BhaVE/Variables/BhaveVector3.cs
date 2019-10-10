using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Vector3", fileName = "BhaveVector3")]
	public sealed class BhaveVector3 : BhaveVar<Vector3>
	{
		public static implicit operator BhaveVector3(Vector3 rhs)
		{
			return new BhaveVector3 { value = rhs };
		}
		public static implicit operator Vector3(BhaveVector3 self)
		{
			return self.value;
		}
	}
}
