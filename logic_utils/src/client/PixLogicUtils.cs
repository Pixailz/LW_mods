using LogicAPI.Client;
using System.Collections.Generic;
using PixLogicUtils.Client;
using PixLogicUtils.Client.Menus;
using LICC;
using System.IO;
using System;
using EccsLogicWorldAPI.Client.Hooks;
using LogicWorld;
using EccsLogicWorldAPI.Shared;

namespace PixLogicUtils
{
    public class PixLogicUtilsClient : ClientMod
    {
        public static List<FileLoadable> fileLoadables = new List<FileLoadable>();
		public static object harmony;

        protected override void Initialize()
        {
			HarmonyApplyPatches();
			LoadMenus();
            Logger.Info("[✔️] Client: loaded PixLogicUtils");
        }

		public void HarmonyPatchDiagramState(object harmony)
		{
			// Patch DiagramStateSelector.SetInputsReference (internal type, use reflection)
			var diagramStateSelectorType = Type.GetType("LogicWorld.UI.Displays.DiagramStateSelector, LogicWorld.UI");
			if (diagramStateSelectorType == null)
			{
				Logger.Warn("⚠️ Could not find DiagramStateSelector type");
				return;
			}

			var originalMethod = diagramStateSelectorType
				.GetMethod("SetInputsReference", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

			var prefixMethod = typeof(DiagramStateSelectorPatch)
				.GetMethod("SetInputsReferencePrefix", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

			if (originalMethod != null && prefixMethod != null)
			{
				HarmonyAtRuntime.patch(harmony, originalMethod, prefix: prefixMethod);
				Logger.Info("✔️ Patched DiagramStateSelector.SetInputsReference");
			}
			else
			{
				Logger.Warn("⚠️ Could not find DiagramStateSelector.SetInputsReference or patch method");
			}
		}

		public void HarmonyApplyPatches()
		{
			try
			{
                harmony = HarmonyAtRuntime.getHarmonyInstance("PixLogicUtils");
				HarmonyPatchDiagramState(harmony);
			}
			catch (Exception e)
			{
				Logger.Error($"❌ Failed to apply Harmony patches: {e}");
			}
		}

		public void LoadMenus()
		{
            WorldHook.worldLoading += () => {
                try
                {
                    RamMenu.init();
                    DecoderMenu.init();
                    RegisterMenu.init();
                    HexDisplayMenu.init();
                    ScreenMenu.init();
                    DummyTestMenu.init();
                }
                catch(Exception e)
                {
                    Logger.Error("Could not initialize PixLogicUtils Menus");
                    SceneAndNetworkManager.TriggerErrorScreen(e);
                }
            };
		}

        [Command("loadfile", Description = "Loads a file into any RAM components with the L pin active, does not clear out memory after the end of the file")]
        public static void loadfile(string file)
        {
            LineWriter lineWriter = LConsole.BeginLine();
            if (File.Exists(file))
            {
                var bs = File.ReadAllBytes(file);
                foreach (var item in fileLoadables)
                    item.Load(bs, lineWriter, false);
            }
            else
            {
                lineWriter.WriteLine($"Attempted to load file {file} that does not exist!");
            }
            lineWriter.End();
        }
    }
}
