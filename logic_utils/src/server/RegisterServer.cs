using LogicWorld.Server.Circuitry;

using PixLogicUtils.Shared.CustomData;
using PixLogicUtils.Shared.Config;
using PixLogicUtils.Shared.Utils;

namespace PixLogicUtils.Server
{
	public class RegisterServer : LogicComponent<IRegData>
	{
		public override bool	HasPersistentValues => true;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}

		private void	ByteToOutputMode(int mode, t_width currentSize)
		{
			t_data value_tmp = this.Data.Value;

			if (mode == 1)
				value_tmp &= (t_data)~(1 << currentSize);
			else if (mode == 2)
				value_tmp >>= currentSize;
			Utils.ByteToOutput(Outputs, value_tmp, currentSize);
		}

		private t_data	InputToByteMode(int mode, t_width currentSize, t_pin startData)
		{
			t_data	retv = this.Data.Value;

			if (mode == 1)
				retv |= Utils.InputToByte(Inputs, currentSize, startData);
			else if (mode == 2)
				retv |= Utils.InputToByte(Inputs, currentSize, startData) << currentSize;
			else
				retv = Utils.InputToByte(Inputs, currentSize, startData);
			return retv;
		}


		private	void	DebugInput(int mode, t_width currentSize, t_pin startData)
		{
			Logger.Info(
				"LogicUpdate; " +
				$"mode = {mode}, " +
				$"currentSize = {currentSize}, " +
				$"startData = {startData}"
			);
			Logger.Info("Pin Clock   " + Converter.ToString(
				Inputs[CRegister.Pin.Clock].On)
			);
			Logger.Info("Pin Write   " + Converter.ToString(
				Inputs[CRegister.Pin.Write].On)
			);
			Logger.Info("Pin Read    " + Converter.ToString(
				Inputs[CRegister.Pin.Read].On)
			);
			Logger.Info("Pin Low     " + Converter.ToString(
				Inputs[CRegister.Pin.Low].On)
			);
			Logger.Info("Pin High    " + Converter.ToString(
				Inputs[CRegister.Pin.High].On)
			);
			Logger.Info("Pin +1      " + Converter.ToString(
				Inputs[CRegister.Pin.Plus].On)
			);
			Logger.Info("Pin -1      " + Converter.ToString(
				Inputs[CRegister.Pin.Minus].On)
			);
			Logger.Info("Data        " + InputToByteMode(mode, currentSize, startData));
			Logger.Info("");
		}

		protected override void DoLogicUpdate()
		{
			t_width currentSize = Outputs.Count;
			t_pin startData = CRegister.Pin.DataStart;
			int mode = 0;

			if (Inputs[CRegister.Pin.Low].On && Inputs[CRegister.Pin.High].On)
			{
				mode = 3;
				currentSize = 0;
			}
			else if (Inputs[CRegister.Pin.Low].On)
			{
				mode = 1;
				currentSize /= 2;
			}
			else if (Inputs[CRegister.Pin.High].On)
			{
				mode = 2;
				currentSize /= 2;
				startData += currentSize;
			}

			Utils.ResetOutput(Outputs);
			// DebugInput(mode, currentSize, startData);

			if (mode == 3)
				return ;

			if (Inputs[CRegister.Pin.Clock].On)
			{
				if (Inputs[CRegister.Pin.Write].On)
				{
					this.Data.Value = InputToByteMode(mode, currentSize, startData);
				}
				if (Inputs[CRegister.Pin.Plus].On)
				{
					this.Data.Value++;
					QueueLogicUpdate();
				}
				else if (Inputs[CRegister.Pin.Minus].On)
				{
					this.Data.Value--;
					QueueLogicUpdate();
				}
			}

			if (Inputs[CRegister.Pin.Read].On)
			{
				ByteToOutputMode(mode, currentSize);
			}
		}
	}
}
