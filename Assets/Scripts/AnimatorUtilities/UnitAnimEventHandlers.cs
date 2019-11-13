using System.Collections;
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

        [Tooltip("Put references to VFXs onboard this unit")]
        [SerializeField] GameObject[] inbuiltVFXs = null;
		[SerializeField] float inbuiltVFXlifetime = 5f;
        public UnityEvent onDeath = null;

        //Members
        protected Unit unit = null;
        protected AnimateUnit animateUnit = null;
        protected DeathDissolver deathDissolver = null;

        void Awake()
        {
            unit = GetComponentInParent<Unit>() as Unit;
            animateUnit = unit as AnimateUnit;
            deathDissolver = GetComponent<DeathDissolver>();
        }

        public virtual void PerformAbility() => animateUnit.Act();

        public virtual void Die()
        {
            deathDissolver.Execute();
            onDeath.Invoke();   //Run death dissolve and die etc
        }

		/// <summary>
		/// Run the built in VFX according to this event handler's lifetime setting
		/// </summary>
		public void RunInbuiltVFX()
        {
            foreach (var ivfx in inbuiltVFXs)
                PlayInbuiltVFX(ivfx);
        }

        /// <summary>
        /// Immediately turn of the main VFX
        /// </summary>
        public void StopInbuiltVFX()
        {
            foreach (var v in inbuiltVFXs)
                v.SetActive(false);
        }

        /// <summary>
        /// Play inbuilt VFX according to it's settings
        /// </summary>
        IEnumerator PlayInbuiltVFX(GameObject ivfx)
        {
            //Activate particle
            ivfx.SetActive(true);

            //Deactivate once it finishes playing.
            if (!Mathf.Approximately(inbuiltVFXlifetime, 0f))
            	yield return new WaitForSeconds(inbuiltVFXlifetime);
			else
				yield return null;

			//Deactivate once it's lifetime is over
            ivfx.SetActive(false);
        }

        /// <summary>
        /// Takes in a PFX and plays it at the object's transform
        /// </summary>
        public void PlayVFX(Object o)
        {
            //Instantiates the particle
            GameObject go = o as GameObject;
            VFX vfx = go.GetComponent<VFX>();
            var instance = Instantiate(vfx.prefab, unit.transform.position, unit.transform.rotation);

            //If the lifetime is set to 0 then let live infinitely
            if (!Mathf.Approximately(vfx.lifetime, 0f))
                Destroy(instance, vfx.lifetime);
        }

        /// <summary>
        /// Instantiates and mounts the VFX to the unit
        /// </summary>
        public void MountVFX(Object o)
        {
            //Instantiates the particle
            GameObject go = o as GameObject;
            VFX vfx = go.GetComponent<VFX>();
            var instance = Instantiate(vfx.prefab, unit.transform.position, unit.transform.rotation, unit.transform);

            //If the lifetime is set to 0 then let live infinitely
            if (!Mathf.Approximately(vfx.lifetime, 0f))
                Destroy(instance, vfx.lifetime);
        }
    }
}