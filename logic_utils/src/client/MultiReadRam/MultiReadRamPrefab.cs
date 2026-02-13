using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicWorld.SharedCode.Components;
using LogicWorld.Rendering.Dynamics;
using UnityEngine;
using PixLogicUtils.Shared.Config;
using LICC;

namespace PixLogicUtils.Client
{
	public class MultiReadRamPrefab : DynamicPrefabGenerator<(
		int InputCount, int OutputCount
	)>
	{
		public t_width	readNumber;
		private t_width	dataWidth;
		private t_width	addressWidth;

		private float	width;
		private float	height;

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
		{
			t_pin default_input =
				CMultiReadRam.DefaultDataWidth +
				CMultiReadRam.Pin.DataStart +
				(CMultiReadRam.DefaultDataWidth + 1) * this.readNumber;
			t_pin default_output = CMultiReadRam.DefaultAddressWidth * this.readNumber;
			return (default_input, default_output);
		}

		protected override
			(int InputCount, int OutputCount) GetIdentifierFor(
					ComponentData componentData
				)
				=> (componentData.InputCount, componentData.OutputCount);

		public override void Setup(ComponentInfo info)
		{
			readNumber = info.CodeInfoInts[0];
			height = readNumber + 1f;
		}

		public void	updateInputOutput(int InputCount, int OutputCount)
		{
			t_width	newDataWidth = OutputCount / this.readNumber;
			t_width newAddressWidth = (
				(
					InputCount - CMultiReadRam.Pin.DataStart - newDataWidth
				) / this.readNumber
			) - 1;

			if (newDataWidth < 1)
				newDataWidth = 1;
			if (newAddressWidth < 1)
				newAddressWidth = 1;

			if (dataWidth == newDataWidth && addressWidth == newAddressWidth)
				return ;

			dataWidth = newDataWidth;
			addressWidth = newAddressWidth;
			width = dataWidth > addressWidth ? dataWidth : addressWidth;
		}

		protected override Prefab GeneratePrefabFor(
			(int InputCount, int OutputCount) identifier
		)
		{
			updateInputOutput(identifier.InputCount, identifier.OutputCount);

			t_pin input_length = CMultiReadRam.Pin.DataStart;
			input_length += this.dataWidth;
			input_length += (this.addressWidth + 1) * this.readNumber;
			t_pin index_reads = this.dataWidth + CMultiReadRam.Pin.DataStart;
			t_pin index_address = index_reads + this.readNumber;
			float length = CMultiReadRam.DataPinLength;

			ComponentInput[] inputs = new ComponentInput[input_length];

			// Load pin
			inputs[CMultiReadRam.Pin.Load] = new ComponentInput()
			{
				Position = new Vector3(
					0f,
					height,
					-0.5f + 0.50f * CMultiReadRam.BlockDepth
				),
				Rotation = new Vector3(0f, 0f, 0f),
				Length = CMultiReadRam.ActionPinLength,
			};

			// Write pin
			inputs[CMultiReadRam.Pin.Write] = new ComponentInput()
			{
				Position = new Vector3(
					CGlobal.LSBDir * -0.5f,
					0.5f,
					-0.5f + 0.50f * CMultiReadRam.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CMultiReadRam.ActionPinLength,
			};

			// Data pin
			for (int i = CMultiReadRam.Pin.DataStart; i < index_reads; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * (i - CMultiReadRam.Pin.DataStart),
						0.5f,
						-0.5f
					),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length
				};
				length += CMultiReadRam.DataPinLengthStep;
			}

			// Reads pin
			for (int i = index_reads; i < index_reads + readNumber; i++)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * -0.5f,
						(i - index_reads) + 1.5f,
						-0.5f + 0.50f * CMultiReadRam.BlockDepth
					),
					Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
					Length = CMultiReadRam.ActionPinLength,
				};
			}

			// Addresses pin
			for (int i = 0; i < readNumber; i++)
			{
				length = CMultiReadRam.DataPinLength;
				for (
					var j = 0;
					j < this.addressWidth;
					j++
				)
				{
					inputs[index_address + (i * this.addressWidth + j)] = new ComponentInput()
					{
						Position = new Vector3(CGlobal.LSBDir * j, i + 1.5f, -0.5f),
						Rotation = new Vector3(-90f, 0f, 0f),
						Length = length
					};
					length += CMultiReadRam.DataPinLengthStep;
				}
			}

			// Datas Output
			ComponentOutput[] outputs = new ComponentOutput[this.dataWidth * this.readNumber];

			for (int i = 0; i < this.readNumber; i++)
			{
				for (int j = 0; j < this.dataWidth; j++)
				{
					outputs[i * this.dataWidth + j] = new ComponentOutput()
					{
						Position = new Vector3(
							CGlobal.LSBDir * j,
							1.5f + i,
							-0.5f + CMultiReadRam.BlockDepth
						),
						Rotation = new Vector3(90f, 0f, 0f),
					};
				}
			}

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(width, height, CMultiReadRam.BlockDepth),
						Position = new Vector3(
							CGlobal.LSBDir * ((width / 2) - CGlobal.Offset),
							0,
							(CMultiReadRam.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CMultiReadRam.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
