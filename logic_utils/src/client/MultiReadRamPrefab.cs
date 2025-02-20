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
    public abstract class MultiReadRamPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
    {
        private readonly Color24	blockColor = new Color24(60, 127, 60);

        public abstract int		readNumber { get; }
		private int				dataWidth;
		private int				addressWidth;

		private float			width;
		private float			height;
		private float			depth;

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (4 + 2 + ((8 + 1) * readNumber), 4 * readNumber);

        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

        public override void Setup(ComponentInfo info)
        {
			height = readNumber + 1f;
			depth = 1f;
        }

		public void	updateInputOutput(int InputCount, int OutputCount)
		{
			int	newDataWidth = (int)(OutputCount / readNumber);
			int	newAddressWidth = (int)((InputCount - (newDataWidth + 2 + readNumber)) / readNumber);

			// LConsole.WriteLine("readNumber       " + readNumber);
			// LConsole.WriteLine("InputCount       " + InputCount);
			// LConsole.WriteLine("OutputCount      " + OutputCount);
			// LConsole.WriteLine("dataWidth        " + dataWidth);
			// LConsole.WriteLine("addressWidth     " + addressWidth);
			// LConsole.WriteLine("newDataWidth     " + newDataWidth);
			// LConsole.WriteLine("newAddressWidth  " + newAddressWidth);

			if (dataWidth == newDataWidth && addressWidth == newAddressWidth)
				return ;

			dataWidth = newDataWidth;
			addressWidth = newAddressWidth;

			width = dataWidth > addressWidth ? dataWidth : addressWidth;
		}

        protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
        {
			updateInputOutput(identifier.InputCount, identifier.OutputCount);

			var inputs = new ComponentInput[identifier.InputCount];
			var index = 0;

			// Load pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(width - 1f, height, 1f),
				Rotation = new Vector3(0f, 90f, 0f),
			};

			// Data pin
			for (var i = dataWidth - 1; i >= 0; i--)
			{
				inputs[index++] = new ComponentInput()
				{
					Position = new Vector3(i, 0.5f, 0.5f),
					Rotation = new Vector3(-90f, 0f, 0f),
				};
			}

			// Addresses pin
			for (var i = 0; i < readNumber; i++)
			{
				// for (var j = 0; j < addressWidth; j++)
				for (var j = addressWidth - 1; j >= 0; j--)
				{
					inputs[index++] = new ComponentInput()
					{
						Position = new Vector3(j, i + 1.5f, 0.5f),
						Rotation = new Vector3(-90f, 0f, 0f),
					};
				}
			}

			// Write pin
			inputs[index++] = new ComponentInput()
			{
				Position = new Vector3(width - 0.5f, 0.5f, 1f),
				Rotation = new Vector3(90f, 90f, 0f),
			};

			// Reads pin
			for (var i = 0; i < readNumber; i++)
			{
				inputs[index++] = new ComponentInput()
				{
					Position = new Vector3(width - 0.5f, i + 1.5f, 1f),
					Rotation = new Vector3(90f, 90f, 0f),
				};
			}

			// Datas Output
			var outputs = new ComponentOutput[identifier.OutputCount];
			index = 0;
			for (var i = 0; i < readNumber; i++)
			{
				// for (var j = 0; j < dataWidth; j++)
				for (var j = dataWidth - 1; j >= 0; j--)
				{
					outputs[index++] = new ComponentOutput()
					{
						Position = new Vector3(j, 1.5f + i, 1.5f),
						Rotation = new Vector3(90f, 0f, 0f),
					};
				}
			}

            return new Prefab()
            {
                Blocks = new Block[]
				{
					new Block()
					{
						Scale = new Vector3(width, height, depth),
						Position = new Vector3(
							(width / 2) - 0.5f,
							0,
							(depth / 2) + 0.5f
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
