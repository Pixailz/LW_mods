using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
	public interface IScreenData : IDisplayConfigurationData
	{
		int			Size { get; set; }
		int			ResolutionX { get; set; }
		int			ResolutionY { get; set; }
		int			DelayOnEndPulse {get; set; }
		byte[]		PixelData { get; set; }
		t_data		CurrentAddress { get; set; }
	}

	public static class ScreenDataInit
	{
		public static void Initialize(this IScreenData data)
		{
			data.BitsPerPixel = CScreen.DefaultBPP;
			data.ConfigurationIndex = 0;

			data.Size = CScreen.DefaultSize;
			data.ResolutionX = CScreen.DefaultResolutionX;
			data.ResolutionY = CScreen.DefaultResolutionX;
			data.DelayOnEndPulse = CScreen.DefaultDelayOnEndPulse;
			data.PixelData = new byte[data.ResolutionX * data.ResolutionY];
			data.CurrentAddress = 0;
		}
	}
}
