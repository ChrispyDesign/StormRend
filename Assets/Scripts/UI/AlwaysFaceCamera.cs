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

namespace StormRend.UI
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