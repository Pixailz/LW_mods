using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LogicWorld.UI.Displays;
using LogicUI.MenuParts;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using EccsLogicWorldAPI.Shared;
using EccsLogicWorldAPI.Client.UnityHelper;
using UnityEngine.UI;
using LogicLocalization;
using TMPro;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Client.Menus
{
    public abstract class DisplayConfigurationMenuBase : EditComponentMenu, IAssignMyFields
    {
        private const int DEFAULT_MIN_WIDTH = 1000;
        private const int DEFAULT_MIN_HEIGHT = 1500;

        protected virtual int MinBPP => 1;
        protected virtual int MaxBPP => 9;
		protected virtual string BitsPerPixelName => "Bits Per Pixel";
        protected abstract IEnumerable<string> ComponentTypeIDs { get; }

        [AssignMe]
		public GameObject displayConfigContainer = null!;

        private DisplayConfigurationsList configurationsList = null!;
        private InputSlider bitsPerPixelSlider = null!;
		private HoverButton editConfigurationsButton = null!;

        private static GameObject vanillaEditDisplayMenuContent = null!;

		public static GameObject getVanillaEditDisplayMenuContent()
		{
            if (vanillaEditDisplayMenuContent == null)
            {
				vanillaEditDisplayMenuContent = GameObjectQuery
					.queryGameObject([
						"Edit Display Menu",
						"Menu",
						"Menu Content"
					]);
				NullChecker.check(
					vanillaEditDisplayMenuContent,
					$"Could not find menu at path: {string.Join(
						" / ",
						["Edit Display Menu", "Menu", "Menu Content"]
					)}"
				);
            }
			return vanillaEditDisplayMenuContent;
		}

        public static GameObject FindChildByName(GameObject parent, string name)
        {
            if (parent.name == name)
                return parent;

            foreach (Transform child in parent.transform)
            {
                var result = FindChildByName(child.gameObject, name);
                if (result != null)
                    return result;
            }

            return null!;
        }

		public static void ReplaceLocalizedText(GameObject parent, string childName, string newText)
        {
            var child = FindChildByName(parent, childName);
            if (child != null)
            {
                var localizedText = child.GetComponentInChildren<LocalizedTextMesh>();
                if (localizedText != null)
                {
                    var tmpText = localizedText.GetComponent<TMP_Text>();
                    UnityEngine.Object.DestroyImmediate(localizedText);
                    if (tmpText != null)
                    {
                        tmpText.text = newText;
                    }
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            var layoutElement = displayConfigContainer.AddComponent<LayoutElement>();
            layoutElement.minWidth = DEFAULT_MIN_WIDTH;
            layoutElement.minHeight = DEFAULT_MIN_HEIGHT;
            configurationsList = displayConfigContainer.GetComponentInChildren<DisplayConfigurationsList>();
            configurationsList.OnSelectionChanged += OnConfigurationSelected;

			this.InitializeSlider();
			this.InitializeEditConfigurationsButton();
        }

		private void InitializeSlider()
		{
			bitsPerPixelSlider = displayConfigContainer.GetComponentInChildren<InputSlider>();
			NullChecker.check(bitsPerPixelSlider, "Could not find bitsPerPixelSlider in displayConfigContainer");

			bitsPerPixelSlider.Min = MinBPP;
			bitsPerPixelSlider.Max = MaxBPP;
			bitsPerPixelSlider.OnValueChanged += OnBitsPerPixelChanged;
			if (MinBPP == MaxBPP)
			{
				// If BPP is fixed, hide the slider
				bitsPerPixelSlider.gameObject.SetActive(false);
				ReplaceLocalizedText(displayConfigContainer, "Input count slider", "");
			}
			else
				ReplaceLocalizedText(displayConfigContainer, "Input count slider", BitsPerPixelName);
		}

		private void InitializeEditConfigurationsButton()
		{
			var exactTarget = FindChildByName(displayConfigContainer, "Edit Configurations Button");
            if (exactTarget != null)
            {
                editConfigurationsButton = exactTarget.GetComponent<HoverButton>();
            }
			NullChecker.check(editConfigurationsButton, "Could not find editConfigurationsButton in displayConfigContainer");

			editConfigurationsButton.OnClickEnd += OnEditConfigurationsClicked;
		}

        protected override void OnStartEditing()
		{
			this.SetupDisplayConfigMenu();
		}

        protected sealed override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return ComponentTypeIDs;
        }


		public void SetupDisplayConfigMenu()
		{
			var currentBpp = GetCurrentBitsPerPixel();

			bitsPerPixelSlider.SetValueWithoutNotify(currentBpp);
			configurationsList.SetupMenu(currentBpp);
            configurationsList.SelectedIndex = GetCurrentConfigurationIndex();
		}

        private void OnEditConfigurationsClicked()
        {
            var clientCodeWrapped = new ComponentClientCodeWrapper(
				FirstComponentBeingEdited.ClientCode,
				GetCurrentBitsPerPixel()
			);
            DisplayConfigurationsMenu.OpenMenu(
				clientCodeWrapped,
				GetCurrentConfigurationIndex()
			);
        }

        private void OnConfigurationSelected(int index)
        {
            SetCurrentConfigurationIndex(index);
        }

        private void OnBitsPerPixelChanged(float bpp)
        {
			foreach (var Component in ComponentsBeingEdited)
			{
				(
					Component.ClientCode.CustomDataObject
					 as IDisplayConfigurationData
				).BitsPerPixel = (int)bpp;
			}
            this.SetupDisplayConfigMenu();
        }

        protected int GetCurrentBitsPerPixel()
        {
			var Data = FirstComponentBeingEdited.ClientCode.CustomDataObject
				as IDisplayConfigurationData;

            return Data.BitsPerPixel;
        }

		// private void OnConfirmChangesClicked()
		// {
		// 	LConsole.WriteLine("Confirm Changes clicked");
		// }

        protected int GetCurrentConfigurationIndex()
        {
			var Data = FirstComponentBeingEdited.ClientCode.CustomDataObject
				as IDisplayConfigurationData;

            return Data.ConfigurationIndex;
        }

        protected void SetCurrentConfigurationIndex(int index)
        {
			foreach (var Component in ComponentsBeingEdited)
			{
				(
					Component.ClientCode.CustomDataObject
					as IDisplayConfigurationData
				).ConfigurationIndex = index;
			}
        }
    }
}
