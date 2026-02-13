using System.Collections.Generic;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client.Menus
{
	public class HexDisplayMenu : DisplayConfigurationMenuBase
	{
		protected override int MinBPP => CHexDisplay.FixedBPP;
		protected override int MaxBPP => CHexDisplay.FixedBPP;

		protected override IEnumerable<string> ComponentTypeIDs => [
			"PixLogicUtils.HexDisplay"
		];

		public static void init()
		{
			WS.window("PixLogicUtils - HexDisplay")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.add(WS.wrap(Instantiate(getVanillaEditDisplayMenuContent()))
						.injectionKey("displayConfigContainer"))
				)
				.add<HexDisplayMenu>()
				.build();
		}
	}
}
