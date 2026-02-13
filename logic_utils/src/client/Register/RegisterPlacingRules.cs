using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class RegisterPlacingRules : DynamicPlacingRulesGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		protected override PlacingRules GeneratePlacingRulesFor((int InputCount, int OutputCount) identifier)
		{
			return new PlacingRules
			{
				AllowFineRotation = true,
			};
		}
	}
}
