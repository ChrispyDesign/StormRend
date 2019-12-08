/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using UnityEngine;

namespace StormRend.VisualFX
{
    public class Rune : MonoBehaviour
    {
        [SerializeField] List<Transform> nodes = new List<Transform>();
        [SerializeField] Transform target;
        [SerializeField] Transform start;

        [SerializeField] float speed = 1;
        [SerializeField] float percent = 1;


        void Start()
        {
            NewNode();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, target.position) <= 0.01f)
                NewNode();

            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start.position, target.position, percent);
        }

        void NewNode()
        {
            start = transform;

            int rand = Random.Range(0, nodes.Count);

            if (nodes[rand] == target)
                NewNode();
            else
                target = nodes[rand];

        }
    }
}