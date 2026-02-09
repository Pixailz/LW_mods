using LogicAPI.Client;
using System.Collections.Generic;
using PixLogicUtils.Client;
using LICC;
using System.IO;

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
