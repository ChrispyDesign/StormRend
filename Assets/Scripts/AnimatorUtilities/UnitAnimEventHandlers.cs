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
        [SerializeField] GameObject[] onboardParticles = null;

        [Tooltip("Put references to VFXs onboard this unit")]
        [SerializeField] GameObject[] inbuiltVFXs = null;
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

        public void RunVFX()
        {
            foreach (var vfx in inbuiltVFXs)
                PlayVFXOnce(vfx.GetComponent<VFX>());
        }

        /// <summary>
        /// Immediately turn of the main VFX
        /// </summary>
        public void StopVFX()
        {
            foreach (var v in inbuiltVFXs)
                v.SetActive(false);
        }

        /// <summary>
        /// Play inbuild VFX according to it's settings
        /// </summary>
        IEnumerator PlayVFXOnce(VFX vfx)
        {
            //Activate particle
            vfx.prefab.SetActive(true);

            //Deactivate once it finishes playing.
            if (!Mathf.Approximately(vfx.lifetime, 0f))
            	yield return new WaitForSeconds(vfx.lifetime);
			else
				yield return null;

			//Deactivate once it's lifetime is over
            vfx.prefab.SetActive(false);
        }
    }
}