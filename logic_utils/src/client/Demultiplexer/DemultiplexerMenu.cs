using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LogicUI.MenuParts;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;

using PixLogicUtils.Shared.Config;
using PixLogicUtils.Shared.CustomData;
using LogicAPI.Data;
using LogicWorld.BuildingManagement;
using LogicAPI.Data.BuildingRequests;

namespace PixLogicUtils.Client.Menus
{
	public class DemultiplexerMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicUtils - Demultiplexer")
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
								.add(WS.textLine.setLocalizationKey("PixLogicUtils.DemultiplexerWidth"))
								.add(WS.slider
									.injectionKey(nameof(dataWidthSlider))
									.fixedSize(500, 45)
									.setInterval(CDemultiplexer.StepDataWidth)
									.setMax(CDemultiplexer.MaxDataWidth)
									.setMin(CDemultiplexer.MinDataWidth)
								)
							)
							.addContainer("BottomBox1", container => container
								.layoutGrowGapHorizontalInnerCentered()
								.add(WS.textLine.setLocalizationKey("PixLogicUtils.DemultiplexerSelectorCount"))
								.add(WS.slider
									.injectionKey(nameof(selectorSlider))
									.fixedSize(500, 45)
									.setInterval(CDemultiplexer.StepSelectorWidth)
									.setMax(CDemultiplexer.MaxSelectorWidth)
									.setMin(CDemultiplexer.MinSelectorWidth)
								)
							)
						)
					)
				)
				.add<DemultiplexerMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider dataWidthSlider;
		[AssignMe]
		public InputSlider selectorSlider;
		[AssignMe]
		public GameObject bottomSection;

		private IDemultiplexerData currentData;

		protected override void OnStartEditing()
		{
			this.currentData =
				FirstComponentBeingEdited.ClientCode.CustomDataObject as IDemultiplexerData;

			dataWidthSlider.SetValueWithoutNotify(this.currentData.DataWidth);
			selectorSlider.SetValueWithoutNotify(this.currentData.SelectorWidth);
			bottomSection.SetActive(true);
		}

		public override void Initialize()
		{
			base.Initialize();
			dataWidthSlider.OnValueChangedInt += OnDataWidthChanged;
			selectorSlider.OnValueChangedInt += OnSelectorChanged;
		}

		private void SendBuildRequest(
			ComponentAddress addr,
			int newDataWidth,
			int newSelectorWidth
		)
		{
			this.currentData.DataWidth = newDataWidth;
			this.currentData.SelectorWidth = newSelectorWidth;
			BuildRequestManager.SendBuildRequest(
				new BuildRequest_ChangeDynamicComponentPegCounts(addr,
					this.currentData.DataWidth + this.currentData.SelectorWidth,
					(1 << this.currentData.SelectorWidth) * this.currentData.DataWidth
				)
			);
		}

		private void OnDataWidthChanged(int newDataWidth)
		{
			SendBuildRequest(
				FirstComponentBeingEdited.Address,
				newDataWidth,
				this.currentData.SelectorWidth
			);
		}

		private void OnSelectorChanged(int newSelectorWidth)
		{
			SendBuildRequest(
				FirstComponentBeingEdited.Address,
				this.currentData.DataWidth,
				newSelectorWidth
			);
		}

		protected override
		IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicUtils.Demultiplexer",
			];
		}
	}
}
