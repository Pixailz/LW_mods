using System;
using LogicWorld.Server.Circuitry;
using PixLogicUtils.Shared.Config;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Server
{
	public class ScreenServer : LogicComponent<IScreenData>
	{
		private bool endPulsed = false;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}

		protected override void DoLogicUpdate()
		{
			t_data MaxAddress =
				(t_data)(this.Data.ResolutionX * this.Data.ResolutionY);
			int PixelCoverage = (int)Math.Floor(
				(float)(CScreen.DefaultDataSize / this.Data.BitsPerPixel)
			);

			this.SyncData();
			this.UpdateEndPulse(MaxAddress, (t_data)PixelCoverage);

			// Reset
			if (this.Data.CurrentAddress >= MaxAddress)
			{
				this.Data.CurrentAddress = 0;
				this.endPulsed = false;
			}

			if (!Inputs[CScreen.Pin.Clock].On)
				return ;

			// Do logic
			// Logger.Info($"MaxAddress {MaxAddress}, PixelCoverage {PixelCoverage}, CurrentAddress {this.Data.CurrentAddress}");

			for (int i = 0; i < PixelCoverage; i++)
			{
				byte pixelData = 0;
				int pinIndex = i * this.Data.BitsPerPixel;
				for (int j = this.Data.BitsPerPixel - 1; j >= 0; j--)
				{
					pixelData <<= 1;
					int currentPinIndex = pinIndex + j + CScreen.Pin.DataStart;
					pixelData |= (byte)(Inputs[currentPinIndex].On ? 1 : 0);
				}
				int currentAddress = (int)this.Data.CurrentAddress + i;

				// if (currentAddress >= (int)MaxAddress)
				// {
				// 	Logger.Info($"Attempt to write beyond max address: {currentAddress} >= {MaxAddress}");
				// 	break;
				// }
				this.Data.PixelData[currentAddress] = pixelData;
			}
			this.Data.CurrentAddress += (t_data)PixelCoverage;
			QueueLogicUpdate();
		}


		private void	UpdateEndPulse(t_data MaxAddress, t_data PixelCoverage)
		{
			if (Outputs[CScreen.Pin.EndPulse].On)
				Outputs[CScreen.Pin.EndPulse].On = false;
			if (this.endPulsed)
				return ;

			t_data threshold = MaxAddress;
			if (this.Data.DelayOnEndPulse > 0)
				threshold = (t_data)this.Data.DelayOnEndPulse * PixelCoverage;
			else if (this.Data.DelayOnEndPulse < 0)
				threshold += (t_data)this.Data.DelayOnEndPulse * PixelCoverage;

			if (this.Data.CurrentAddress >= threshold)
			{
				Outputs[CScreen.Pin.EndPulse].On = true;
				this.endPulsed = true;
			}
		}

		private void SyncData()
		{
			int bufferLength = this.Data.ResolutionX * this.Data.ResolutionY;

			if (this.Data.PixelData.Length != bufferLength)
				this.Data.PixelData = new byte[bufferLength];
		}
	}
}
