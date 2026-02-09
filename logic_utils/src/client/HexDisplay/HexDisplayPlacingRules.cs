using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using UnityEngine;

namespace PixLogicUtils.Client
{
	public class HexDisplayPlacingRules : DynamicPlacingRulesGenerator<int>
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
