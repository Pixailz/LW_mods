using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

using LogicAPI.Data;

using UnityEngine;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class PulserPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CPulser.DefaultInput, CPulser.DefaultOutput);

		protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
		{
			float pin_x = (CPulser.BlockWidth / 2) - CGlobal.Offset;
			float pin_y = CPulser.BlockHeight / 2;

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							CPulser.BlockWidth,
							CPulser.BlockHeight,
							CPulser.BlockDepth
						),
						Position = new Vector3(
							(CPulser.BlockWidth / 2) - CGlobal.Offset,
							0f,
							(CPulser.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CPulser.BlockColor,
					}
				],
				Inputs = [
					new ComponentInput()
					{
						Position = new Vector3(
							pin_x,
							pin_y,
							-CGlobal.Offset
						),
						Rotation = new Vector3(-90f, 0f, 0f),
						Length = CPulser.ActionPinLength
					},
					new ComponentInput()
					{

						Position = new Vector3(
							pin_x,
							pin_y,
							CPulser.BlockDepth - CGlobal.Offset
						),
						Rotation = new Vector3(90f, 0f, 0f),
						Length = CPulser.ActionPinLength * 1.5f
					}
				],
				Outputs = [],
			};
		}
	}
}
