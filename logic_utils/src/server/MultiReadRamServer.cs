using LogicWorld.Server.Circuitry;

using System;

using PixLogicUtils.Shared.CustomData;

using System.IO;
using System.IO.Compression;

namespace PixLogicUtils.Components
{
	public abstract class MultiReadRamServer : LogicComponent<IMultiReadRamData>
	{
		public override bool HasPersistentValues => true;

        public abstract int		readNumber { get; }
		private int		dataWidth;
		private int		addressWidth;

		private ulong	sizeInByte;
		private ulong	totalSizeInByte;

		private int		pinWrite;
		private int[]	pinsRead;

		private ulong[]	addresses;

        private bool	loadFromSave;

		private byte[]	memory;

		protected override void SetDataDefaultValues()
		{
			Data.initialize();
		}

		protected override void Initialize()
		{
			int		index = 0;

			dataWidth = 4;

			addressWidth =  8;

			index = 1 + dataWidth + (addressWidth * readNumber);

			pinWrite = index++;

			pinsRead = new int[readNumber];
			for (int i = 0; i < readNumber; i++)
				pinsRead[i] = index++;

			sizeInByte = upperWidth(dataWidth);

			totalSizeInByte = (ulong)(1 << addressWidth) * sizeInByte;

			addresses = new ulong[readNumber];
			memory = new byte[totalSizeInByte];

			Data.dataWidth = dataWidth;
			Data.addressWidth = addressWidth;

			loadFromSave = true;
		}

		private static ulong	upperWidth(int width)
		{
			ulong div = (ulong)width / 8;
			ulong mod = (ulong)width % 8 > 0 ? 1ul : 0ul;

			return div + mod;
		}

		private ulong	inputToByte(int start, int size)
		{
			ulong tmp = 0;

			// for (var i = start; i < start + size; i++)
			for (var i = start + size - 1; i >= start; i--)
			{
				tmp <<= 1;
				if (Inputs[i].On)
					tmp++;
			}
			return tmp;
		}

		private void	byteToOutput(ulong value, int start, int size)
		{
			int index = 0;
			for (int i = start; i < start + size; i++)
			{
				if ((value & 1) > 0)
					Outputs[index].On = true;
				else
					Outputs[index].On = false;
				value >>= 1;
				index++;
			}
		}

		private void	debug_getoutput()
		{
			string	str = "RamMultiRead: Server:\n";

			str += "  Data       " + inputToByte(0, dataWidth) + "\n";
			for (var i = 0; i < readNumber; i++)
			{
				str += "  Address    " + (i + 1) + " ";
				str += inputToByte(dataWidth + (addressWidth * i), addressWidth) + "\n";
			}
			str += "  Write      " + (Inputs[pinWrite].On ? "On" : "Off");
			for (var i = 0; i < readNumber; i++)
			{
				str += "\n  Read       " + (i + 1) + "     ";
				str += Inputs[pinsRead[i]].On ? "On" : "Off";
			}
			Logger.Info(str);
		}

		private void	debug_setoutput()
		{
			// for (var i = 0; i < readNumber; i++)
			// 	byteToOutput(inputToByte(dataWidth + (addressWidth * i), addressWidth), (addressWidth * i), addressWidth);
		}

		private void	updateAddresses()
		{
			for (int i = 0; i < readNumber; i++)
			{
				addresses[i] = inputToByte(dataWidth + (addressWidth * i) + 1, addressWidth);
			}
		}

		public void	updateInputOutput()
		{
			int	newDataWidth = (int)(Outputs.Count / readNumber);
			int	newAddressWidth = (int)((Inputs.Count - (newDataWidth + 2 + readNumber)) / readNumber);

			if (dataWidth == newDataWidth && addressWidth == newAddressWidth)
				return ;

			dataWidth = newDataWidth;
			addressWidth = newAddressWidth;

			int index = 1 + dataWidth + (addressWidth * readNumber);

			pinWrite = index++;

			pinsRead = new int[readNumber];
			for (int i = 0; i < readNumber; i++)
				pinsRead[i] = index++;

			sizeInByte = upperWidth(dataWidth);

			totalSizeInByte = (ulong)(1 << addressWidth) * sizeInByte;

			addresses = new ulong[readNumber];
			memory = new byte[totalSizeInByte];

			Data.dataWidth = dataWidth;
			Data.addressWidth = addressWidth;
		}

		private void	setData(ulong data, ulong addr)
		{
			for (ulong i = 0; i < sizeInByte; i++)
			{
				memory[addr + i] = (byte)(data & 0xff);
				data >>= 8;
			}
		}

		private ulong	getData(ulong addr)
		{
			ulong data = 0;

			for (ulong i = 0; i < sizeInByte; i++)
			{
				data <<= 8;
				data |= memory[addr + i];
			}
			return data;
		}

		protected override void DoLogicUpdate()
		{
			// debug_getoutput();
			// debug_setoutput();

			ulong data;

			updateInputOutput();
			updateAddresses();

			if (Inputs[pinWrite].On)
			{
				data = inputToByte(1, dataWidth);
				setData(data, addresses[0]);
			}
			for (int i = 0; i < readNumber; i++)
			{
				data = 0;
				if (Inputs[pinsRead[i]].On)
				{
					data = getData(addresses[i]);
				}
				byteToOutput(data, addressWidth * i, dataWidth);
			}
		}

		protected override void OnCustomDataUpdated()
		{
			if (loadFromSave && Data.Data != null || Data.State == 1 && Data.ClientIncomingData != null)
			{
				var to_load_from = Data.Data;
				if (Data.State == 1)
				{
					Logger.Info("Loading data from client");
					to_load_from = Data.ClientIncomingData;
				}
				MemoryStream stream = new MemoryStream(to_load_from);
				stream.Position = 0;
				byte[] mem1 = new byte[memory.Length];
				try
				{
					DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
					int bytesRead;
					int nextStartIndex = 0;
					while((bytesRead = decompressor.Read(mem1, nextStartIndex, mem1.Length - nextStartIndex)) > 0){
						nextStartIndex += bytesRead;
					}
					Buffer.BlockCopy(mem1, 0, memory, 0, mem1.Length);
				}
				catch(Exception ex)
				{
					Logger.Error("[test_memory] Loading data from client failed with exception: " + ex);
				}
				loadFromSave = false;
				if (Data.State == 1)
				{
					Data.State = 0;
					Data.ClientIncomingData = new byte[0];
				}
				QueueLogicUpdate();
			}
		}

        public override void Dispose()
        {}

        protected override void SavePersistentValuesToCustomData()
        {
            MemoryStream memstream = new MemoryStream();
            memstream.Position = 0;
            DeflateStream compressor = new DeflateStream(memstream, CompressionLevel.Optimal, true);
            compressor.Write(memory, 0, memory.Length);
            compressor.Flush();
            int length = (int)memstream.Position;
            memstream.Position = 0;
            byte[] bytes = new byte[length];
            memstream.Read(bytes, 0, length);
            Data.Data = bytes;
        }
	}
}
