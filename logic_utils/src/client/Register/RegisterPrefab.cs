using LogicAPI.Data;
using LogicWorld.SharedCode.Components;
using LogicWorld.Rendering.Dynamics;
using UnityEngine;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class RegisterPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
	{
		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CRegister.DefaultInput, CRegister.DefaultOutput);

		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		public override void Setup(ComponentInfo info)
		{ }

		protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
		{
			ComponentInput[] inputs = new ComponentInput[identifier.InputCount];
			float x_comp = identifier.OutputCount / 2;

			inputs[CRegister.Pin.Clock] = new ComponentInput()
			{
				Position = new Vector3(
					-CGlobal.LSBDir * CGlobal.Offset,
					0.75f * CRegister.BlockHeight,
					-CGlobal.Offset + 0.50f * CRegister.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.Write] = new ComponentInput()
			{
				Position = new Vector3(
					-CGlobal.LSBDir * CGlobal.Offset,
					0.25f * CRegister.BlockHeight,
					-CGlobal.Offset + 0.25f * CRegister.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.Read] = new ComponentInput()
			{
				Position = new Vector3(
					-CGlobal.LSBDir * CGlobal.Offset,
					0.25f * CRegister.BlockHeight,
					-CGlobal.Offset + 0.75f * CRegister.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.Low] = new ComponentInput()
			{
				Position = new Vector3(
					0f,
					CRegister.BlockHeight,
					-0.5f + 0.50f * CRegister.BlockDepth
				),
				Rotation = new Vector3(0f, 90f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.High] = new ComponentInput()
			{
				Position = new Vector3(
					CGlobal.LSBDir * x_comp,
					CRegister.BlockHeight,
					-0.5f + 0.50f * CRegister.BlockDepth
				),
				Rotation = new Vector3(0f, 90f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.Plus] = new ComponentInput()
			{
				Position = new Vector3(
					CGlobal.LSBDir * -0.25f,
					0.75f * CRegister.BlockHeight,
					-0.5f
				),
				Rotation = new Vector3(-90f, 0f, 0f),
				Length = CRegister.ActionPinLength,
			};

			inputs[CRegister.Pin.Minus] = new ComponentInput()
			{
				Position = new Vector3(
					CGlobal.LSBDir * 0.25f,
					0.25f * CRegister.BlockHeight,
					-0.5f
				),
				Rotation = new Vector3(-90f, 0f, 0f),
				Length = CRegister.ActionPinLength,
			};

			float length = CRegister.DataPinLength;
			for (
				var i = CRegister.Pin.DataStart;
				i < identifier.OutputCount + CRegister.Pin.DataStart;
				i++
			)
			{
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * (i - CRegister.Pin.DataStart),
						0.5f * CRegister.BlockHeight,
						-0.5f
					),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length,
				};
				length += CRegister.DataPinLengthStep;
			}

			ComponentOutput[] outputs = new ComponentOutput[identifier.OutputCount];
			for (var i = 0; i < identifier.OutputCount; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * i,
						0.5f * CRegister.BlockHeight,
						-0.5f + 1f * CRegister.BlockDepth
					),
					Rotation = new Vector3(90f, 0f, 0f),
				};
			}

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							identifier.OutputCount,
							CRegister.BlockHeight,
							CRegister.BlockDepth
						),
						Position = new Vector3(
							CGlobal.LSBDir * (x_comp - CGlobal.Offset),
							0,
							(CRegister.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CRegister.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = outputs,
			};
		}
	}
}
