using pokoro.BhaVE.Core.Variables;
using UnityEngine;

namespace StormRend.Systems
{
    public class GloryResetOnStartup : MonoBehaviour
    {
        [SerializeField] BhaveInt glory;
        void Start()
        {
            glory.value = 0;
        }
    }

}