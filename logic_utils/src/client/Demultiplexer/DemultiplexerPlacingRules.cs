using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;

namespace PixLogicUtils.Client
{
	public class DemultiplexerPlacingRules : DynamicPlacingRulesGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		protected override PlacingRules GeneratePlacingRulesFor((int InputCount, int OutputCount) identifier)
		{
			DemultiplexerPrefab.TryRecoverDataSelect(identifier,
				out int data,
				out int selector
			);

			return new PlacingRules
			{
				CanBeFlipped = true,
				FlippingPointHeight = (1 << selector) / 2,

				AllowFineRotation = false,
			};
		}
	}
}
