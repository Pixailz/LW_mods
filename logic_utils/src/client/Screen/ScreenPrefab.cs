using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using UnityEngine;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class ScreenPrefab : DynamicPrefabGenerator<int>
	{
		protected override int GetIdentifierFor(ComponentData componentData)
			=> componentData.InputCount;

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CScreen.DefaultInput, CScreen.DefaultOutput);

		public static readonly Vector3 PinOrientation = new Vector3(-90f, 0f, 0f);

		protected override Prefab GeneratePrefabFor(int inputCount)
		{
			ComponentInput[] inputs = new ComponentInput[
				CScreen.Pin.DataStart + CScreen.DefaultDataSize
			];

			inputs[CScreen.Pin.Clock] = new ComponentInput
			{
				Rotation = PinOrientation,
				Length = CScreen.ActionPinLength,
			};

			float length = CScreen.DataPinLength;

			for (
				int i = CScreen.Pin.DataStart;
				i < CScreen.Pin.DataStart + CScreen.DefaultDataSize;
				i++
			)
			{
				inputs[i] = new ComponentInput
				{
					Rotation = PinOrientation,
					Length = length,
				};
				length += CScreen.DataPinLengthStep;
			}

			ComponentOutput EndPulse = new ComponentOutput
			{
				Rotation = PinOrientation,
			};

			// Base block for the display
			Block BaseBlock = new Block
			{
				RawColor = CScreen.BlockColor,
			};

			return new Prefab
			{
				Blocks = [
					BaseBlock
				],
				Inputs = inputs,
				Outputs = [
					EndPulse
				],
			};
		}
	}
}
