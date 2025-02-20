using System;
using EccsLogicWorldAPI.Client.Hooks;
using LogicAPI.Client;
using LogicWorld;

namespace PixLogicUtilsMenu.Client
{
    public class PixLogicUtilsMenuClient : ClientMod
    {
        protected override void Initialize()
        {
            WorldHook.worldLoading += () => {
                try
                {
                    RamMenu.init();
                    DecoderMenu.init();
                    RegisterMenu.init();
                }
                catch(Exception e)
                {
                    Logger.Error("Could not initialize PixLogicUtilsMenu");
                    SceneAndNetworkManager.TriggerErrorScreen(e);
                }
            };
        }
    }
}
