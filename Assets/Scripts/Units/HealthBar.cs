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
		void Start()
		{
			bar.fillAmount = (float)unit.HP / unit.maxHP;
		}
		public void OnHealthChange()
		{
			bar.fillAmount = (float)unit.HP / unit.maxHP;
		}
		void Update()
		{
			transform.rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y - 180f, Vector3.up);
		}
	}
}