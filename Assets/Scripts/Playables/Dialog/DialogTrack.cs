/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Sequencing;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StormRend.Playables
{
	//Pastel Orange
	[TrackColor(1, 0.73f, 0.35f), TrackBindingType(typeof(TextMeshProUGUI)), TrackClipType(typeof(DialogClip))]
	public class DialogTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<DialogMixerBehaviour>.Create(graph, inputCount);
		}

		public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
#if UNITY_EDITOR
			TextMeshProUGUI binding = director.GetGenericBinding(this) as TextMeshProUGUI;

			if (!binding) return;

			var serializedObject = new UnityEditor.SerializedObject(binding);
			var iterator = serializedObject.GetIterator();
			while (iterator.NextVisible(true))
			{
				if (iterator.hasVisibleChildren)
					continue;

				driver.AddFromName<TextMeshProUGUI>(binding.gameObject, iterator.propertyPath);
			}
#endif
			base.GatherProperties(director, driver);
		}
	}
}