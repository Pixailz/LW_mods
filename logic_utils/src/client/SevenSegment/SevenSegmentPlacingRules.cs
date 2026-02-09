using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;

namespace PixLogicUtils.Client
{
    public class SevenSegmentPlacingRules : DynamicPlacingRulesGenerator<int>
    {
        protected override int GetIdentifierFor(ComponentData componentData)
            => componentData.InputCount;

        protected override PlacingRules GeneratePlacingRulesFor(int identifier)
        {
            return new PlacingRules
            {
                AllowFineRotation = false,
            };
        }
    }
}
