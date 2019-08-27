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

		void Start()
		{
			//Initially face the camera
			transform.rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y - 180f, Vector3.up);
		}

		void Update()
		{
			healthValue = (unit.HP / (float)unit.maxHP);

			healthBar.fillAmount = healthValue;
		}
	}
}