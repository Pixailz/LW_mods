using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LogicAPI.Data.BuildingRequests;
using LogicUI.MenuParts;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicWorld.BuildingManagement;

using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client.Menus
{
	public class RegisterMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicUtils - Register")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.addContainer("BottomBox", bottomBox => bottomBox
						.injectionKey(nameof(bottomSection))
						.layoutVerticalInnerCentered()
						.addContainer("BottomInnerBox", innerBox => innerBox
							.layoutGrowGapVerticalInner()
							.addContainer("BottomBox1", container => container
								.layoutGrowGapHorizontalInnerCentered()
								.add(WS.textLine.setLocalizationKey("PixLogicUtils.RegisterWidth"))
								.add(WS.slider
									.injectionKey(nameof(registerPegSlider))
									.fixedSize(500, 45)
									.setInterval(CRegister.StepInput)
									.setMin(CRegister.MinInput)
									.setMax(CRegister.MaxInput)
								)
							)
						)
					)
				)
				.add<RegisterMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider registerPegSlider;
		[AssignMe]
		public GameObject bottomSection;

		protected override void OnStartEditing()
		{
			registerPegSlider.SetValueWithoutNotify(
				FirstComponentBeingEdited.Component.Data.OutputCount
			);
			bottomSection.SetActive(true);
		}

		public override void Initialize()
		{
			base.Initialize();
			registerPegSlider.OnValueChangedInt += registerCountChanged;
		}

		private void registerCountChanged(int newRegisterWidth)
		{
			BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
				FirstComponentBeingEdited.Address,
				newRegisterWidth + CRegister.Pin.DataStart,
				newRegisterWidth
			));
		}

		protected override
		IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicUtils.Register",
			];
		}
	}
}
