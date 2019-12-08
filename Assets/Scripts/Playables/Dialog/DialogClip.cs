/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StormRend.Playables
{
	[Serializable]
	public class DialogClip : PlayableAsset, ITimelineClipAsset
	{
		[SerializeField] DialogBehaviour template = new DialogBehaviour();

		public ClipCaps clipCaps => ClipCaps.Blending;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<DialogBehaviour>.Create(graph, template);
		}
	}
}