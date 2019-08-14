using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Bool", fileName = "BhaveBool")]
	public sealed class BhaveBool : BhaveVar<bool>
	{
		public static implicit operator BhaveBool(bool rhs)
		{
			return new BhaveBool{ value = rhs };
		}
		public static implicit operator bool(BhaveBool self)
		{
			return self.value;
		}
	}
}
