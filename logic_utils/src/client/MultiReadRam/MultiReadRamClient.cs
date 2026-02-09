using LogicWorld.Rendering.Components;
using System;
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
            try
            {
                if (force || GetInputState(CMultiReadRam.Pin.Load))
                {
                    if (filedata == null || filedata.Length == 0)
                    {
                        writer.WriteLine($"✗ Cannot load empty file");
                        return;
                    }

                    this.Data.ClientIncomingData = Compress(filedata);
                    this.Data.State = 1;
                    writer.WriteLine($"✓ Loaded {filedata.Length} bytes into RAM");
                }
                else
                {
                    writer.WriteLine($"⚠ Load pin (L) is not active - skipping this RAM");
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"✗ Failed to load data: {ex.Message}");
                Logger.Error($"[PixLogicUtils] Failed to load RAM data: {ex}");
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
            Data.Initialize();
			Data.ReadNumber = CodeInfoInts[0];
        }
    }
}
