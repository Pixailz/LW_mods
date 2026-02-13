using LogicAPI.Data;
using LogicWorld.SharedCode.Components;
using LogicWorld.Rendering.Dynamics;
using UnityEngine;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class MultiplexerPrefab : DynamicPrefabGenerator<(
		int InputCount, int OutputCount
	)>
	{
		public override (int inputCount, int outputCount) GetDefaultPegCounts()
		{
			return (CMultiplexer.DefaultInput, CMultiplexer.DefaultOutput);
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


		public static void getCurrentSelector((int InputCount, int OutputCount) id, out int selector)
		{
			selector = id.InputCount % id.OutputCount;
			if (selector == 0)
			{
				for (
					selector = CMultiplexer.MinSelectorWidth;
					selector < CMultiplexer.MaxSelectorWidth;
					selector++
				)
				{
					if (id.InputCount == selector + (id.OutputCount << selector))
						break ;
				}
			}
		}

		private void getCurrentValue((int InputCount, int OutputCount) id)
		{
			int tmpCurrentSelector;

			getCurrentSelector(id, out tmpCurrentSelector);
			if (
				this.currentDataWidth == id.OutputCount
				&& this.currentSelector == tmpCurrentSelector
			)
				return ;
			this.currentDataWidth = id.OutputCount;
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

			float length = CMultiplexer.DataPinLength;

			// Selector pin
			for (int i = 0; i < this.currentSelector; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						-0.5f,
						0.5f + i,
						-0.5f + (0.50f * CMultiplexer.BlockDepth)
					),
					Rotation = selectorRotation,
					Length = length
				};
				length += CMultiplexer.DataPinLengthStep;
			}

			float pin_y = 0.5f;

			// Data pin
			for (int i = this.currentSelector; i < id.InputCount; i++)
			{
				int ix = (i - this.currentSelector) % this.currentDataWidth;
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						ix,
						pin_y,
						-0.5f
					),
					Rotation = inputRotation,
					Length = CMultiplexer.ActionPinLength
				};
				if (ix == this.currentDataWidth - 1)
					pin_y += 1f;
			}

			ComponentOutput[] outputs = new ComponentOutput[id.OutputCount];

			for (int i = 0; i < id.OutputCount; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						i,
						0.5f,
						-0.5f + CMultiplexer.BlockDepth
					),
					Rotation = outputRotation
				};
			}

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							this.currentDataWidth,
							this.currentHeight,
							CMultiplexer.BlockDepth
						),
						Position = new Vector3(
							(this.currentDataWidth / 2) - this.currentPad,
							0,
							(CMultiplexer.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CMultiplexer.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
