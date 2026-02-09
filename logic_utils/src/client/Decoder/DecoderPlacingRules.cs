using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
    public class DecoderPlacingRules : DynamicPlacingRulesGenerator<(
		int InputCount, int OutputCount
	)>
    {
        protected override (int InputCount, int OutputCount) GetIdentifierFor(
			ComponentData componentData
		)
		{
			return (componentData.InputCount, componentData.OutputCount);
		}

        protected override PlacingRules GeneratePlacingRulesFor(
			(int InputCount, int OutputCount) identifier
		)
        {
			return new PlacingRules
			{
				CanBeFlipped = true,
				FlippingPointHeight = 0.50f * CDecoder.BlockHeight,

				AllowFineRotation = true,
			};
        }
    }
}
