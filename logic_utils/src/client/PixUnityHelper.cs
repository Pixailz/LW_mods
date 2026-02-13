using EccsLogicWorldAPI.Client.UnityHelper;
using LICC;
using LogicLocalization;
using TMPro;
using UnityEngine;

namespace PixLogicUtils.Client
{
	public static class UnityHelper
	{
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

		public static bool ReplaceLocalizedText(GameObject parent, string childName, string newText)
		{
			var child = FindChildByName(parent, childName);
			if (child != null)
			{
				return ReplaceText(child, newText);
			}
			return false;
		}

		public static bool ReplaceLocalizedText(string parent, string childName, string newText)
		{
			var parentObj = GameObjectQuery.queryGameObject(
				parent.Split('/')
			);
			if (parentObj != null)
			{
				return ReplaceLocalizedText(parentObj, childName, newText);
			}
			else
			{
				LConsole.WriteLine($"Could not find parent GameObject: {parent}");
			}
			return false;
		}


		public static bool ReplaceText(GameObject child, string newText)
		{
			var localizedText = child.GetComponentInChildren<LocalizedTextMesh>();
			if (localizedText != null)
			{
				var tmpText = localizedText.GetComponent<TMP_Text>();
				UnityEngine.Object.DestroyImmediate(localizedText);
				if (tmpText != null)
				{
					tmpText.text = newText;
					return true;
				}
			}

			TextMeshProUGUI textMeshProUGUI = child.GetComponentInChildren<TextMeshProUGUI>();

			if (textMeshProUGUI != null)
			{
				textMeshProUGUI.text = newText;
				return true;
			}
			return false;
		}

		public static string GetText(GameObject child)
		{
			var localizedText = child.GetComponentInChildren<LocalizedTextMesh>();
			if (localizedText != null)
			{
				var tmpText = localizedText.GetComponent<TMP_Text>();
				if (tmpText != null)
				{
					return tmpText.text;
				}
			}
			return string.Empty;
		}
	}
}