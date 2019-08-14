using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/GameObject", fileName = "BhaveGameObject")]
	public sealed class BhaveGameObject : BhaveVar<GameObject>
	{
		public static implicit operator BhaveGameObject(GameObject rhs)
		{
			return new BhaveGameObject { value = rhs };
		}
		public static implicit operator GameObject(BhaveGameObject self)
		{
			return self.value;
		}
	}
}
