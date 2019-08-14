using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/String", fileName = "BhaveString")]
	public sealed class BhaveString : BhaveVar<string>
	{
		public static implicit operator BhaveString(string rhs)
		{
			return new BhaveString { value = rhs };
		}
		public static implicit operator string(BhaveString self)
		{
			return self.value;
		}
	}
}
