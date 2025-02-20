using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LICC;
using LogicAPI.Data.BuildingRequests;
using TMPro;
using LogicUI.MenuParts;
using PixLogicUtils.Client;
using System.IO;
using EccsGuiBuilder.Client.Layouts.Controller;
using EccsGuiBuilder.Client.Layouts.Elements;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicWorld.BuildingManagement;

namespace PixLogicUtilsMenu.Client
{
    public class DecoderMenu : EditComponentMenu, IAssignMyFields
    {
        public static void init()
        {
            WS.window("PixLogicUtilsMenu - Decoder")
                .configureContent(content => content
                    .vertical(20f, new RectOffset(20, 20, 20, 20), expandHorizontal: true)
                    .addContainer("BottomBox", bottomBox => bottomBox
                        .injectionKey(nameof(bottomSection))
                        .vertical(anchor: TextAnchor.UpperCenter)
                        .addContainer("BottomInnerBox", innerBox => innerBox
                            .addAndConfigure<GapListLayout>(layout => {
                                layout.layoutAlignment = RectTransform.Axis.Vertical;
                                layout.childAlignment = TextAnchor.UpperCenter;
                                layout.elementsUntilGap = 0;
                                layout.countElementsFromFront = false;
                                layout.spacing = 20;
                                layout.expandChildThickness = true;
                            })
                            .addContainer("BottomBox1", container => container
                                .addAndConfigure<GapListLayout>(layout => {
                                    layout.layoutAlignment = RectTransform.Axis.Horizontal;
                                    layout.childAlignment = TextAnchor.MiddleCenter;
                                    layout.elementsUntilGap = 0;
                                    layout.spacing = 20;
                                })
                                .add(WS.textLine.setLocalizationKey("PixLogicUtilsMenu.DecoderWidth"))
                                .add(WS.slider
                                    .injectionKey(nameof(decoderPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(1)
                                    .setMin(1)
                                    .setMax(9)
                                )
                            )
                        )
                    )
                )
                .add<DecoderMenu>()
                .build();
        }

        [AssignMe]
        public InputSlider decoderPegSlider;
        [AssignMe]
        public GameObject bottomSection;

        private bool isComponentResizable;

        protected override void OnStartEditing()
        {
            if (FirstComponentBeingEdited.ClientCode is DecoderClient)
            {
				decoderPegSlider.SetValueWithoutNotify(FirstComponentBeingEdited.Component.Data.InputCount);
                bottomSection.SetActive(true);
                isComponentResizable = true;
            }
            else
            {
				decoderPegSlider.SetValueWithoutNotify(FirstComponentBeingEdited.Component.Data.InputCount);
                bottomSection.SetActive(false);
                isComponentResizable = false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            decoderPegSlider.OnValueChangedInt += decoderCountChanged;
        }

        private void decoderCountChanged(int newDecoderWidth)
        {
            if(!isComponentResizable)
                return;
            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                newDecoderWidth,
                1 << newDecoderWidth
            ));
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return new string[] {
                "PixLogicUtils.Decoder",
            };
        }
    }
}
