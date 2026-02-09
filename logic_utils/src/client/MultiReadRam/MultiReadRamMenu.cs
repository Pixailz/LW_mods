using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LICC;
using LogicAPI.Data.BuildingRequests;
using TMPro;
using LogicUI.MenuParts;
using System.IO;
using EccsGuiBuilder.Client.Layouts.Elements;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicWorld.BuildingManagement;

namespace PixLogicUtils.Client.Menus
{
    public class RamMenu : EditComponentMenu, IAssignMyFields
    {
		public static void init()
        {
            WS.window("PixLogicUtils - RAM")
                .setYPosition(150)
                .configureContent(content => content
                    .layoutVertical()
                    .add(WS.inputField
                        .injectionKey(nameof(filePathInputField))
                        .fixedSize(1000, 80)
                        .setPlaceholderLocalizationKey("PixLogicUtils.FileFieldHint")
                        .disableRichText()
                    )
                    .add(WS.textLine
                        .setLocalizationKey("PixLogicUtils.FileNotFound")
                        .injectionKey(nameof(errorText))
                    )
                    .add(WS.button.setLocalizationKey("PixLogicUtils.FileLoad")
                        .injectionKey(nameof(loadButton))
                        .add<ButtonLayout>()
                    )
                    .addContainer("BottomBox", bottomBox => bottomBox
                        .injectionKey(nameof(bottomSection))
                        .layoutVerticalInnerCentered()
                        .addContainer("BottomInnerBox", innerBox => innerBox
                            .layoutGrowGapVerticalInner()
                            .addContainer("BottomBox1", container => container
                                .layoutGrowGapHorizontalInnerCentered()
                                .add(WS.textLine.setLocalizationKey("PixLogicUtils.AddressLines"))
                                .add(WS.slider
                                    .injectionKey(nameof(addressPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(1)
                                    .setMin(1)
                                    .setMax(24)
                                )
                            )
                            .addContainer("BottomBox2", container => container
                                .layoutGrowGapHorizontalInnerCentered()
                                .add(WS.textLine.setLocalizationKey("PixLogicUtils.BitWidth"))
                                .add(WS.slider
                                    .injectionKey(nameof(widthPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(1)
                                    .setMin(1)
                                    .setMax(64)
                                )
                            )
                        )
                    )
                )
                .add<RamMenu>()
                .build();
        }

        [AssignMe]
        public TMP_InputField filePathInputField;
        [AssignMe]
        public HoverButton loadButton;
        [AssignMe]
        public InputSlider addressPegSlider;
        [AssignMe]
        public InputSlider widthPegSlider;
        [AssignMe]
        public GameObject bottomSection;
        [AssignMe]
        public GameObject errorText;

        protected override void OnStartEditing()
        {
			var data = (FirstComponentBeingEdited.ClientCode as MultiReadRamClient).Data;
			var input_count = FirstComponentBeingEdited.Component.Data.InputCount;
			var output_count = FirstComponentBeingEdited.Component.Data.OutputCount;
			var bit_width = output_count / (int)data.ReadNumber;
			var address_width = ((input_count - 1 - 1 - bit_width) / (int)data.ReadNumber) - 1;

			errorText.SetActive(false);
			addressPegSlider.SetValueWithoutNotify(address_width);
			widthPegSlider.SetValueWithoutNotify(bit_width);
			bottomSection.SetActive(true);
            filePathInputField.text = "";
        }

        public override void Initialize()
        {
            base.Initialize();
            loadButton.OnClickEnd += loadFile;
            addressPegSlider.OnValueChangedInt += addressCountChanged;
            widthPegSlider.OnValueChangedInt += bitwidthChanged;
            filePathInputField.onValueChanged.AddListener(text => errorText.SetActive(false));
        }

        private void bitwidthChanged(int newBitwidth)
        {
			int read_number = (FirstComponentBeingEdited.ClientCode as MultiReadRamClient).Data.ReadNumber;

            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                2 + newBitwidth + ((addressPegSlider.ValueAsInt + 1) * read_number),
                newBitwidth * read_number
            ));
        }

        private void addressCountChanged(int newAddressBitWidth)
        {
			int read_number = (FirstComponentBeingEdited.ClientCode as MultiReadRamClient).Data.ReadNumber;
            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                2 + widthPegSlider.ValueAsInt + ((newAddressBitWidth + 1) * read_number),
                widthPegSlider.ValueAsInt * read_number
            ));
        }

        private void loadFile()
        {
            var loadable = (FileLoadable) FirstComponentBeingEdited.ClientCode;
            var filePath = filePathInputField.text;
            if (File.Exists(filePath))
            {
                var bytes = File.ReadAllBytes(filePath);
                var lineWriter = LConsole.BeginLine();
                loadable.Load(bytes, lineWriter, true);
                lineWriter.End();
            }
            else
            {
                errorText.SetActive(true);
                LConsole.WriteLine($"Unable to load file rich text <mspace=0.65em>'<noparse>{filePath}</noparse>'</mspace> as it does not exist");
            }
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return [
                "PixLogicUtils.DoubleReadRam",
                "PixLogicUtils.Ram",
			];
        }
    }
}
