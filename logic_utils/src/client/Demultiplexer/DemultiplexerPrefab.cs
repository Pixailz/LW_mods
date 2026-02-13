using LogicAPI.Data;
using LogicWorld.SharedCode.Components;
using LogicWorld.Rendering.Dynamics;
using UnityEngine;
using PixLogicUtils.Shared.Config;
using LICC;

namespace PixLogicUtils.Client
{
	public class DemultiplexerPrefab : DynamicPrefabGenerator<(
		int InputCount, int OutputCount
	)>
	{
		public override (int inputCount, int outputCount) GetDefaultPegCounts()
		{
			return (CDemultiplexer.DefaultInput, CDemultiplexer.DefaultOutput);
		}

		protected override
			(int InputCount, int OutputCount) GetIdentifierFor(
					ComponentData componentData
				)
				=> (componentData.InputCount, componentData.OutputCount);

		private static readonly Vector3 outputRotation = new Vector3(
			90f, 0f, 0f
		);
		private static readonly Vector3 inputRotation = new Vector3(
			-90f, 0f, 0f
		);
		private static readonly Vector3 selectorRotation = new Vector3(
			90f, -90f, 0f
		);

		private int		currentDataWidth	= 0;
		private int		currentSelector		= 0;
		private float	currentHeight		= 0;
		private float	currentPad			= 0;

		public static bool TryRecoverDataSelect((int InputCount, int OutputCount) id, out int data, out int select)
		{
			for (
				int s = CDemultiplexer.MinSelectorWidth;
				s <= CDemultiplexer.MaxSelectorWidth;
				s++
			)
			{
				int d = id.InputCount - s;
				if (d < 0) continue;

				if ((d << s) == id.OutputCount)
				{
					data = d;
					select = s;
					return true;
				}
			}

			data = 0;
			select = 0;
			return false;
		}

		private void getCurrentValue((int InputCount, int OutputCount) id)
		{
			TryRecoverDataSelect(id,
				out int tmpCurrentDataWidth,
				out int tmpCurrentSelector
			);

			if (
				this.currentDataWidth == tmpCurrentDataWidth
				&& this.currentSelector == tmpCurrentSelector
			)
				return ;
			this.currentDataWidth = tmpCurrentDataWidth;
			this.currentSelector = tmpCurrentSelector;
			this.currentHeight = 1 << this.currentSelector;
			if (this.currentDataWidth % 2 == 0)
				this.currentPad = CGlobal.Offset;
			else
				this.currentPad = 0f;
		}

		protected override Prefab GeneratePrefabFor(
			(int InputCount, int OutputCount) id
		)
		{
			ComponentInput[] inputs = new ComponentInput[id.InputCount];

			getCurrentValue(id);

			float length = CDemultiplexer.DataPinLength;

			// Selector pin
			for (int i = 0; i < this.currentSelector; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						-0.5f,
						0.5f + i,
						-0.5f + (0.50f * CDemultiplexer.BlockDepth)
					),
					Rotation = selectorRotation,
					Length = length
				};
				length += CDemultiplexer.DataPinLengthStep;
			}

			// Data pin
			for (
				int i = this.currentSelector;
				i < this.currentDataWidth + this.currentSelector;
				i++
			)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						i - this.currentSelector,
						0.5f,
						-0.5f
					),
					Rotation = inputRotation,
					Length = CDemultiplexer.ActionPinLength
				};
			}

			ComponentOutput[] outputs = new ComponentOutput[id.OutputCount];

			float pin_y = 0.5f;

			for (int i = 0; i < id.OutputCount; i++)
			{
				int ix = i % this.currentDataWidth;
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						ix,
						pin_y,
						-0.5f + CDemultiplexer.BlockDepth
					),
					Rotation = outputRotation
				};
				if (ix == this.currentDataWidth - 1)
					pin_y += 1f;
			}

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							this.currentDataWidth,
							this.currentHeight,
							CDemultiplexer.BlockDepth
						),
						Position = new Vector3(
							((this.currentDataWidth / 2) - this.currentPad),
							0,
							(CDemultiplexer.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CDemultiplexer.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
