using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using PixLogicUtils.Shared.CustomData;
using LICC;

namespace PixLogicUtils.Client
{
	public class MultiReadRamClient : ComponentClientCode<IMultiReadRamData>, FileLoadable
	{
		private	int PEG_L = 0;

		public void Load(byte[] filedata, LineWriter writer, bool force)
		{
			if (force || GetInputState(PEG_L))
			{
				Data.ClientIncomingData = Compress(filedata);
				Data.State = 1;
			}
		}

		public byte[] Compress(byte[] data)
		{
			MemoryStream output = new MemoryStream();
			using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
			{
				dstream.Write(data, 0, data.Length);
			}
			return output.ToArray();
		}

		protected override void SetDataDefaultValues()
		{
			Data.initialize();
		}
	}
}
