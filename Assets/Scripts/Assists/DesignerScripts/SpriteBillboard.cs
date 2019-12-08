/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
