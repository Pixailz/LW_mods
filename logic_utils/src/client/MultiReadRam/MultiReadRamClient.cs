using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using PixLogicUtils.Shared.CustomData;
using LICC;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class MultiReadRamClient : ComponentClientCode<IMultiReadRamData>, FileLoadable
	{
		public void Load(byte[] filedata, LineWriter writer, bool force)
		{
			if (force || GetInputState(CMultiReadRam.Pin.Load))
			{
				this.Data.ClientIncomingData = Compress(filedata);
				this.Data.State = 1;
				writer.WriteLine($"✓ Loaded {filedata.Length} bytes into RAM");
			}
		}

		static byte[] Compress(byte[] data)
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
			this.Data.Initialize();
		}
	}
}
