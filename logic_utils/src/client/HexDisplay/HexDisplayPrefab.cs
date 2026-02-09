using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using JimmysUnityUtilities;
using LogicAPI.Data;
using UnityEngine;
using System.Collections.Generic;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class HexDisplayPrefab : DynamicPrefabGenerator<int>
	{
		protected override int GetIdentifierFor(ComponentData componentData)
			=> componentData.InputCount;

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CHexDisplay.DefaultInput, CHexDisplay.DefaultOutput);

		protected override Prefab GeneratePrefabFor(int inputCount)
		{
			ComponentInput[] inputs = new ComponentInput[inputCount];

			inputs[CHexDisplay.Pin.OnOff] = new ComponentInput
			{
				Rotation = new Vector3(-90f, 0f, 0f),
				Length = CHexDisplay.ActionPinLength,
			};

			float length = CHexDisplay.DataPinLength;

			for (
				int i = CHexDisplay.Pin.DataStart;
				i < CHexDisplay.Pin.DataStart + 4;
				i++
			)
			{
				inputs[i] = new ComponentInput
				{
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length,
				};
				length += CHexDisplay.DataPinLengthStep;
			}

			Block baseBlock = new Block
			{
				RawColor = CHexDisplay.BlockColor,
				// Scale = new Vector3(
				// 	CHexDisplay.OriginalWidth,
				// 	CHexDisplay.OriginalHeight,
				// 	CHexDisplay.BlockDepth
				// ),
			};
			return new Prefab
			{
				Blocks = [
					baseBlock
				],
				Inputs = inputs,
				Outputs = []
			};
		}
	}
}
