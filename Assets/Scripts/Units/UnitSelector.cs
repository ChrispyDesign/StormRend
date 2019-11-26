using StormRend.Systems;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	[RequireComponent(typeof(Unit))]
	public class UnitSelector : MonoBehaviour
	{
		[SerializeField] KeyCode selectKey = KeyCode.Alpha0;
		AnimateUnit au = null;
		UserInputHandler uih = null;

		void Awake()
		{
			au = GetComponent<AnimateUnit>();
			uih = UserInputHandler.current;
		}

		void Update()
		{
			if (Input.GetKeyDown(selectKey))
				if (!au.isDead)
					uih.SelectUnit(au, true);
		}
   	}
}