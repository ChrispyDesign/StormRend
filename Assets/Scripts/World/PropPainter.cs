using UnityEngine;

namespace StormRend.MapSystems
{
	[ExecuteInEditMode]
	public class PropPainter : MonoBehaviour	//Rename to Decorator, DecorationPainter, OrnamentPainter
	{
		//Enums
		public enum CollisionTest
		{
			RendererBounds,
			ColliderBounds
		}

		//Inspector
		public LayerMask layerMask;
		public Transform rootTransform;
		public CollisionTest collisionTest;

		[Range(0.1f, 5f)] public float _brushRadius = 5;
		[Range(0, 10f)] public float _brushDensity = 0.25f;
		[Range(0, 1)] public float _maxDensity = 0;
		[Range(0, 360)] public float _maxRandomRotation = 360f;

		[HideInInspector] public int selectedPrefabIndex = 0;

		public bool randomizeEachStamp = true;
		public GameObject[] prefabPalette;

		//Properties
		public float brushRadius
		{
			get => _brushRadius;
			set => _brushRadius = Mathf.Clamp(value, 0.1f, 5f);
		}
		public float brushDensity
		{
			get => _brushDensity;
			set => _brushDensity = Mathf.Clamp(value, 0, 10f);
		}
		public float maxDensity
		{
			get => _maxDensity;
			set => _maxDensity = Mathf.Clamp01(value);
		}
		public float maxRandomRotation
		{
			get => _maxRandomRotation;
			set => _maxRandomRotation = Mathf.Clamp(value, 0, 360f);
		}
		public GameObject SelectedPrefab => prefabPalette == null || prefabPalette.Length == 0 ? null : prefabPalette[selectedPrefabIndex];

		[ContextMenu("Delete Children")]
		void DeleteChildren()
		{
			while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}
}