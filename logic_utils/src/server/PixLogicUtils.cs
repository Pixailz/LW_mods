using LogicAPI.Server;

namespace PixLogicUtils.Server
{
    class PixLogicUtilsServer : ServerMod
    {
        protected override void Initialize()
        {
			Logger.Info("[✔️] Server: loaded PixLogicUtils");
        }
    }
}
