using System;
using LogicWorld.Server.Circuitry;
using PixLogicUtils.Shared.Config;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Server
{
    public class ScreenServer : LogicComponent<IScreenData>
    {
        protected override void DoLogicUpdate()
        {
			int PixelCoverage = (int)Math.Floor(
				(float)(CScreen.DefaultDataSize / this.Data.BitsPerPixel)
			);
			t_data MaxAddress =
				(t_data)(this.Data.ResolutionX * this.Data.ResolutionY);

			// if (this.Data.PixelData == null || this.Data.PixelData.Length != (int)MaxAddress)
			// 	this.Data.PixelData = new byte[(int)MaxAddress];

			// DEBUG
			// if (this.Data.CurrentAddress >= 10)
			// {
			// 	this.Data.CurrentAddress = MaxAddress;
			// }

			// 1tick pulse
			if (Outputs[CScreen.Pin.EndPulse].On)
				Outputs[CScreen.Pin.EndPulse].On = false;
			// Set EndPulse before resetting
			if (this.Data.CurrentAddress >= MaxAddress)
				Outputs[CScreen.Pin.EndPulse].On = true;
			// Reset
			if (this.Data.CurrentAddress >= MaxAddress)
				this.Data.CurrentAddress = 0;
			if (!Inputs[CScreen.Pin.Clock].On)
				return ;
			// t_data InputData = Utils.InputToByte(
			// 	Inputs,
			// 	CScreen.DefaultDataSize,
			// 	CScreen.Pin.DataStart
			// );

			// Do logic
			for (int i = 0; i < PixelCoverage; i++)
			{
				byte pixelData = 0;
				int pinIndex = i * this.Data.BitsPerPixel;
				for (int j = this.Data.BitsPerPixel - 1; j >= 0; j--)
				{
					pixelData <<= 1;
					pixelData |= (byte)(Inputs[pinIndex + j + CScreen.Pin.DataStart].On ? 1 : 0);
				}
				int currentAddress = (int)this.Data.CurrentAddress + i;
				if (currentAddress >= (int)MaxAddress)
				{
					Logger.Info($"Attempt to write beyond max address: {currentAddress} >= {MaxAddress}");
					break;
				}
				this.Data.PixelData[currentAddress] = pixelData;
			}
			this.Data.CurrentAddress += (t_data)PixelCoverage;
			QueueLogicUpdate();
        }

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
    }
}
