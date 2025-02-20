using LogicAPI.Client;
using System.Collections.Generic;
using PixLogicUtils.Client;
using LICC;
using System.IO;

namespace PixLogicUtils
{
	public class PixLogicUtilsClient : ClientMod
	{
		public static List<FileLoadable> fileLoadables = new List<FileLoadable>();

		protected override void Initialize()
		{
			Logger.Info("[✔️] Client: loaded PixLogicUtils");
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
