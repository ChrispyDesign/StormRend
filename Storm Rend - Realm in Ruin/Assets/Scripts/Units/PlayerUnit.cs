using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_cameraAnchor;

    public override void OnSelect()
    {
        base.OnSelect();

        CameraZoom zoom = m_camera.GetComponent<CameraZoom>();
        zoom.ZoomTo(m_cameraAnchor);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
    }

    public override void OnHover()
    {
        base.OnHover();
    }

    public override void OnUnhover()
    {
        base.OnUnhover();
    }
}
