using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

using LogicAPI.Data;

using UnityEngine;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class DecoderPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CDecoder.DefaultInput, (int)CDecoder.DefaultOutput);

		protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
		{
			t_pin	n_output = 1 << identifier.InputCount;
			float	offset_height = 0.5f * CDecoder.BlockHeight;
			float	offset_depth = 0.5f * CDecoder.BlockDepth;

			ComponentInput[] inputs = new ComponentInput[identifier.InputCount];

			float	length = CDecoder.DataPinLength;

			for (int i = 0; i < identifier.InputCount; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * i,
						offset_height,
						-offset_depth
					),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length,
				};
				length += CDecoder.DataPinLengthStep;
			};

			ComponentOutput[] outputs = new ComponentOutput[n_output];

			for (int i = 0; i < n_output; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * i,
						offset_height,
						offset_depth
					),
					Rotation = new Vector3(90f, 0f, 0f),
				};
			};

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							n_output,
							CDecoder.BlockHeight,
							CDecoder.BlockDepth
						),
						Position = new Vector3(
							CGlobal.LSBDir * ((n_output / 2) - CGlobal.Offset),
							0f,
							0f
						),
						RawColor = CDecoder.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
