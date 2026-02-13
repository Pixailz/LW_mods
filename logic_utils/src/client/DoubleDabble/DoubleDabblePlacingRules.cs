using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;

namespace PixLogicUtils.Client
{
	public class DoubleDabblePlacingRules : DynamicPlacingRulesGenerator<(
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
				AllowFineRotation = true,
			};
		}
	}
}
