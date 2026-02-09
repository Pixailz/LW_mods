using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
    public interface IHexDisplayData : IDisplayConfigurationData
    {
		int Size { get; set; }
    }

    public static class HexDisplayDataInit
    {
        public static void Initialize(this IHexDisplayData data)
		{
			data.BitsPerPixel = 1;
			data.ConfigurationIndex = 0;

			data.Size = CHexDisplay.DefaultSize;
		}
    }
}