using EccsLogicWorldAPI.Client.UnityHelper;
using LogicAPI.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

using EccsLogicWorldAPI.Shared;

using PixLogicUtils.Client;
using LogicUI.MenuParts;

namespace PixModManager
{
	public class PixModManagerClient : ClientMod
	{
		protected override void Initialize()
		{
			Logger.Info("[✔️] Client: loaded PixModManager");

			SceneManager.sceneLoaded += GetModsMenu;
		}

		private GameObject ModsMenu;
		private GameObject Title;
		private GameObject OpenModsFolderButton;

		private void GetModsMenu(Scene scene, LoadSceneMode mode)
		{
			// LConsole.WriteLine($"+SCENE {scene.name} {mode}");

			if (scene.name == "UI_MainMenu" && mode == LoadSceneMode.Single)
			{
				// FindAndHookModsMenu();
				FindAndModifyModsMenu();
				SceneManager.sceneLoaded -= GetModsMenu;
				Logger.Info("Unsubscribed from sceneLoaded event");
			}
		}

		private void GetVanillaContent()
		{
			// gui_inspect_game_object "Mods Menu"
			ModsMenu = GameObjectQuery.queryGameObject(
				"Mods Menu"
			);
			NullChecker.check(ModsMenu, "Could not find Mods Menu");


			// gui_inspect_game_object "Mods Menu/Top Bar/Title"
			Title = GameObjectQuery.queryGameObject(
				ModsMenu,
				"Top Bar",
				"Title"
			);
			NullChecker.check(Title, "Could not find Title in Mods Menu");

			// gui_inspect_game_object "Mods Menu/Contents/Right side/Open Mods Folder button"
			OpenModsFolderButton = GameObjectQuery.queryGameObject(
				ModsMenu,
				"Contents",
				"Right side",
				"Open Mods Folder button"
			);
			NullChecker.check(OpenModsFolderButton, "Could not find Open Mods Folder button in Mods Menu");
		}

		private void FindAndModifyModsMenu()
		{
			this.GetVanillaContent();

			this.SetModsMenuTitle();
			// this.AddUpdateButton();
		}

		private void SetModsMenuTitle()
		{
			if (!UnityHelper.ReplaceText(Title, "Better Mod Manager"))
			{
				Logger.Warn("Failed to replace localized text for Mods Menu title");
			}
			else
			{
				Logger.Info("Successfully replaced Mods Menu title");
			}
		}

		private void AddUpdateButton()
		{
			// gui_inspect_game_object "Mods Menu/Contents/Right side/Disclaimer"
			GameObject Contents = GameObjectQuery.queryGameObject(
				ModsMenu,
				"Contents",
				"Right side",
				"Disclaimer"
			);
			NullChecker.check(Contents, "Could not find Contents in Mods Menu");

			GameObject UpdateButtonObj = GameObject.Instantiate(
				OpenModsFolderButton,
				OpenModsFolderButton.transform.parent
			);

			UpdateButtonObj.name = "Check for Updates Button";
			UnityHelper.ReplaceText(UpdateButtonObj, "Check for Updates");
			UpdateButtonObj.GetComponent<HoverButton>().OnClickEnd += () =>
			{
				Logger.Info("Check for Updates button clicked!");
				// Here you can add the logic to check for updates and download them
			};

			// gui_inspect_game_object "Mods Menu/Contents/Right side/Open Mods Folder button/Icon area/Icon"
			GameObject IconArea = GameObjectQuery.queryGameObject(
				UpdateButtonObj,
				"Icon area",
				"Icon"
			);
			NullChecker.check(IconArea, "Could not find Icon Area in Check for Updates button");
			UnityHelper.ReplaceText(IconArea, "⬆️");

			Contents.addChild(UpdateButtonObj);
		}

		// private void FindAndHookModsMenu()
		// {
		// 	// Hook Mods menu to add our own options
		// 	GameObject ModsButton = GameObjectQuery.queryGameObject(
		// 		"Main Canvas",
		// 		"Buttons",
		// 		"Mods Button (1)"
		// 	);
		// 	if (ModsButton == null)
		// 	{
		// 		Logger.Error("Could not find Mods Button");
		// 		return ;
		// 	}

		// 	Logger.Info("Found Mods Button");

		// 	// Add our own OnClick listener to the Mods button
		// 	HoverButton button = ModsButton.GetComponent<HoverButton>();
		// 	if (button == null)
		// 	{
		// 		Logger.Error("Could not find Button component on Mods Button");
		// 		return ;
		// 	}

		// 	RemoveAllListeners(button, "OnClickEnd");

		// 	// Inspect existing listeners

		// }

		// private void RemoveAllListeners(object target, string eventName)
		// {
		// 	if (target == null)
		// 	{
		// 		return;
		// 	}

		// 	var type = target.GetType();
		// 	FieldInfo field = null;
		// 	Type current = type;
		// 	while (current != null)
		// 	{
		// 		field = current.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		// 		if (field != null)
		// 		{
		// 			break;
		// 		}
		// 		current = current.BaseType;
		// 	}

		// 	var eventInfo = type.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		// 	if (field == null)
		// 	{
		// 		Logger.Error($"Could not find event field '{eventName}' on {type.FullName}");
		// 		return;
		// 	}

		// 	if (eventInfo == null)
		// 	{
		// 		Logger.Error($"Could not find event info '{eventName}' on {type.FullName}");
		// 		return;
		// 	}

		// 	if (field.GetValue(target) is Delegate del)
		// 	{
		// 		foreach (var handler in del.GetInvocationList())
		// 		{
		// 			Logger.Info($"Removing listener: Method={handler.Method.Name}, Target={(handler.Target != null ? handler.Target.GetType().FullName : "Static")}");
		// 			eventInfo.RemoveEventHandler(target, handler);
		// 		}
		// 	}

		// 	// field.SetValue(target, null);
	}
}
