using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

using JimmysUnityUtilities;

using LogicAPI.Data;

using UnityEngine;

using System.Collections.Generic;

using LICC;

namespace PixLogicUtils.Client
{
    public class DecoderPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
    {
		private readonly Color24	blockColor = new Color24(127, 60, 127);

        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
            => (componentData.InputCount, componentData.OutputCount);

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (2, 4);

        protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
        {
			ulong	n_input = (ulong)identifier.InputCount;
			ulong	n_output = 1ul << identifier.InputCount;
			float	width = (float)n_output;
			float	n_width = -((width / 2) - 0.5f);

			var inputs = new ComponentInput[n_input];

			float	length = 0.4f;

			for (int i = 0; i < (int)n_input; i++)
			{
                inputs[i] = new ComponentInput()
				{
					Position = new Vector3(-i, 0.5f, -0.5f),
					Rotation = new Vector3(-90f, 0f, 0f),
					Length = length,
				};
				// length -= 0.1f;
			};


			var outputs = new ComponentOutput[n_output];

			for (int i = 0; i < (int)n_output; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(-i, 0.5f, 0.5f),
                    Rotation = new Vector3(90f, 0f, 0f),
				};
			};

            return new Prefab()
            {
                Blocks = new Block[]
				{
					new Block()
					{
						Scale = new Vector3(width, 1f, 1f),
						Position = new Vector3(n_width, 0f, 0f),
						RawColor = blockColor,
					}
				},
                Inputs = inputs,
                Outputs = outputs,
            };
        }
    }
}
