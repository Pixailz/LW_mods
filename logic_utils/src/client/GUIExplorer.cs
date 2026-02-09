using UnityEngine;
using LICC;
using System.Linq;

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
    }
}
