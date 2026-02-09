using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using UnityEngine;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
    public class SevenSegmentPrefab : DynamicPrefabGenerator<int>
    {
        protected override int GetIdentifierFor(ComponentData componentData)
            => componentData.InputCount;

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (CSevenSegment.DefaultInput, CSevenSegment.DefaultOutput);

        protected override Prefab GeneratePrefabFor(int inputCount)
        {
			ComponentInput[] inputs = new ComponentInput[CSevenSegment.DefaultInput];
			float length = CSevenSegment.DataPinLength;

			for (int i = CSevenSegment.DefaultInput - 1; i >= 0; i--)
			{
				inputs[i] = new ComponentInput
				{
					Position = new Vector3(
						CGlobal.LSBDir * ((0.5f * (CSevenSegment.DefaultInput - i)) - CGlobal.Offset),
						0.5f,
						-0.25f
					),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length,
				};
				length += CSevenSegment.DataPinLengthStep;
			}

			Block[] blocks = new Block[8];

			// 0-7 Add segments
			for (int i = 0; i < CSevenSegment.DefaultInput; i++)
			{
				blocks[CSevenSegment.DefaultInput - i - 1] = new Block{
					RawColor = CSevenSegment.OffColor,
					Position = CSevenSegment.Pos.segmentPositions[i][0],
					Rotation = CSevenSegment.Pos.segmentPositions[i][1],
					Scale = new Vector3(
						2f,
						1f,
						0.25f
					)
				};
			}

			blocks[7] = new Block{
				RawColor = CSevenSegment.BlockColor,
				Position = new Vector3(
					CGlobal.LSBDir * ((CSevenSegment.OriginalWidth / 2) - CGlobal.Offset),
					0f,
					0f
				),
				Scale = new Vector3(
					CSevenSegment.OriginalWidth,
					CSevenSegment.OriginalHeight,
					0.5f
				)
			};

            return new Prefab
            {
                Blocks = blocks,
                Inputs = inputs,
                Outputs = []
            };
        }
    }
}
