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