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

namespace StormRend.Playables
{
	public class DialogMixerBehaviour : PlayableBehaviour
	{
		//Defaults
		float defaultFontSize;
		Color defaultColor;
		string defaultText;

		TextMeshProUGUI binding;
		bool firstFrameHappened;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			binding = playerData as TextMeshProUGUI;

			if (binding == null) return;

			if (!firstFrameHappened)
			{
				firstFrameHappened = true;
				defaultFontSize = binding.fontSize;
				defaultColor = binding.color;
				defaultText = binding.text;
			}

			Color blendedColor = Color.clear;
			float blendedFontSize = 0f;
			float totalWeight = 0f;
			float greatestWeight = 0f;
			int currentInputs = 0;

			for (int i = 0; i < playable.GetInputCount(); ++i)
			{
				float inputWeight = playable.GetInputWeight(i);
				ScriptPlayable<DialogBehaviour> inputPlayable = (ScriptPlayable<DialogBehaviour>)playable.GetInput(i);
				DialogBehaviour input = inputPlayable.GetBehaviour();

				//Blending color and size
				blendedColor += input.color * inputWeight;
				blendedFontSize += input.fontSize * inputWeight;
				
				totalWeight += inputWeight;

				if (inputWeight > greatestWeight)
				{
					binding.text = input.text;
					greatestWeight = inputWeight;
				}

				if (!Mathf.Approximately(inputWeight, 0f))
					currentInputs++;
			}

			binding.color = blendedColor + defaultColor * (1f - totalWeight);
			binding.fontSize = Mathf.RoundToInt(blendedFontSize + defaultFontSize * (1f - totalWeight));
			if (currentInputs != 1 && 1f - totalWeight > greatestWeight)
			{
				binding.text = defaultText;
			}
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			firstFrameHappened = false;

			if (binding == null) return;

			binding.color = defaultColor;
			binding.fontSize = defaultFontSize;
			binding.text = defaultText;
		}
	}
}