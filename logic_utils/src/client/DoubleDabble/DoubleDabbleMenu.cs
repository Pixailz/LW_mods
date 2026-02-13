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
	public class DoubleDabbleMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicUtils - DoubleDabble")
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
								.add(
									WS.textLine.setLocalizationKey(
										"PixLogicUtils.DoubleDabbleWidth"
									)
								)
								.add(WS.slider
									.injectionKey(nameof(dataWidthSlider))
									.fixedSize(500, 45)
									.setInterval(CDoubleDabble.StepDataWidth)
									.setMax(CDoubleDabble.MaxDataWidth)
									.setMin(CDoubleDabble.MinDataWidth)
								)
							)
						)
					)
				)
				.add<DoubleDabbleMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider dataWidthSlider = null!;
		[AssignMe]
		public GameObject bottomSection = null!;

		protected override void OnStartEditing()
		{
			dataWidthSlider.SetValueWithoutNotify(
				FirstComponentBeingEdited.Component.Data.InputCount
			);
			bottomSection.SetActive(true);
		}

		public override void Initialize()
		{
			base.Initialize();
			dataWidthSlider.OnValueChangedInt += decoderCountChanged;
		}

		private void decoderCountChanged(int newDecoderWidth)
		{
			BuildRequestManager.SendBuildRequest(
				new BuildRequest_ChangeDynamicComponentPegCounts(
					FirstComponentBeingEdited.Address,
					newDecoderWidth,
					CDoubleDabble.WidthToOutput(newDecoderWidth)
				)
			);
		}

		protected override
		IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicUtils.DoubleDabble",
			];
		}
	}
}
