using StormRend.CameraSystem;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.Units
{
	public class HealthBar : MonoBehaviour
	{
		public Image bar;
		Camera cam;
		Unit unit;

		void Awake()
		{
			cam = MasterCamera.current.camera;
			unit = GetComponentInParent<Unit>();
		}

		//Register callbacks
		void OnEnable()
		{
			unit.onHeal.AddListener(OnHealthChange);
			unit.onTakeDamage.AddListener(OnHealthChange);
		}
		void OnDisable()
		{
			unit.onHeal.RemoveListener(OnHealthChange);
			unit.onTakeDamage.RemoveListener(OnHealthChange);
		}

		//Init bar at start
		void Start() => bar.fillAmount = (float)unit.HP / unit.maxHP;

		//Always face toward the camera
		void Update() => transform.rotation = Quaternion.AngleAxis(MasterCamera.current.transform.rotation.eulerAngles.y - 180f, Vector3.up);

		//Callbacks
		public void OnHealthChange(DamageData damageData) => OnHealthChange();
		public void OnHealthChange() => bar.fillAmount = (float)unit.HP / unit.maxHP;

	}
}