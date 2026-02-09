namespace PixLogicUtils.Shared.CustomData
{
	public interface IScreenData : IDisplayConfigurationData
	{
		int		Size { get; set; }
		int		ResolutionX { get; set; }
		int		ResolutionY { get; set; }
		byte[]	PixelData { get; set; }
		t_data	CurrentAddress { get; set; }
	}

    public static class ScreenDataInit
    {
        public static void Initialize(this IScreenData data)
		{
			data.BitsPerPixel = 1;
			data.ConfigurationIndex = 0;

			data.Size = 4;
			data.ResolutionX = 64;
			data.ResolutionY = 64;
			int bytesNeeded = (data.ResolutionX * data.ResolutionY * 8) / 8; // 4096 bytes for 8 BPP max
			data.PixelData = new byte[bytesNeeded];
			data.CurrentAddress = 0;
		}
    }
}
