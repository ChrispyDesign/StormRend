using UnityEngine;

namespace StormRend.VisualFX
{
    [CreateAssetMenu(menuName = "StormRend/VFX", fileName = "VFX")]
    public class VFX : ScriptableObject
    {
        [Tooltip("The life time of the VFX in seconds. Set to 0 for infinite")]
        public float lifetime = 5f;
        public GameObject prefab;
        
    }
}