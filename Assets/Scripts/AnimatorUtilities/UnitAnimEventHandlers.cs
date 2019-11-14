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
        [Tooltip("Put references to prefabbed particles here")]
        [SerializeField] GameObject[] inbuiltVFX = null;
		[SerializeField] float inbuiltVFXLifetime = 5f;
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

    #region General
        public virtual void PerformAbility() => animateUnit.Act();

        public virtual void Die()
        {
            deathDissolver.Execute();
            onDeath.Invoke();   //Run death dissolve and die etc
        }
    #endregion

    #region Inbuilt VFX
		/// <summary>
		/// Run the built in VFX according to this event handler's lifetime setting
		/// </summary>
		public void ActivateInbuiltVFX(string name)
        {
            foreach (var ivfx in inbuiltVFX)
                if (ivfx.name == name) ivfx.SetActive(true);
        }

		/// <summary>
		/// Run the built in VFX according to this event handler's lifetime setting
		/// </summary>
		public void PlayInbuiltVFX(string name)
        {
            foreach (var ivfx in inbuiltVFX)
                if (ivfx.name == name) PlayInbuiltVFX(ivfx);
        }

        /// <summary>
        /// Immediately turn of the main VFX
        /// </summary>
        public void DeactivateInbuiltVFX(string name)
        {
            foreach (var ivfx in inbuiltVFX)
                if (ivfx.name == name) ivfx.SetActive(false);
        }

        /// <summary>
        /// Play inbuilt VFX according to it's settings
        /// </summary>
        IEnumerator PlayInbuiltVFX(GameObject ivfx)
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
            VFX vfx = o as VFX;
            var instance = Instantiate(vfx.prefab, unit.transform.position, unit.transform.rotation, unit.transform);

            //If the lifetime is set to 0 then let live infinitely
            if (!Mathf.Approximately(vfx.lifetime, 0f))
                Destroy(instance, vfx.lifetime);
        }
    #endregion
    }
}