using LogicAPI.Client;

namespace PixModManager
{
	public class PixModManagerClient : ClientMod
	{
		protected override void Initialize()
		{
			Logger.Info("[✔️] Client: loaded PixModManager");
		}
	}
}
