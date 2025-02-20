using JimmysUnityUtilities;
/*
 Color24
 */

using LogicAPI.Data;
/*
 ComponentData
 */

using LogicWorld.SharedCode.Components;
/*
 Prefab
 Block
 ComponentOutput
 ComponentInput
 */

using LogicWorld.Rendering.Dynamics;
/*
 DynamicPrefabGenerator<>
 */

using System.Collections.Generic;
/*
 List<>
 */

using UnityEngine;
/*
 Vector3
 Vector2
 */

using PixLogicUtils.Shared.CustomData;

using LICC;
using TMPro;
using System;
using System.IO;

using LogicWorld.Interfaces;
/*
 Instances
 Instances.MainWorld
 */

namespace PixLogicUtils.Client
{
    public class RegisterPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
    {
        private Color24			blockColor = new Color24(60, 60, 127);

		private int				dataWidth;
		private readonly int	defaultDataWidth = 8;

		private float			width;
		private float			height;
		private float			depth;

		private	float			x_comp;

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (
				1 +					// Clock
				1 +					// Write
				1 +					// Read
				1 +					// Low
				1 +					// High
				1 +					// +1
				1 +					// -1
				defaultDataWidth,	// DataWidth

				defaultDataWidth		// DataWidth
			);

        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		protected void setupPos(int outputCount)
		{
			width = dataWidth = outputCount;
			x_comp = -(width / 2);
		}

        public override void Setup(ComponentInfo info)
        {
			height = 2f;
			depth = 2f;

			setupPos(defaultDataWidth);
		}


		public void	updateOutput(int OutputCount)
		{
			if (dataWidth == OutputCount)
				return ;
			setupPos(OutputCount);
		}

        protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
        {
			updateOutput(identifier.OutputCount);

			var inputs = new ComponentInput[identifier.InputCount];
			var index = 0;

			// Clock pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0.5f, 1.5f, 0f),
				Rotation = new Vector3(90f, 90f, 0f),
			};

			// Write pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0.5f, 0.5f, 0f),
				Rotation = new Vector3(90f, 90f, 0f),
			};

			// Read pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0.5f, 0.5f, 1f),
				Rotation = new Vector3(90f, 90f, 0f),
			};

			// Low pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0f, 2f, 0.5f),
				Rotation = new Vector3(0f, 90f, 0f),
			};

			// High pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(x_comp, 2f, 0.5f),
				Rotation = new Vector3(0f, 90f, 0f),
			};

			// +1 pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0f, 1.5f, -0.5f),
				Rotation = new Vector3(-90f, 0f, 0f),
			};

			// -1 pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(0f, 0.5f, -0.5f),
				Rotation = new Vector3(-90f, 0f, 0f),
			};

			// Data Input
			for (var i = 0; i < dataWidth; i++)
			{
				inputs[index++] = new ComponentInput()
				{
					Position = new Vector3(-i, 1f, -0.5f),
					Rotation = new Vector3(-90f, 0f, 0f),
				};
			}

			// Datas Output
			var outputs = new ComponentOutput[identifier.OutputCount];
			index = 0;
			for (var i = 0; i < dataWidth; i++)
			{
				outputs[index++] = new ComponentOutput()
				{
					Position = new Vector3(-i, 1f, 1.5f),
					Rotation = new Vector3(90f, 0f, 0f),
				};
			}

            return new Prefab()
            {
                Blocks = new Block[]
				{
					new Block()
					{
						Scale = new Vector3(width, height, depth),
						Position = new Vector3(
							x_comp + 0.5f,		// x
							0,					// z
							(depth / 2) - 0.5f	// y
						),
						RawColor = blockColor,
					}
				},
                Inputs = inputs,
                Outputs = outputs,
            };
        }
	}
}
