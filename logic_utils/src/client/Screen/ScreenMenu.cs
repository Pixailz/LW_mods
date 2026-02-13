using System.Collections.Generic;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicUI.MenuParts;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client.Menus
{
	public class ScreenMenu : DisplayConfigurationMenuBase
	{
		protected override int MinBPP => CScreen.MinBPP;
		protected override int MaxBPP => CScreen.MaxBPP;
		protected override IEnumerable<string> ComponentTypeIDs => [
			"PixLogicUtils.Screen"
		];

		public static void init()
		{
			WS.window("PixLogicUtils - Screen")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.add(WS.wrap(Instantiate(getVanillaEditDisplayMenuContent()))
						.injectionKey("displayConfigContainer"))
					.add(WS.textLine
						.setLocalizationKey("PixLogicUtils.ResolutionX")
						.setFontSize(24)
					)
					.add(WS.slider
						.injectionKey(nameof(ResolutionXSlider))
						.fixedSize(500, 45)
						.setInterval(1)
						.setMin(CScreen.MinResolutionX)
						.setMax(CScreen.MaxResolutionX)
					)
					.add(WS.textLine
						.setLocalizationKey("PixLogicUtils.ResolutionY")
						.setFontSize(24)
					)
					.add(WS.slider
						.injectionKey(nameof(ResolutionYSlider))
						.fixedSize(500, 45)
						.setInterval(1)
						.setMin(CScreen.MinResolutionY)
						.setMax(CScreen.MaxResolutionY)
					)
					.add(WS.textLine
						.setLocalizationKey("PixLogicUtils.DelayOnEndPulse")
						.setFontSize(24)
					)
					.add(WS.slider
						.injectionKey(nameof(DelayOnEndPulseSlider))
						.fixedSize(500, 45)
						.setInterval(1)
						.setMin(CScreen.MinDelayOnEndPulse)
						.setMax(CScreen.MaxDelayOnEndPulse)
					)
				)
				.add<ScreenMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider ResolutionXSlider = null;
		[AssignMe]
		public InputSlider ResolutionYSlider = null;
		[AssignMe]
		public InputSlider DelayOnEndPulseSlider = null;


		public override void Initialize()
		{
			base.Initialize();

			ResolutionXSlider.OnValueChanged += OnResolutionXChanged;
			ResolutionYSlider.OnValueChanged += OnResolutionYChanged;
			DelayOnEndPulseSlider.OnValueChanged += OnDelayOnEndPulseChanged;
		}

		public void OnResolutionXChanged(float newValue)
		{
			foreach (var Component in ComponentsBeingEdited)
			{
				(
					Component.ClientCode
					as ScreenClient
				).Data.ResolutionX = (int)newValue;
			}
		}

		public void OnResolutionYChanged(float newValue)
		{
			foreach (var Component in ComponentsBeingEdited)
			{
				(
					Component.ClientCode
					as ScreenClient
				).Data.ResolutionY = (int)newValue;
			}
		}

		public void OnDelayOnEndPulseChanged(float newValue)
		{
			foreach (var Component in ComponentsBeingEdited)
			{
				(
					Component.ClientCode
					as ScreenClient
				).Data.DelayOnEndPulse = (int)newValue;
			}
		}

		protected override void OnStartEditing()
		{
			base.OnStartEditing();
			var Data = (FirstComponentBeingEdited.ClientCode as ScreenClient).Data;
			ResolutionXSlider.SetValueWithoutNotify(Data.ResolutionX);
			ResolutionYSlider.SetValueWithoutNotify(Data.ResolutionY);
			DelayOnEndPulseSlider.SetValueWithoutNotify(Data.DelayOnEndPulse);
		}
	}
}
