using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

using LogicAPI.Data;

using UnityEngine;

using PixLogicUtils.Shared.Config;
using LICC;

namespace PixLogicUtils.Client
{
	public class DoubleDabblePrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CDoubleDabble.DefaultInput, CDoubleDabble.DefaultOutput);

		private int get_width(int output_count)
		{
			int pad = (output_count / 4) - 1;
			return pad + output_count;
		}

		protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
		{
			ComponentInput[] inputs = new ComponentInput[identifier.InputCount];
			float pin_y = CDoubleDabble.BlockHeight / 2;
			float pin_z = 0f;
			float length = CDoubleDabble.DataPinLength;
			for (int i = 0; i < identifier.InputCount; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * pin_z,
						pin_y,
						-CGlobal.Offset
					),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length
				};
				if (i % 4 == 3)
					pin_z += 2f;
				else
					pin_z++;
				length += CDoubleDabble.DataPinLengthStep;
			}

			int output_count = CDoubleDabble.WidthToOutput(identifier.InputCount);
			ComponentOutput[] outputs = new ComponentOutput[output_count];

			pin_z = 0f;
			for (int i = 0; i < output_count; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * pin_z,
						pin_y,
						CDoubleDabble.BlockDepth - CGlobal.Offset
					),
					Rotation = new Vector3(90f, 0f, 0f),
				};
				if (i % 4 == 3)
					pin_z += 2f;
				else
					pin_z++;
			}

			int width = get_width(output_count);

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							width,
							CDoubleDabble.BlockHeight,
							CDoubleDabble.BlockDepth
						),
						Position = new Vector3(
							CGlobal.LSBDir * (width / 2) + (
								width % 2 == 0
								? CGlobal.Offset
								: 0f
							),
							0f,
							(CDoubleDabble.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CDoubleDabble.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
