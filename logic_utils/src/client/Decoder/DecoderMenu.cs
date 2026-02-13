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
	public class DecoderMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicUtils - Decoder")
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
										"PixLogicUtils.DecoderWidth"
									)
								)
								.add(WS.slider
									.injectionKey(nameof(decoderPegSlider))
									.fixedSize(500, 45)
									.setInterval(1)
									.setMin(CDecoder.MinInput)
									.setMax(CDecoder.MaxInput)
								)
							)
						)
					)
				)
				.add<DecoderMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider decoderPegSlider = null!;
		[AssignMe]
		public GameObject bottomSection = null!;

		protected override void OnStartEditing()
		{
			decoderPegSlider.SetValueWithoutNotify(
				FirstComponentBeingEdited.Component.Data.InputCount
			);
			bottomSection.SetActive(true);
		}

		public override void Initialize()
		{
			base.Initialize();
			decoderPegSlider.OnValueChangedInt += decoderCountChanged;
		}

		private void decoderCountChanged(int newDecoderWidth)
		{
			BuildRequestManager.SendBuildRequest(
				new BuildRequest_ChangeDynamicComponentPegCounts(
					FirstComponentBeingEdited.Address,
					newDecoderWidth,
					1 << newDecoderWidth
				)
			);
		}

		protected override
		IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicUtils.Decoder",
			];
		}
	}
}
