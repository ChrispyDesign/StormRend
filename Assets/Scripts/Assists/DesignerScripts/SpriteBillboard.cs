using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
        public Camera m_Camera;

        void Start()
        {
            if (m_Camera.orthographic)
                transform.LookAt(transform.position - m_Camera.transform.forward, m_Camera.transform.up);
            else
                transform.LookAt(m_Camera.transform.position, m_Camera.transform.up);
        }
    
}
