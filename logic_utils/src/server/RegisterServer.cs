using LogicWorld.Server.Circuitry;

using System;

using PixLogicUtils.Shared.CustomData;

using System.IO;
using System.IO.Compression;

using LICC;

namespace PixLogicUtils.Components
{
	public class RegisterServer : LogicComponent<IRegData>
	{
		public override bool HasPersistentValues => true;

		private int				dataWidth;

		private int				value;

        private bool			loadFromSave;

		private readonly int	Pclk = 0;
		private readonly int	Pwrite = 1;
		private readonly int	Pread = 2;
		private readonly int	Plow = 3;
		private readonly int	Phigh = 4;
		private readonly int	Pplus = 5;
		private readonly int	Pminus = 6;

		private int				currentSize;
		private int				startData;

		protected override void SetDataDefaultValues()
		{
			Data.initialize();
		}

		protected override void Initialize()
		{
			loadFromSave = true;

			setupValue(8);
		}

		protected void setupValue(int value)
		{
			Data.dataWidth = dataWidth = value;
			Data.value = value = 0;
		}

		private byte	inputToByte(int start, int size)
		{
			byte tmp = 0;

			// for (var i = start; i < start + size; i++)
			for (var i = start + size - 1; i >= start; i--)
			{
				tmp <<= 1;
				if (Inputs[i].On)
					tmp++;
			}
			return tmp;
		}

		private void	byteToOutput(int value, int start, int size)
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

		public void	updateInputOutput()
		{
			if (dataWidth == Outputs.Count)
				return ;

			setupValue(Outputs.Count);
		}

		public void	resetOutput()
		{
			foreach (var Output in Outputs)
				Output.On = false;
		}

		private	bool	isOn(int pin)
		{
			return (Inputs[pin].On);
		}

		private int	getMode()
		{
			int mode = 0;
			this.currentSize = dataWidth;
			this.startData = 7;

			if (isOn(Plow))
			{
				mode = 1;
				this.currentSize /= 2;
			}
			else if (isOn(Phigh))
			{
				mode = 2;
				this.currentSize /= 2;
				this.startData += this.currentSize;
			}
			return (mode);
		}

		private void	byteToOutputMode(int mode)
		{
			int value_tmp = this.value;

			if (mode == 1)
				value_tmp &= ~(1 << (dataWidth / 2));
			else if (mode == 2)
				value_tmp >>= (dataWidth / 2);
			byteToOutput(value_tmp, 0, dataWidth);
		}

		private byte	inputToByteMode(int mode)
		{
			int	retv = 0;

			if (mode == 1)
				retv = ((this.value & (~(1 << this.currentSize))) << this.currentSize) | inputToByte(start, this.currentSize);
			else if (mode == 2)
			{
				retv = (this.value & (~(1 << this.currentSize))) | (inputToByte(start, this.currentSize) << this.currentSize);
			}
			else
				retv = inputToByte(start, size);
			return (byte)retv;
		}

		private	string _debugInput(bool v)
		{
			if (v)
				return "True";
			else
				return "False";
		}

		private	void	debugInput()
		{
			LConsole.WriteLine("Pin Clock   " + _debugInput(isOn(Pclk)));
			LConsole.WriteLine("Pin Write   " + _debugInput(isOn(Pwrite)));
			LConsole.WriteLine("Pin Read    " + _debugInput(isOn(Pread)));
			LConsole.WriteLine("Pin Low     " + _debugInput(isOn(Plow)));
			LConsole.WriteLine("Pin High    " + _debugInput(isOn(Phigh)));
			LConsole.WriteLine("Pin +1      " + _debugInput(isOn(Pplus)));
			LConsole.WriteLine("Pin -1      " + _debugInput(isOn(Pminus)));
		}

		protected override void DoLogicUpdate()
		{
			updateInputOutput();
			resetOutput();

			// debugInput();
			int mode = getMode();

			if (isOn(Pclk))
			{
				if (isOn(Pwrite))
				{
					LConsole.WriteLine(this.value);
					this.value = inputToByteMode(mode);
					LConsole.WriteLine(this.value);
				}
			}
			if (isOn(Pread))
			{
				byteToOutputMode(mode);
			}
		}

		protected override void OnCustomDataUpdated()
		{
			if (loadFromSave)
			{
				dataWidth = Data.dataWidth;
				value = Data.value;
				loadFromSave = false;
				QueueLogicUpdate();
			}
		}

        public override void Dispose()
        {}

        protected override void SavePersistentValuesToCustomData()
        {
            Data.dataWidth = dataWidth;
            Data.value = value;
        }
	}
}
