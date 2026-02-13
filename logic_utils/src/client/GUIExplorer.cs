using UnityEngine;
using LICC;
using System.Linq;
using EccsLogicWorldAPI.Client.UnityHelper;
using LogicUI.MenuParts;
using UnityEngine.Rendering.VirtualTexturing;
using System.Reflection;
using System;

namespace PixLogicUtils.Client
{
	public static class GUIExplorer
	{
		[Command("gui_list", Description = "List all active menus and windows")]
		public static void ListActiveMenus()
		{
			var lineWriter = LConsole.BeginLine();
			lineWriter.WriteLine("=== Active Menus and Windows ===");

			var canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);

			foreach (var canvas in canvases.Where(c => c.gameObject.activeInHierarchy))
			{
				lineWriter.WriteLine($"Canvas: {canvas.name} (Layer: {canvas.sortingOrder})");
			}
			lineWriter.End();
		}

		private static readonly string[] black_listed_name = [
			"Icon area",
			// "Icon",
			// "Text"
		];

		[Command("gui_inspect", Description = "Inspect a specific menu or window by name")]
		public static void InspectMenu(string menuName)
		{
			var lineWriter = LConsole.BeginLine();
			lineWriter.WriteLine($"=== Inspecting Menu: {menuName} ===");

			var canvases = UnityEngine.Object.FindObjectsByType<Canvas>(
				FindObjectsSortMode.None
			);
			var targetCanvas = canvases.FirstOrDefault(
				c => c.name.Equals(
					menuName, System.StringComparison.OrdinalIgnoreCase
				)
			);

			if (targetCanvas != null)
			{
				lineWriter.WriteLine($"Found Canvas: {targetCanvas.name}");
				lineWriter.WriteLine($"Active: {targetCanvas.gameObject.activeInHierarchy}");
				lineWriter.WriteLine($"Sorting Order: {targetCanvas.sortingOrder}");
				lineWriter.WriteLine($"Render Mode: {targetCanvas.renderMode}");
				lineWriter.WriteLine($"World Camera: {(targetCanvas.worldCamera != null ? targetCanvas.worldCamera.name : "None")}");
				foreach (var graphic in targetCanvas.GetComponentsInChildren<UnityEngine.UI.Graphic>(true))
				{
					if (black_listed_name.Contains(graphic.name))
						continue;
					lineWriter.WriteLine($"- Graphic: {graphic.name} (Type: {graphic.GetType().Name}, Active: {graphic.gameObject.activeInHierarchy})");
				}
			}
			else
			{
				lineWriter.WriteLine($"No active canvas found with name: {menuName}");
			}

			lineWriter.End();
		}

		// gui_inspect_game_object "Mods Menu/Contents/Left side/Title"
		[Command("gui_inspect_game_object", Description = "Inspect a specific menu or window by name")]
		public static void InspectMenuGameObject(string ObjNameParts)
		{
			GameObject obj = GameObjectQuery.queryGameObject(
				ObjNameParts.Split('/')
			);
			var lineWriter = LConsole.BeginLine();
			if (obj == null)
			{
				lineWriter.WriteLine($"Could not find GameObject with path: {ObjNameParts}");
				lineWriter.End();
				return ;
			}
			lineWriter.WriteLine($"=== Inspecting Menu: {ObjNameParts} ===");
			var type = obj.GetType();
			if (type.GetProperty("name") != null)
			{
				lineWriter.WriteLine($"GameObject Name: {obj.name}");
			}
			else
				lineWriter.WriteLine("GameObject does not have a name property");

			lineWriter.WriteLine($"Active: {obj.activeSelf}");
			var canvas = obj.GetComponent<Canvas>();
			if (canvas == null)
			{
				lineWriter.WriteLine("Could not find Canvas component");
			}
			else
			{
				lineWriter.WriteLine($"Sorting Order: {canvas.sortingOrder}");
				lineWriter.WriteLine($"Render Mode: {canvas.renderMode}");
				lineWriter.WriteLine($"World Camera: {(canvas.worldCamera != null ? canvas.worldCamera.name : "None")}");
			}
			foreach (Transform child in obj.transform)
			{
				var graphic = child.GetComponent<UnityEngine.UI.Graphic>();
				if (graphic != null)
				{
					lineWriter.WriteLine($"- Graphic: {child.name} (Type: {graphic.GetType().Name}, Active: {child.gameObject.activeSelf})");
				}
				else
				{
					lineWriter.WriteLine($"- Child: {child.name} (Active: {child.gameObject.activeSelf})");
				}
			}
			lineWriter.End();
		}

		// gui_inspect_button_listener "Main Canvas/Buttons/Mods Button (1)"
		[Command("gui_inspect_button_listener", Description = "Inspect listeners of a specific button")]
		public static void InspectMenuListeners(string ObjNameParts)
		{
			var lineWriter = LConsole.BeginLine();
			GameObject obj = GameObjectQuery.queryGameObject(
				ObjNameParts.Split('/')
			);
			if (obj == null)
			{
				lineWriter.WriteLine($"Could not find GameObject with path: {ObjNameParts}");
				lineWriter.End();
				return ;
			}
			lineWriter.WriteLine($"=== Inspecting Listeners for: {ObjNameParts} ===");
			var button = obj.GetComponent<HoverButton>();

			if (button == null)
			{
				lineWriter.WriteLine("Could not find Button component");
				lineWriter.End();
				return ;
			}

			InspectHoverButtonEventListeners(lineWriter, button, "OnClickEnd");
			InspectHoverButtonEventListeners(lineWriter, button, "OnClickBegin");
			InspectHoverButtonEventListeners(lineWriter, button, "OnDoubleClick");
			InspectHoverButtonEventListeners(lineWriter, button, "OnPointerEnter");
			InspectHoverButtonEventListeners(lineWriter, button, "OnPointerExit");
			lineWriter.End();
		}

		private static void InspectHoverButtonEventListeners(
			LineWriter lineWriter,
			HoverButton button,
			string eventName
		)
		{
			var eventInfo = typeof(HoverButton).GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (eventInfo == null)
			{
				lineWriter.WriteLine($"Could not find {eventName} event");
				return ;
			}

			var field = typeof(HoverButton).GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (field == null)
			{
				lineWriter.WriteLine($"Could not find {eventName} event field");
				return ;
			}

			if (field.GetValue(button) is Delegate del)
			{
				var handlers = del.GetInvocationList();
				lineWriter.WriteLine($"Found {handlers.Length} listeners for {eventName}:");
				foreach (var handler in handlers)
				{
					lineWriter.WriteLine($"- Method: {handler.Method.Name}, Target: {(handler.Target != null ? handler.Target.GetType().FullName : "Static")}");
				}
			}
			else
			{
				lineWriter.WriteLine($"No listeners found for {eventName}");
			}
		}
	}
}
