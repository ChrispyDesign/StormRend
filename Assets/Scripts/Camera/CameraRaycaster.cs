using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// camera raycasting script responsible for hovering/selecting objects in the scene
/// </summary>
namespace StormRend
{
	[RequireComponent(typeof(Camera))]
	public class CameraRaycaster : MonoBehaviour
	{
		private Camera m_camera;

		// stored hover/selection objects
		private static GameObject m_hoveredObject;
		private static GameObject m_unhoveredObject;
		private static GameObject m_selectedObject;
		private static GameObject m_deselectedObject;

		#region getters

		public static GameObject GetHoveredObject() { return m_hoveredObject; }
		public static GameObject GetUnhoveredObject() { return m_unhoveredObject; }
		public static GameObject GetSelectedObject() { return m_selectedObject; }
		public static GameObject GetDeselectedObject() { return m_deselectedObject; }

		#endregion

		/// <summary>
		/// cache camera component on start
		/// </summary>
		void Start()
		{
			m_camera = GetComponent<Camera>();
		}

		/// <summary>
		/// perform raycast on every frame
		/// </summary>
		void Update()
		{
			PerformRaycast();
		}

		/// <summary>
		/// raycast function which uses mouse input to hover/select objects
		/// </summary>
		private void PerformRaycast()
		{
			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;

			// perform raycast
			Physics.Raycast(ray, out raycastHit);
			GameObject hitObject = null;

			// disallow raycasting if UI objects are selected/hovered
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (raycastHit.collider != null)
				hitObject = raycastHit.collider.gameObject;

			if (m_hoveredObject != hitObject)
				Hover(hitObject); // new hovered object

			if (Input.GetMouseButtonDown(0))
				Select(hitObject); // select (left click)
		}

		/// <summary>
		/// hover function which tracks hovered and unhovered objects, using the IHoverable interface
		/// </summary>
		/// <param name="hitObject">the object that was hit, can be null</param>
		private void Hover(GameObject hitObject)
		{
			if (m_unhoveredObject != m_hoveredObject)
			{
				// store unhovered object
				m_unhoveredObject = m_hoveredObject;

				// unhovered object can be null, so perform safety check
				if (m_unhoveredObject)
				{
					IHoverable unhoveredClickable = m_unhoveredObject.GetComponent<IHoverable>();
					if (unhoveredClickable != null)
						unhoveredClickable.OnUnhover(); // call unhover on unhovered object
				}
			}

			// store hovered object
			m_hoveredObject = hitObject;

			// hovered object can be null, so perform safety check
			if (m_hoveredObject)
			{
				IHoverable hoveredClickable = m_hoveredObject.GetComponent<IHoverable>();
				if (hoveredClickable != null)
					hoveredClickable.OnHover(); // call hover on hovered object
			}
		}

		/// <summary>
		/// select function which tracks selected and deselected objects, using the ISelectable interface
		/// </summary>
		/// <param name="hitObject">the object that was hit, can be null</param>
		private void Select(GameObject hitObject)
		{
			if (m_deselectedObject != m_selectedObject && hitObject != m_selectedObject)
			{
				// store deselected object
				m_deselectedObject = m_selectedObject;

				// deselected object can be null, so perform safety check
				if (m_deselectedObject)
				{
					ISelectable deselectedClickable = m_deselectedObject.GetComponent<ISelectable>();
					if (deselectedClickable != null)
						deselectedClickable.OnDeselect(); // call deselect on deselected object
				}
			}

			// store hit object
			m_selectedObject = hitObject;

			// selected object can be null, so perform safety check
			if (m_selectedObject)
			{
				ISelectable selectedClickable = m_selectedObject.GetComponent<ISelectable>();
				if (selectedClickable != null)
					selectedClickable.OnSelect(); // call select on selected object
			}
		}
	}
}