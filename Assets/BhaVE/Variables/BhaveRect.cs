using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Rect", fileName = "BhaveRect")]
	public sealed class BhaveRect : BhaveVar<Rect>
	{
		public static implicit operator BhaveRect(Rect rhs)
		{
			return new BhaveRect{ value = rhs };
		}
		public static implicit operator Rect(BhaveRect self)
		{
			return self.value;
		}
	}
}
