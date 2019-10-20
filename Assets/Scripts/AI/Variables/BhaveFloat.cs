using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Float", fileName = "BhaveFloat")]
	public sealed class BhaveFloat : BhaveVar<float>
	{
		public static implicit operator BhaveFloat(float rhs)
		{
			return new BhaveFloat { value = rhs };
		}
		public static implicit operator float(BhaveFloat self)
		{
			return self.value;
		}
	}
}
