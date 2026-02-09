using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using UnityEngine;

namespace PixLogicUtils.Client
{
	public class ScreenPlacingRules : DynamicPlacingRulesGenerator<int>
	{
		protected override int GetIdentifierFor(ComponentData componentData)
			=> componentData.InputCount;

		protected override PlacingRules GeneratePlacingRulesFor(int identifier)
		{
			return new PlacingRules
			{
				AllowFineRotation = false,
				GridPlacingDimensions = new Vector2Int(1, 1),
			};
		}
	}
}
