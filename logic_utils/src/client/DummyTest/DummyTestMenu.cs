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

            // Set fixed size on container with LayoutElement
            var layoutElement = fixedContainer.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 400;
            layoutElement.preferredHeight = 800;
            layoutElement.minWidth = 400;
            layoutElement.minHeight = 800;

            var rectTransform = fixedContainer.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(400, 800);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }

            // Add a background to see the container
            var image = fixedContainer.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Add a test text to see it works
            var textObj = new GameObject("TestText");
            textObj.transform.SetParent(fixedContainer.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(380, 780);

            var text = textObj.AddComponent<TMPro.TextMeshProUGUI>();
            text.text = "Fixed Container Test\n400x800 pixels\nWorking!";
            text.fontSize = 24;
            text.alignment = TMPro.TextAlignmentOptions.Center;
            text.color = Color.white;
        }

        protected override void OnStartEditing()
        {
            // Nothing to do
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return new string[] {
                "PixLogicUtils.DummyTest",
            };
        }
    }
}
