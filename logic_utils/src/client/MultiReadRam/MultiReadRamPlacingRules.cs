using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using UnityEngine;
using LogicAPI.Data;

namespace PixLogicUtils.Client
{
    public class MultiReadRamPlacingRules : DynamicPlacingRulesGenerator<(int InputCount, int OutputCount)>
    {
        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
            => (componentData.InputCount, componentData.OutputCount);

        protected override PlacingRules GeneratePlacingRulesFor((int InputCount, int OutputCount) identifier)
        {
            return new PlacingRules
            {
                AllowFineRotation = false,
            };
        }
    }
}
