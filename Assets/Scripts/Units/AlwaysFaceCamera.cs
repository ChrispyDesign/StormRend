using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using UnityEngine;

namespace StormRend.Units
{
    public class AlwaysFaceCamera : MonoBehaviour
    {
        Camera cam = null;
        Unit u = null;
        AnimateUnit au = null;

        void Awake()
        {
            cam = MasterCamera.current.camera;
            u = GetComponentInParent<Unit>();
        }

        void OnEnable()
        {
            au = u as AnimateUnit;
            //Animate unit
            if (au)
            {
                au.onMoved.AddListener(UpdateFacing);
            }

            //Initial face
            UpdateFacing();
        }

        void OnDisable()
        {
            if (au) au.onMoved.RemoveAllListeners();
        }

        void UpdateFacing(Tile t = null)
        {
            transform.rotation = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y - 180f, Vector3.up);
        }
    }
}