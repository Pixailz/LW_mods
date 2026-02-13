namespace PixLogicUtils.Shared.CustomData
{
	public interface IDisplayConfigurationData
	{
		int BitsPerPixel { get; set; }
		int ConfigurationIndex { get; set; }
	}

	public static class DisplayConfigurationDataInit
	{
		public static void Initialize(this IDisplayConfigurationData data)
		{
			data.BitsPerPixel = 1;
			data.ConfigurationIndex = 0;
		}
	}
}