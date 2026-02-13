using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using EccsGuiBuilder.Client.Layouts.Helper;
using UnityEngine.UI;

namespace PixLogicUtils.Client.Menus
{
	public class DummyTestMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicUtils - DummyTest")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.addContainer("FixedContainer", container => container
						.injectionKey(nameof(fixedContainer))
					)
				)
				.add<DummyTestMenu>()
				.build();
		}

		[AssignMe]
		public GameObject fixedContainer = null!;

		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void OnStartEditing()
		{
		}

		protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicUtils.DummyTest",
			];
		}
	}
}
