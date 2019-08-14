using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Color", fileName = "BhaveColor")]
	public sealed class BhaveColor : BhaveVar<Color>
	{
		public static implicit operator BhaveColor(Color rhs)
		{
			return new BhaveColor{ value = rhs };
		}
		public static implicit operator Color(BhaveColor self)
		{
			return self.value;
		}
	}
}
