/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] Image bar = null;
		Camera cam = null;
		AnimateUnit au = null;

		void Awake()
		{
			cam = MasterCamera.current.camera;
			au = GetComponentInParent<AnimateUnit>();

			Debug.Assert(au, "Animate unit not found!");

			//Register callbacks
			au.onHeal.AddListener(OnHealthChange);
			au.onTakeDamage.AddListener(OnHealthChange);
			// au.onMoved.AddListener(UpdateFacing);
			au.onDeath.AddListener(OnDeath);

			UpdateFacing();
		}

		void OnDestroy()
		{
			au.onHeal.RemoveListener(OnHealthChange);
			au.onTakeDamage.RemoveListener(OnHealthChange);
			// au.onMoved.RemoveListener(UpdateFacing);
			au.onDeath.RemoveListener(OnDeath);
		}

		//Init bar at start
		void Start() => bar.fillAmount = (float)au.HP / au.maxHP;

		void Update() => UpdateFacing();

		//Always face toward the camera
		void UpdateFacing(Tile t = null) => transform.rotation = Quaternion.AngleAxis(MasterCamera.current.transform.rotation.eulerAngles.y - 180f, Vector3.up);

		//Callbacks
		void OnDeath(Unit u) => gameObject.SetActive(false);
		public void OnHealthChange(HealthData damageData) => OnHealthChange();
		public void OnHealthChange() => bar.fillAmount = (float)au.HP / au.maxHP;
	}
}