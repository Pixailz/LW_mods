using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using JimmysUnityUtilities;
using LogicAPI.Data;
using UnityEngine;

namespace PixLogicUtils.Client
{
    public class DummyTestPrefab : DynamicPrefabGenerator<int>
    {
        private readonly Color24 blockColor = new Color24(100, 150, 200); // Light blue color

        protected override int GetIdentifierFor(ComponentData componentData)
            => 0; // No variants

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (0, 0);

        protected override Prefab GeneratePrefabFor(int identifier)
        {
            // Create a simple 2x2 block
            var blocks = new Block[1];

            blocks[0] = new Block
            {
                RawColor = blockColor,
                Position = new Vector3(1f, 1f, 0f),
                Scale = new Vector3(2f, 2f, 2f)
            };

            return new Prefab
            {
                Blocks = blocks,
                Inputs = new ComponentInput[0],
                Outputs = new ComponentOutput[0]
            };
        }
    }
}
