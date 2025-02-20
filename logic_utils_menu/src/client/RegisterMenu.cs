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
    public class RegisterMenu : EditComponentMenu, IAssignMyFields
    {
        public static void init()
        {
            WS.window("PixLogicUtilsMenu - Register")
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
                                .add(WS.textLine.setLocalizationKey("PixLogicUtilsMenu.RegisterWidth"))
                                .add(WS.slider
                                    .injectionKey(nameof(registerPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(4)
                                    .setMin(4)
                                    .setMax(32)
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

        private bool isComponentResizable;

        protected override void OnStartEditing()
        {
            if (FirstComponentBeingEdited.ClientCode is RegisterClient)
            {
				registerPegSlider.SetValueWithoutNotify(FirstComponentBeingEdited.Component.Data.OutputCount);
                bottomSection.SetActive(true);
                isComponentResizable = true;
            }
            else
            {
				registerPegSlider.SetValueWithoutNotify(FirstComponentBeingEdited.Component.Data.OutputCount);
                bottomSection.SetActive(false);
                isComponentResizable = false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            registerPegSlider.OnValueChangedInt += registerCountChanged;
        }

        private void registerCountChanged(int newRegisterWidth)
        {
            if(!isComponentResizable)
                return;
            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                newRegisterWidth + 7,
                newRegisterWidth
            ));
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return new string[] {
                "PixLogicUtils.Register",
            };
        }
    }
}
