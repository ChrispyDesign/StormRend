using UnityEngine;
using UnityEngine.UI;

namespace StormRend.Prototype
{
	public class HealthBarUpdater : MonoBehaviour
	{
		[SerializeField] Image healthBar;
		 
		Unit unit;
		float healthValue;


		void Awake()
		{
			unit = GetComponentInParent<Unit>();
			healthValue = unit.HP;
		}

		void Update()
		{
			//Face camera's y angle
			transform.rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y - 180f, Vector3.up);

			//Update health bar
			healthValue = (unit.HP / (float)unit.maxHP);
			healthBar.fillAmount = healthValue;
		}
	}
}