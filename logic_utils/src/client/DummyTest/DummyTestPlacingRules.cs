using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using UnityEngine;
using LogicAPI.Data;

namespace PixLogicUtils.Client
{
    public class DummyTestPlacingRules : DynamicPlacingRulesGenerator<int>
    {
        protected override int GetIdentifierFor(ComponentData componentData)
            => 0;

        protected override PlacingRules GeneratePlacingRulesFor(int identifier)
        {
            return new PlacingRules
            {
                AllowFineRotation = true,
				// PrimaryGridPositions = [
				// 	new Vector2(0.5f, 0.5f),
				// 	new Vector2(1.5f, 0.5f),
				// 	new Vector2(2.5f, 0.5f),
				// 	new Vector2(3.5f, 0.5f),
				// ],
				// // GridPositionsAreRelative = true,
				// GridPlacingDimensions = new Vector2Int(4, 2)


				// // DEFAULT SETTINGS
				// GridPlacingDimensions = Vector2Int.one,
				// OffsetDimensions = Vector2Int.one,
				// DefaultOffset = Vector2Int.zero,
				// OffsetScale = Vector2Int.one,
				// CornerMidpoint = Vector2Int.zero,
				// AllowWorldRotation = true,
				// PrimaryGridPositions = [
				// 	new Vector2(0.5f, 0.5f)
				// ],
				// SecondaryGridPositions = [],
				// PrimaryEdgePositions = [],
				// SecondaryEdgePositions = [],
				// GridPositionsAreRelative = true,
				// EnableEdgeExtensionsDuringFinePlacement = true,


				// // XOR placing rules
				// OffsetDimensions = new Vector2Int(4, 1),
				// GridPlacingDimensions = new Vector2Int(4, 2),
				// AllowFineRotation = false,
				// CanBeFlipped = true,
				// FlippingPointHeight = 0.5f,
            };
        }
    }
}
