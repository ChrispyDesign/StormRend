/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections;
using System.Collections.Generic;
using StormRend.Assists;
using StormRend.Units;
using StormRend.VisualFX;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Anim.EventHandlers
{
	[RequireComponent(typeof(DeathDissolver))]      //All units pretty much require this
	public class UnitAnimEventHandlers : MonoBehaviour
	{
		//Inspector
		[Tooltip("Put references to prefabbed particles here")]
		[SerializeField] GameObject[] inbuiltVFX = null;
		[SerializeField] float inbuiltVFXLifetime = 5f;

		//Members
		protected Unit unit = null;
		protected AnimateUnit animateUnit = null;
		protected DeathDissolver deathDissolver = null;
		// protected Dictionary<GameObject, float> vfxDurations = new Dictionary<GameObject, float>();

		void Awake()
		{
			unit = GetComponentInParent<Unit>() as Unit;
			animateUnit = unit as AnimateUnit;
			deathDissolver = GetComponent<DeathDissolver>();
		}

		#region General
		public virtual void PerformAbility() => animateUnit?.Act();

		public virtual void Die()
		{
			switch (unit)
			{
				case AnimateUnit au:
					deathDissolver.Execute();
					break;
				case InAnimateUnit iu:
					iu.Die();
					break;
				default:
					unit.Die();
					break;
			}
		}
		#endregion

		#region Inbuilt VFX
		/// <summary>
		/// Activate the specified VFX
		/// </summary>
		public void ActivateInbuiltVFX(string name)
		{
			DeactivateInbuiltVFX(name);
			foreach (var ivfx in inbuiltVFX)
				if (ivfx.name == name) ivfx.SetActive(true);
		}

		/// <summary>
		/// Activates and then deactivates the inbuilt vfx based on it's total duration
		/// </summary>
		public void PlayInbuiltVFX(string name)
		{
			foreach (var ivfx in inbuiltVFX)
				if (ivfx.name == name)
					StartCoroutine(RunInbuiltVFX(ivfx));
		}

		/// <summary>
		/// Immediately turn off specified VFX
		/// </summary>
		public void DeactivateInbuiltVFX(string name)
		{
			foreach (var ivfx in inbuiltVFX)
				if (ivfx.name == name) ivfx.SetActive(false);
		}

		/// <summary>
		/// Immediately turn off all inbuilt VFX
		/// </summary>
		public void DeactivateAllInbuiltVFX()
		{
			foreach (var ivfx in inbuiltVFX)
				ivfx.SetActive(false);
		}


		//---------------------- COROUTINE IENUMERATOR -------------------------------
		IEnumerator RunInbuiltVFX(GameObject ivfx)
		{
			//Activate particle
			ivfx.SetActive(true);

			//Deactivate once it finishes playing.
			if (!Mathf.Approximately(inbuiltVFXLifetime, 0f))
				yield return new WaitForSeconds(inbuiltVFXLifetime);
			else
				yield return null;

			//Deactivate once it's lifetime is over
			ivfx.SetActive(false);
		}
		#endregion

		#region External VFX source
		/// <summary>
		/// Takes in a PFX and plays it at the object's transform
		/// </summary>
		public void PlayVFX(Object o)
		{
			//Instantiates the particle
			var vfx = o as VFX;
			var instance = Instantiate(vfx.prefab, unit.transform.position, unit.transform.rotation);

			//Particle's duration
			if (vfx.autoDuration)
				Destroy(instance, vfx.totalDuration);
			//User set duration
			else if (!Mathf.Approximately(vfx.lifetime, 0f))
				Destroy(instance, vfx.lifetime);
		}

		/// <summary>
		/// Instantiates and mounts the VFX to the unit
		/// </summary>
		public void MountVFX(Object o)
		{
			//Instantiates the particle
			VFX vfx = o as VFX;
			var instance = Instantiate(vfx.prefab, unit.transform.position, unit.transform.rotation, unit.transform);

			//Particle's duration
			if (vfx.autoDuration)
				Destroy(instance, vfx.totalDuration);
			//User set duration
			else if (!Mathf.Approximately(vfx.lifetime, 0f))
				Destroy(instance, vfx.lifetime);
		}
		#endregion
	}
}