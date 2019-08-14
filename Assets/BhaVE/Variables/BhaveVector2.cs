using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Vector2", fileName = "BhaveVector2")]
	public sealed class BhaveVector2 : BhaveVar<Vector2> 
	{
		public static implicit operator BhaveVector2(Vector2 rhs)
		{
			return new BhaveVector2 { value = rhs };
		}

		public static implicit operator Vector2(BhaveVector2 self)
		{
			return self.value;
		}

	}
}
